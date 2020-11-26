using Conduit.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Conduit.Features.Comments
{
    public static class CommentExtensions
    {
        public static IQueryable<Comment> GetAllData(this DbSet<Comment> comments)
        {
            return comments
                .Include(x => x.Author)
                .Include(x => x.Article)
                .AsNoTracking();
        }
    }
}
