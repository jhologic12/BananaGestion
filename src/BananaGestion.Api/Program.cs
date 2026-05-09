using System.Text;
using BananaGestion.Application;
using BananaGestion.Infrastructure;
using BananaGestion.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Render sets PORT env var; use it for Kestrel
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BananaGestion API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<BananaDbContext>(options =>
{
    // Read from environment variable
    var conn = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    
    if (string.IsNullOrEmpty(conn))
        conn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
    
    if (string.IsNullOrEmpty(conn))
        conn = builder.Configuration.GetConnectionString("DefaultConnection");
    
    Console.WriteLine($"[DEBUG] Connection string source: {(string.IsNullOrEmpty(conn) ? "NULL" : "Found (" + conn.Length + " chars)")}");
    
    if (!string.IsNullOrEmpty(conn))
    {
        try
        {
            string normalizedConn;
            string host;
            int port;
            
            if (conn.StartsWith("postgresql://") || conn.StartsWith("postgres://"))
            {
                var uri = new Uri(conn);
                var userInfo = uri.UserInfo.Split(':');
                host = uri.Host;
                port = uri.Port > 0 ? uri.Port : 5432;
                
                var builder = new Npgsql.NpgsqlConnectionStringBuilder
                {
                    Host = host,
                    Port = port,
                    Database = uri.AbsolutePath.TrimStart('/'),
                    Username = userInfo.Length > 0 ? userInfo[0] : "",
                    Password = userInfo.Length > 1 ? userInfo[1] : "",
                    SslMode = Npgsql.SslMode.Require
                };
                normalizedConn = builder.ToString();
                Console.WriteLine($"[DEBUG] Parsed URI: {host}:{port}/{builder.Database}");
            }
            else
            {
                var npgsqlBuilder = new Npgsql.NpgsqlConnectionStringBuilder(conn);
                host = npgsqlBuilder.Host;
                port = npgsqlBuilder.Port;
                normalizedConn = npgsqlBuilder.ToString();
                Console.WriteLine($"[DEBUG] Parsed key=value: {host}:{port}");
            }
            
            // Try to resolve hostname to IPv4 (Render cannot reach Supabase via IPv6)
            try
            {
                var addresses = System.Net.Dns.GetHostAddresses(host);
                var ipv4 = addresses.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                if (ipv4 != null)
                {
                    var ipv4Builder = new Npgsql.NpgsqlConnectionStringBuilder(normalizedConn)
                    {
                        Host = ipv4.ToString()
                    };
                    normalizedConn = ipv4Builder.ToString();
                    Console.WriteLine($"[DEBUG] Resolved {host} -> {ipv4} (IPv4)");
                }
                else
                {
                    Console.WriteLine($"[WARN] No IPv4 address found for {host}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] DNS resolution failed: {ex.Message}");
            }
            
            // Increase connection timeout to 120s for slow Supabase pooler
            var connStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder(normalizedConn)
            {
                Timeout = 120
            };
            var finalConn = connStringBuilder.ToString();
            options.UseNpgsql(finalConn, npgsqlOptions => {
                npgsqlOptions.CommandTimeout(120);
                npgsqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
            });
            Console.WriteLine("[DEBUG] Npgsql configured successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Npgsql configuration failed: {ex.Message}");
            throw;
        }
    }
    else
    {
        Console.WriteLine("WARNING: No connection string found!");
    }
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(key))
        {
            Console.WriteLine("WARNING: JWT Key not configured, using temporary key for startup");
            key = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));
        }
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "BananaGestion",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "BananaGestionApp",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (allowedOrigins.Any())
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
        else
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("login", loginOptions =>
    {
        loginOptions.PermitLimit = 5;
        loginOptions.Window = TimeSpan.FromMinutes(1);
        loginOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        loginOptions.QueueLimit = 0;
    });
});

var app = builder.Build();

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Append("Permissions-Policy", "camera=(), microphone=(), geolocation=()");
    await next();
});

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        logger.LogError(exception, "Unhandled exception: {ExceptionType}: {Message}\nStack: {StackTrace}", 
            exception?.GetType().Name, exception?.Message, exception?.StackTrace);

        var showDetails = context.Request.Query.ContainsKey("debug");
        
        if (showDetails || context.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new { 
                error = "Error interno del servidor",
                details = exception?.Message,
                stack = exception?.StackTrace
            }));
            return;
        }

        if (exception is FluentValidation.ValidationException validationEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            var errors = validationEx.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new { errors }));
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\":\"Error interno del servidor\"}");
    });
});

app.UseCors("AllowFrontend");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Initialize database in background (non-blocking for Render port detection)
_ = Task.Run(async () =>
{
    await Task.Delay(2000);
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BananaDbContext>();
        var canConnect = await db.Database.CanConnectAsync();
        if (canConnect)
        {
            await db.Database.MigrateAsync();
            Console.WriteLine("Database initialized successfully");
        }
        else
        {
            Console.WriteLine("WARNING: Cannot connect to database!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR initializing database: {ex.Message}");
    }
});

app.Run();

public partial class Program { }
