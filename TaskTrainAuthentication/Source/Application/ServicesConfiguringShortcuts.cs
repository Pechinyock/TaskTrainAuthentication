using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using TT.Core;
using TT.Auth.Entities;
using TT.Storage.Npgsql;
using TT.Auth.Constants;

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
        var auth = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        auth.AddJwtBearer(options => 
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

    public static IServiceCollection AddUserService(this IServiceCollection services, string repoConnectionString) 
    {
        services.AddTransient<IUserService, UserService>();
        services.Configure<UserServiceOptions>(options => 
        {
            options.ConnectionString = repoConnectionString;
        });
        return services;
    }

    public static IServiceCollection AddPasswordHasher(this IServiceCollection services) 
    {
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        return services;
    }

    public static IServiceCollection AddAuthorizationWithPolicies(this IServiceCollection services) 
    {
        services.AddAuthorization(options => 
        {
            options.AddPolicy(Policy.AccessLayer.Admin
                , policy => policy.Requirements.Add(new AdminAccessLayer())
            );
        });
        return services;
    }
}
