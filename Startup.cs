using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortfolioServer.Api.Authorization;
using PortfolioServer.Api.Configuration;
using PortfolioServer.Api.Entity;
using PortfolioServer.Api.Repository;

namespace PortfolioServer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: Secure database credentials (Google Cloud Secret Manager)
            var dbConfig = Configuration.GetSection(DatabaseConfiguration.Section)
                                        .Get<DatabaseConfiguration>();

            services.AddDbContext<UserDbContext>(options => 
            {
                var serverVersion = ServerVersion.AutoDetect(dbConfig.ConnectionString);
                options.UseMySql(dbConfig.ConnectionString, serverVersion);
            });
            services.AddDbContext<PortfolioDbContext>(options => 
            {
                var serverVersion = ServerVersion.AutoDetect(dbConfig.ConnectionString);
                options.UseMySql(dbConfig.ConnectionString, serverVersion).LogTo(Console.WriteLine, LogLevel.Information);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IAuthorizationHandler, UserAuthorizationHandler>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddGoogleIdTokenValidation(Configuration);
            
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = 
                    new AuthorizationPolicyBuilder()
                        .RequireClaim(ClaimTypes.NameIdentifier)
                        .Build();

                options.AddPolicy(Policies.UserExists, policy =>
                    policy.Requirements.Add(new UserExistsRequirement()));
            });

             services.AddControllers(options => {
                options.SuppressAsyncSuffixInActionNames = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
