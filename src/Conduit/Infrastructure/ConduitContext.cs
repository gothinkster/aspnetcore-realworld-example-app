using System.Data;
using Conduit.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Conduit.Infrastructure
{
    public class ConduitContext : DbContext
    {
        private IDbContextTransaction? _currentTransaction;

        public ConduitContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<ArticleTag> ArticleTags { get; set; } = null!;
        public DbSet<ArticleFavorite> ArticleFavorites { get; set; } = null!;
        public DbSet<FollowedPeople> FollowedPeople { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleTag>(b =>
            {
                b.HasKey(t => new { t.ArticleId, t.TagId });

                b.HasOne(pt => pt.Article)
                .WithMany(p => p!.ArticleTags)
                .HasForeignKey(pt => pt.ArticleId);

                b.HasOne(pt => pt.Tag)
                .WithMany(t => t!.ArticleTags)
                .HasForeignKey(pt => pt.TagId);
            });

            modelBuilder.Entity<ArticleFavorite>(b =>
            {
                b.HasKey(t => new { t.ArticleId, t.PersonId });

                b.HasOne(pt => pt.Article)
                    .WithMany(p => p!.ArticleFavorites)
                    .HasForeignKey(pt => pt.ArticleId);

                b.HasOne(pt => pt.Person)
                    .WithMany(t => t!.ArticleFavorites)
                    .HasForeignKey(pt => pt.PersonId);
            });

            modelBuilder.Entity<FollowedPeople>(b =>
            {
                b.HasKey(t => new { t.ObserverId, t.TargetId });

                // we need to add OnDelete RESTRICT otherwise for the SqlServer database provider, 
                // app.ApplicationServices.GetRequiredService<ConduitContext>().Database.EnsureCreated(); throws the following error:
                // System.Data.SqlClient.SqlException
                // HResult = 0x80131904
                // Message = Introducing FOREIGN KEY constraint 'FK_FollowedPeople_Persons_TargetId' on table 'FollowedPeople' may cause cycles or multiple cascade paths.Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
                // Could not create constraint or index. See previous errors.
                b.HasOne(pt => pt.Observer)
                    .WithMany(p => p!.Followers)
                    .HasForeignKey(pt => pt.ObserverId)
                    .OnDelete(DeleteBehavior.Restrict);

                // we need to add OnDelete RESTRICT otherwise for the SqlServer database provider, 
                // app.ApplicationServices.GetRequiredService<ConduitContext>().Database.EnsureCreated(); throws the following error:
                // System.Data.SqlClient.SqlException
                // HResult = 0x80131904
                // Message = Introducing FOREIGN KEY constraint 'FK_FollowingPeople_Persons_TargetId' on table 'FollowedPeople' may cause cycles or multiple cascade paths.Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
                // Could not create constraint or index. See previous errors.
                b.HasOne(pt => pt.Target)
                    .WithMany(t => t!.Following)
                    .HasForeignKey(pt => pt.TargetId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        #region Transaction Handling
        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            if (!Database.IsInMemory())
            {
                _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
            }
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
