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
        private readonly IConfiguration _configuration;

        public IsmDbContext(IConfiguration configuration) =>
            _configuration = configuration;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetValue<string>("ISMConnectionString");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

    }
}
