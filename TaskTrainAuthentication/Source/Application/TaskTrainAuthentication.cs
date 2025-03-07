﻿using TT.Core;

namespace TT.Auth;

public class TaskTrainAuthentication : ITTApp
{
    #region Startup initializer
    private class Initialize
    {
        private IConfiguration Configuration { get; }

        private readonly string _postgreSystemConnectionString;
        private readonly string _postgreWorkingConnectionString;

        public Initialize(IHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("Config/appsettings.json", false, true)
                .AddJsonFile($"Config/appsettings.{env.EnvironmentName}.json")
                .Build();

            _postgreSystemConnectionString = Configuration["Storage:PostgreSQL:SystemConnectionString"];
            _postgreWorkingConnectionString = Configuration["Storage:PostgreSQL:WorkingConnectionString"];
        }

        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddJwtAuth();
            services.AddAuthorizationWithPolicies();
            services.AddTokenGenerator();
            services.AddControllers();
            services.AddSwaggerGenAuth();
            services.AddNpgsqlUpdater(_postgreSystemConnectionString, _postgreWorkingConnectionString);
            services.AddPasswordHasher();
            services.AddUserService(_postgreWorkingConnectionString);
            services.AddRabbitMQPublisher("localhost");
        }

        public void Configure(IApplicationBuilder builder, IWebHostEnvironment env) 
        {
            builder.UseRouting();

            builder.UseAuthentication();
            builder.UseAuthorization();

            builder.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
            });
            builder.UseSwagger();
            builder.UseSwaggerUI();
        }
    }
    #endregion

    private IHost _app;

    public void Build(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureWebHostDefaults(webBuilder => 
        {
            webBuilder.UseStartup<Initialize>()
                .UseKestrel()
                .UseUrls("http://*:5000");
        });
        _app = builder.Build();
    }

    public void Run() => _app.Run();
}
