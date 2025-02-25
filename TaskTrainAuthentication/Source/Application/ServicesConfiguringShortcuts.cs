using Microsoft.AspNetCore.Identity;
using TT.Auth.Entities;
using TT.Storage.Npgsql;
using TT.Auth.Constants;
using TT.Core;

namespace TT.Auth;

public static class ServicesConfiguringShortcuts
{
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

    public static IServiceCollection AddPublisherService(this IServiceCollection services) 
    {
        services.AddScoped<IMessagePublisher, RabbitMQPublisher>();
        services.Configure<RabbitMQPublisherOptions>(options => 
        {
            options.Host = "localhost";
        });
        return services;
    }
}
