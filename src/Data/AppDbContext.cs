using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using bom.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace bom.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            

            // ASP.NET Identity            
            builder.Entity<User>().ToTable("Users", "Identity");
            builder.Entity<IdentityRole>().ToTable("Roles", "Identity");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Identity");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Identity");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Identity");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Identity");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Identity");
            

        }
        
        public void Seed(IApplicationBuilder app, IConfigurationRoot config)
        {
            //Identity
            SeedIdentity(app,config);

            //Application data
            SeedAppData();
        }

        private async void SeedIdentity(IApplicationBuilder app, IConfigurationRoot config)
        {
            if (!Roles.Any())
            {
                // 'admin' role
                using (var _roleManager = app.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>())
                {
                    var adminRole = new IdentityRole("Admin");
                    await _roleManager.CreateAsync(adminRole);
                    //await _roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim("Permission","*"));
                };
            }
            if (!Users.Any())
            {
                // Admin user
                using (var _userManager = app.ApplicationServices.GetRequiredService<UserManager<User>>())
                {
                    var adminUser = new User();
                    string _email = null, _password = null;
                    var _userSecrets = config["Authentication:Admin:UserName"];
                    if (_userSecrets == null)
                    {
                        #warning Change this credential!
                        _email = $"_ChangeThisAdmin@{Guid.NewGuid()}.com";
                        _password = _email;
                    }
                    else
                    {                        
                        _email = config["Authentication:Admin:Email"];
                        _password = config["Authentication:Admin:Password"];
                    }                    
                    adminUser.Email = _email;
                    await _userManager.CreateAsync(adminUser, _password);
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                };
            }
        }

        private async void SeedAppData()
        {
            await Task.FromResult(0);
        }
        
    }
}
