//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;
//using Create = Conduit.Features.Articles.Create;

//namespace Conduit.IntegrationTests.Features.Articles
//{
//    public class CreateTests : SliceFixture
//    {
//        [Fact]
//        public async Task Expect_Create_Article()
//        {
//            var command = new Create.Command(new Create.ArticleData()
//            {
//                Title = "Test article dsergiu77",
//                Description = "Description of the test article",
//                Body = "Body of the test article",
//                TagList = new string[] { "tag1", "tag2" }
//            });

//            //var user = await UserHelpers.CreateDefaultUser(this);
//            //var accessorMock = new Mock<ICurrentUserAccessor>();
//            //var dbContext = GetDbContext();
//            //var currentAccessor = new StubCurrentUserAccessor(user.Username);
//            var article = await ArticleHelpers.CreateArticle(this, command);
//            //var articleCreateHandler = new Create.Handler(dbContext, currentAccessor);
//            //var created = await articleCreateHandler.Handle(command, new System.Threading.CancellationToken());
//            //await SendAsync(command);
//            //var article = await ExecuteDbContextAsync(db => db.Articles.Where(d => d.Title == command.Article.Title).SingleOrDefaultAsync());
//            Assert.NotNull(article);
//            Assert.Equal(article.Title, command.Article.Title);
//            Assert.Equal(article.TagList.Count(), command.Article.TagList.Count());
//        }
//    }
//}
