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
                    string _userName=null,_email=null,_password=null;
                    var _userSecrets = _config["Authentication:Admin:UserName"];
                    if (_userSecrets == null)
                    {
                        #warning Use secrets or change/delete this data
                        _userName = $"{Guid.NewGuid()}@{Guid.NewGuid()}.com";
                        _email = _userName;
                        _password = "$P455w0rd";                                                
                    } else
                    {
                        _userName = _config["Authentication:Admin:UserName"];
                        _email = _config["Authentication:Admin:Email"];
                        _password = _config["Authentication:Admin:Password"];
                    }
                    adminUser.UserName = _userName;
                    adminUser.Email = _email;
                    await _userManager.CreateAsync(adminUser, _password);
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                };
            }
        }

        private async Task SeedAppData()
        {
            await Task.FromResult(0);
        }

    }
}
