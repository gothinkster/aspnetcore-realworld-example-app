using System.Linq;
using System.Threading.Tasks;
using Conduit.Features.Articles;
using Conduit.IntegrationTests.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace Conduit.IntegrationTests.Features.Articles
{
    public static class ArticleHelpers
    {
        /// <summary>
        /// creates an article based on the given Create command. It also creates a default user
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static async Task<Domain.Article> CreateArticle(SliceFixture fixture, Create.Command command)
        {
            // first create the default user
            var user = await UserHelpers.CreateDefaultUser(fixture);

            var dbContext = fixture.GetDbContext();
            var currentAccessor = new StubCurrentUserAccessor(user.Username);

            var articleCreateHandler = new Create.Handler(dbContext, currentAccessor);
            var created = await articleCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbArticle = await fixture.ExecuteDbContextAsync(db => db.Articles.Where(a => a.ArticleId == created.Article.ArticleId)
                .SingleOrDefaultAsync());

            return dbArticle;
        }
    }
}
