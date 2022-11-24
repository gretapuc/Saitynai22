using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saitynai.Auth.Model;
using Saitynai.Data.Entities;

namespace Saitynai.Data
{
    public class IsmDbContext : IdentityDbContext<IsmRestUser>
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=localhost;user=root;password=password;database=ISM";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

    }
}
