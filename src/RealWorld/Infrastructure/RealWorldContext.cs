using Microsoft.EntityFrameworkCore;
using RealWorld.Domain;

namespace RealWorld.Infrastructure
{
    public class RealWorldContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=realworld.db");
        }
    }
}
