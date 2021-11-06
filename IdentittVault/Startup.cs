using IdentittVault.Entities;
using IdentittVault.Repositories;
using IdentittVault.Services;
using IdentittVault.System;
using IdentittVault.System.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentittVault
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentittVaultContext>(options =>
            {
                string connectionString = Configuration.GetConnectionString("DefaultConnection");

                //Only for development when docker localhost address is different
                if (IsDevelopmentEnv() && IsRunningInContainer())
                    connectionString = Configuration.GetConnectionString("DockerConnection");

                options.UseMySQL(connectionString);
            });


            services.AddSingleton<IdentittVaultSecure>();

            //services.AddScoped<ICrudRepository<Key>, CrudRepository<Key>>();
            //services.AddScoped<ICrudService<Application>, CrudService<Application, ICrudRepository<Application>>>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            static bool IsDevelopmentEnv()
            {
                return "DEVELOPMENT".Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), StringComparison.OrdinalIgnoreCase);
            }

            static bool IsRunningInContainer()
            {
                return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != null && bool.Parse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"));
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //app.UseDeveloperExceptionPage();
            app.UseExceptionHandler("/api/error");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
