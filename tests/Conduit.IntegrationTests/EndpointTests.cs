
using System.Net;
using System.Threading.Tasks;
using Conduit.Features.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net.Http.Json;
using Conduit.IntegrationTests.Features.Users;
using Conduit.Features.Articles;
using Conduit.IntegrationTests.Features.Articles;
using System.Net.Http.Headers;

namespace Conduit.IntegrationTests
{
    [TestCaseOrderer(
    "RunTestsInOrder.XUnit.PriorityOrderer",
    "RunTestsInOrder.XUnit")]
    public class EndpointTests : IntegrationTestBase
    {
        public EndpointTests(WebApplicationFactory<Program> factory)
            : base(factory) { }

        [Fact, TestPriority(1)]
        public async Task CreateUser()
        {
            // Arrange
            var payload = new UserTestData
            {
                User = new UserPayload
                {
                    Username = "John Appleseed",
                    Email = "John.Appleseed@Hotmail.com",
                    Password = "CthulthuOv3rL0Rd!2026"
                }
            };

            // Act
            var result = await client.PostAsJsonAsync("user", payload);
            var content = await result.Content.ReadFromJsonAsync<UserEnvelope>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.False(content is null);
        }


        [Fact, TestPriority(2)]
        public async Task LoginAndPostArticle()
        {
            // Arrange
            var payload = new UserTestData
            {
                User = new UserPayload
                {
                    Email = "John.Appleseed@Hotmail.com",
                    Password = "CthulthuOv3rL0Rd!2026"
                }
            };

            // Act
            var result = await client.PostAsJsonAsync("user/login", payload);
            var content = await result.Content.ReadFromJsonAsync<UserEnvelope>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.False(content is null);

            var payload2 = new ArticleTestData
            {
                Article = new ArticlePayload
                {
                    Title = "Test Create Article",
                    Description = "Description of the test article",
                    Body = "Body of the test article",
                    TagList = new string[] { "tag1", "tag2" }
                }
            };

            //add bearer token
            client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", content.User.Token);
            // Act
            var resultOnPostingArticle = await client.PostAsJsonAsync("articles", payload2);
            var response = await resultOnPostingArticle.Content.ReadFromJsonAsync<ArticleEnvelope>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, resultOnPostingArticle.StatusCode);
            Assert.False(response is null);
        }


        [Fact, TestPriority(3)]
        public async Task LoginPostAndDeleteArticle()
        {
            // Arrange
            var payload = new UserTestData
            {
                User = new UserPayload
                {
                    Email = "John.Appleseed@Hotmail.com",
                    Password = "CthulthuOv3rL0Rd!2026"
                }
            };

            // Act
            var result = await client.PostAsJsonAsync("user/login", payload);
            var content = await result.Content.ReadFromJsonAsync<UserEnvelope>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.False(content is null);

            var payload2 = new ArticleTestData
            {
                Article = new ArticlePayload
                {
                    Title = "Test Delete Article",
                    Description = "Description of the test article",
                    Body = "Body of the test article",
                    TagList = new string[] { "tag3", "tag4" }
                }
            };

            //add bearer token
            client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", content.User.Token);
            // Act
            var resultOnPostingArticle = await client.PostAsJsonAsync("articles", payload2);
            var response = await resultOnPostingArticle.Content.ReadFromJsonAsync<ArticleEnvelope>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, resultOnPostingArticle.StatusCode);
            Assert.False(response is null);

            var resultOnDeletingArticle = await client.DeleteAsync($"articles/{response.Article.Slug}");
            var responseOnDelete = await resultOnDeletingArticle.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, resultOnDeletingArticle.StatusCode);
            //Assert.Equal("1", responseOnDelete);
        }

        [Fact, TestPriority(4)]
        public async Task LoginPostAndEditArticle()
        {
            // Arrange
            var payload = new UserTestData
            {
                User = new UserPayload
                {
                    Email = "John.Appleseed@Hotmail.com",
                    Password = "CthulthuOv3rL0Rd!2026"
                }
            };

            // Act
            var result = await client.PostAsJsonAsync("user/login", payload);
            var content = await result.Content.ReadFromJsonAsync<UserEnvelope>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.False(content is null);

            var payloadCreate = new ArticleTestData
            {
                Article = new ArticlePayload
                {
                    Title = "Test Update Article Ward",
                    Description = "Description of the Update article",
                    Body = "Body of the Update article",
                    TagList = new string[] { "tag10", "tag15" }
                }
            };

            //add bearer token
            client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", content.User.Token);
            // Act
            var resultOnPostingArticle = await client.PostAsJsonAsync("articles", payloadCreate);
            var response = await resultOnPostingArticle.Content.ReadFromJsonAsync<ArticleEnvelope>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, resultOnPostingArticle.StatusCode);
            Assert.False(response is null);

            var payloadUpdated = new ArticleTestData
            {
                Article = new ArticlePayload
                {
                    Title = "Updated this Test Update Article",
                    Description = "Description of the test Update article",
                    Body = "Body of the test Update article",
                    TagList = new string[] { "tag7", "tag6" }
                }
            };

            var resultOnUpdatingArticle = await client.PutAsJsonAsync($"articles/{response.Article.Slug}", payloadUpdated);
            var responseOnUpdate = await resultOnUpdatingArticle.Content.ReadFromJsonAsync<ArticleEnvelope>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, resultOnUpdatingArticle.StatusCode);
            Assert.True(responseOnUpdate is not null);
        }
    }
}
