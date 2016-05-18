using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using bom.Models;
using bom.Services;
using Microsoft.AspNet.Http;

namespace bom
{
    public class Startup
    {
        private IConfigurationRoot _config;
        private IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            _env = env;
                        
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()                                
                .AddJsonFile("appsettings.json")
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
            services.AddInstance(_config);

            // Add framework services.                                    
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<AppDbContext>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();               
                        
            // Add application services.            
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {            
            loggerFactory.AddConsole(_config.GetSection("Logging"));
            loggerFactory.AddDebug();            

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
                        _db.Seed(app);
                    }
                }
                catch {}                
            }
            else
            {                
                app.UseExceptionHandler("/Home/Error");                
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles(); 
            
            app.UseIdentity();            
                        
            
            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
