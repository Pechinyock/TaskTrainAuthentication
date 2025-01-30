using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using TT.Core;

namespace TT.Auth;

public static class ServicesConfiguringShortcuts
{
    public static IServiceCollection AddSwaggerGenAuth(this IServiceCollection services) 
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    }
                    , new List<string>()
                }
            });
        });
        return services;
    }

    public static IServiceCollection AddJwtAuth(this IServiceCollection services) 
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => 
            {
                options.RequireHttpsMetadata = true;
                var authOptions = AuthenticationDefaults.GetDefaultOptions();
                options.TokenValidationParameters = AuthenticationDefaults.GetValidationParameters(authOptions);
            });

        return services;
    }

    public static IServiceCollection AddTokenGenerator(this IServiceCollection services) 
    {
        services.AddSingleton<ITokenService, TokenService>();
        return services;
    }

    public static IServiceCollection AddDatabaseMetaInfoService(this IServiceCollection services, string connectionString) 
    {
        if(String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        services.AddScoped<IDatabaseMetaInfoService, DatabaseMetaInfoService>();
        services.Configure<DatabaseMetaInfoOptions>(options => 
        {
            options.ConnectionString = connectionString;
        });

        return services;
    }
}
