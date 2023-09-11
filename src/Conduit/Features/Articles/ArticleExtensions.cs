using System.Linq;
using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Articles;

public static class ArticleExtensions
{
    public static IQueryable<Article> GetAllData(this DbSet<Article> articles) =>
        articles
            .Include(x => x.Author)
            .Include(x => x.ArticleFavorites)
            .Include(x => x.ArticleTags)
            .AsNoTracking();
}
