using Conduit.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading.Tasks;

namespace Conduit.Infrastructure
{
    public class ConduitContext : DbContext
    {
        private readonly string _databaseName = Startup.DATABASE_FILE;
        private IDbContextTransaction _currentTransaction;

        public ConduitContext(DbContextOptions options)
            : base(options)
        {
        }

        public ConduitContext(string databaseName)
        {
            _databaseName = databaseName;
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<ArticleFavorite> ArticleFavorites { get; set; }
        public DbSet<FollowedPeople> FollowedPeople { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databaseName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleTag>(b =>
            {
                b.HasKey(t => new { t.ArticleId, t.TagId });

                b.HasOne(pt => pt.Article)
                .WithMany(p => p.ArticleTags)
                .HasForeignKey(pt => pt.ArticleId);

                b.HasOne(pt => pt.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(pt => pt.TagId);
            });

            modelBuilder.Entity<ArticleFavorite>(b =>
            {
                b.HasKey(t => new { t.ArticleId, t.PersonId });

                b.HasOne(pt => pt.Article)
                    .WithMany(p => p.ArticleFavorites)
                    .HasForeignKey(pt => pt.ArticleId);

                b.HasOne(pt => pt.Person)
                    .WithMany(t => t.ArticleFavorites)
                    .HasForeignKey(pt => pt.PersonId);
            });

            modelBuilder.Entity<FollowedPeople>(b =>
            {
                b.HasKey(t => new { t.ObserverId, t.TargetId });

                b.HasOne(pt => pt.Observer)
                    .WithMany(p => p.Followers)
                    .HasForeignKey(pt => pt.ObserverId);

                b.HasOne(pt => pt.Target)
                    .WithMany(t => t.Following)
                    .HasForeignKey(pt => pt.TargetId);
            });
        }

        #region Transaction Handling
        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void CommitTransaction()
        {
            try
            {
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        #endregion
    }
}
