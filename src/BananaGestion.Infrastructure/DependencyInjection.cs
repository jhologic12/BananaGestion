using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Infrastructure.Data;
using BananaGestion.Infrastructure.Repositories;
using BananaGestion.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BananaGestion.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BananaDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection") ?? 
                "Host=localhost;Database=bananagestion;Username=postgres;Password=postgres"));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IHarvestRepository, HarvestRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IFinancialRepository, FinancialRepository>();

        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
