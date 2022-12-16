using System.Linq;
using System.Threading.Tasks;
using Conduit.Features.Comments;
using Conduit.IntegrationTests.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace Conduit.IntegrationTests.Features.Comments
{
    public static class CommentHelpers
    {
        /// <summary>
        /// creates an article comment based on the given Create command. 
        /// Creates a default user if parameter userName is empty.
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="command"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static async Task<Domain.Comment> CreateComment(SliceFixture fixture, Create.Command command, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                var user = await UserHelpers.CreateDefaultUser(fixture);
                userName = user.Username;
            }

            var dbContext = fixture.GetDbContext();
            var currentAccessor = new StubCurrentUserAccessor(userName);

            var commentCreateHandler = new Create.Handler(dbContext, currentAccessor);
            var created = await commentCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbArticleWithComments = await fixture.ExecuteDbContextAsync(
                db => db.Articles
                    .Include(a => a.Comments).Include(a => a.Author)
                    .Where(a => a.Slug == command.Slug)
                    .SingleOrDefaultAsync()
            );

            var dbComment = dbArticleWithComments.Comments
                .Where(c => c.ArticleId == dbArticleWithComments.ArticleId && c.Author == dbArticleWithComments.Author)
                .FirstOrDefault();

            return dbComment;
        }
    }
}
