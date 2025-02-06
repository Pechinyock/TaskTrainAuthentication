using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using TT.Core;
using TT.Storage.Npgsql;

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

    public static IServiceCollection AddNpgsqlUpdater(this IServiceCollection services, string systemConnectionString, string workingConnectionString) 
    {
        var pgUpdater = new NpgsqlDatabaseUpdater(systemConnectionString, Path.Combine(AppContext.BaseDirectory, "Postgres"));
        NpgsqlConnectionParameters.Parse(workingConnectionString, out var workingDbParams);
        if (!pgUpdater.IsDatabaseInitialized(workingDbParams.Database))
        {
            pgUpdater.Initialize();
        }

        services.AddScoped<IUpdateService, UpdateService>();
        services.Configure<UpdaterOptions>(options => 
        {
            options.DatabaseConnectionString = workingConnectionString;
            options.MigrationsFolederPath = Path.Combine(AppContext.BaseDirectory, "Postgres");
        });
        return services;
    }
}
