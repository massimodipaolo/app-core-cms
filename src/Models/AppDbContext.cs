using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Builder;

namespace bom.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        private IConfigurationRoot _config;

        #region Properties        
        //public new DbSet<User> Users { get; set; }
        #endregion


        public AppDbContext(IConfigurationRoot config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(_config["Data:DefaultConnection:ConnectionString"]);
                base.OnConfiguring(builder);
            }
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // ASP.NET Identity
            mb.Entity<User>().ToTable("Users", "Identity");
            mb.Entity<IdentityRole>().ToTable("Roles", "Identity");
            mb.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Identity");
            mb.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Identity");
            mb.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Identity");
            mb.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Identity");
        }

        public void Seed(IApplicationBuilder app)
        {
            //Identity
            SeedIdentity(app);

            //Application data
            SeedAppData();
        }

        private async void SeedIdentity(IApplicationBuilder app)
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
                    #warning Change this data ASAP!
                    adminUser.UserName = "admin";
                    adminUser.Email = "admin@mydomain.com";                    
                    await _userManager.CreateAsync(adminUser, "MYP@55word");                    
                    await _userManager.AddToRoleAsync(adminUser, "admin");                    
                };
            }
        }

        private void SeedAppData()
        {

        }

    }
}
