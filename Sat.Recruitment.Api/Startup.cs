using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api
{
    public class Startup
    {
        AppSettings oAppSettings;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            oAppSettings = new AppSettings();

            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();
            Configuration.Bind(oAppSettings);

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Declaro la interfaz generica
            IStorage oStorage;

            //Valido la configuracion
            if (Configuration["StorageType"] == "SQLServer")
            {
                //Si el tipo de base es SQLServer, instancio el objeto de clase SQLServer
                oStorage = new SQLServer(Configuration["ConnectionString"]);
            }
            else
            {
                //Si el tipo de base es SQL Lite, instancio el objeto de clase SQLite
                oStorage = new SQLite(Configuration["ConnectionString"]);
            }

            //Inyeccion de dependecias, agrego el objeto como singleton para que lo podamos usar globalmente
            services.AddSingleton<IStorage>(oStorage);
            //Inyeccion de dependecias, singleton para objeto de configuracion
            services.AddSingleton<AppSettings>(oAppSettings);

            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
