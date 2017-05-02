using Microsoft.EntityFrameworkCore;
using RealWorld.Domain;

namespace RealWorld.Infrastructure
{
    public class RealWorldContext : DbContext
    {
        private readonly string _databaseName = Startup.DATABASE_FILE;

        public RealWorldContext()
        {
        }

        public RealWorldContext(string databaseName)
        {
            _databaseName = databaseName;
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databaseName}");
        }
    }
}
