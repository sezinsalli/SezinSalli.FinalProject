using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Simpra.Core.Jwt;
using System.Configuration;
using System.Text;

namespace Simpra.Api.Extensions;

public static class JwtExtension
{
    public static JwtConfig JwtConfig { get; private set; }

    public static void AddJwtExtension(this IServiceCollection services, IConfiguration configuration)
    {
        JwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>();
        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, 
                ValidIssuer = JwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConfig.Secret)),
                ValidAudience = JwtConfig.Audience,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        });
    }
}
