﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using bom.Data;
using bom.Models;
using bom.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace bom
{
    public class Startup
    {
        private IConfigurationRoot _config;
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables(); //override any config files / user secrets          

            if (_env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets(); // %APPDATA%\microsoft\UserSecrets\<applicationId>\secrets.json
            }
            _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            // Add framework services.
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));

            services.AddIdentity<Models.Identity.User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(
                    opt => {
                        opt.UseApiRoutePrefix(new RouteAttribute("api/[controller]" /*"api/v{version}"*/));
                        //opt.FormatterMappings.SetMediaTypeMappingForFormat("xml", new MediaTypeHeaderValue("application/xml"));
                    }
                )
                //.AddXmlDataContractSerializerFormatters()
                ;            

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_config.GetSection("Logging"));
            loggerFactory.AddDebug();

            /*
            app.Run(async ctx =>
            {
                await ctx.Response.WriteAsync($"{_env.EnvironmentName}{Environment.NewLine}{_config.GetConnectionString("DefaultConnection")}");
            });
            */
            
            if (_env.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();                                

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859                
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var _db = serviceScope.ServiceProvider.GetService<AppDbContext>();
                        _db.Database.Migrate();
                        _db.Seed(app, _config);
                    }
                }
                catch { }
            }
            else
            {                
                app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
            }            

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseIdentity();

            app.UseStaticFiles();

            app.UseMvc(_routes =>
            {
                
                var _prefix = ""; // "{lg:regex(it|en|de)?}/";                

                //_routes.DefaultHandler = new bom.Services.RouteDebugger();

                _routes.MapRoute(name: "default",
                                template: _prefix + "{controller=Root}/{action=Index}/{id?}");

                _routes.MapRoute(name: "cms",
                                template: "cms/{action=Table}/{entity?}/{operation:regex(view|create|edit)?}/{id?}",
                                defaults: new { controller = "Cms" });

                /*
                _routes.MapRoute(name: "catchall",
                                template: _prefix + "{*catch-all}",
                                defaults: new { controller = "Root", action = "Index" });
                */

                // Microsoft.AspNetCore.SpaServices: http://blog.nbellocam.me/2016/03/21/routing-angular-2-asp-net-core/
                //_routes.MapSpaFallbackRoute("spa-fallback",defaults: new { controller = "Root", action = "Index" });
                
            });

        }
    }
}
