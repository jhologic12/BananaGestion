using BananaGestion.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BananaGestion.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public string GenerateToken(Guid userId, string email, string role)
    {
        try
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60");

            _logger.LogInformation("JWT GenerateToken - Key present: {KeyPresent}, Key length: {KeyLength}", 
                !string.IsNullOrEmpty(key), key?.Length ?? 0);
            _logger.LogInformation("JWT Settings - Issuer: {Issuer}, Audience: {Audience}, ExpireMinutes: {ExpireMinutes}",
                issuer, audience, expireMinutes);

            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT Key not configured");
            }

            var claims = new[]
            {
                new System.Security.Claims.Claim("sub", userId.ToString()),
                new System.Security.Claims.Claim("email", email),
                new System.Security.Claims.Claim("role", role),
                new System.Security.Claims.Claim("jti", Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("JWT token generated successfully for {Email}", email);
            return tokenString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR generating JWT for {Email}: {Message}", email, ex.Message);
            throw;
        }
    }
}
