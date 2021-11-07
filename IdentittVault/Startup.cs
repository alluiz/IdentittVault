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
using MySql.Data.MySqlClient;
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

                if (IsRunningInContainer())
                    connectionString = Configuration.GetConnectionString("DockerConnection");

                MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
                connectionStringBuilder.Password = Environment.GetEnvironmentVariable("MYSQL_ROOT_PASSWORD") ?? "default_password";

                options.UseMySQL(connectionStringBuilder.ConnectionString);
            });


            services.AddSingleton(x => new IdentittVaultSecure(
                Environment.GetEnvironmentVariable("IDENTITT_KEY"), 
                Environment.GetEnvironmentVariable("IDENTITT_IV")));

            services.AddScoped<ICrudRepository<Account>, CrudRepository<Account>>();
            //services.AddScoped<ICrudService<Application>, CrudService<Application, ICrudRepository<Application>>>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<AccountService>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

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
