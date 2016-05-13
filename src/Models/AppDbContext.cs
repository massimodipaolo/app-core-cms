using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Entity.Infrastructure;

namespace bom.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        private IConfigurationRoot _config;
        private static bool _isInitialized;

        #region Properties        
        //public new DbSet<User> Users { get; set; }
        #endregion


        public AppDbContext(IConfigurationRoot config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(_config["Data:DefaultConnection:ConnectionString"]);               
            base.OnConfiguring(builder);
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
    }
}
