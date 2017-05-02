using System.Threading.Tasks;
using RealWorld.Features.Person;
using Xunit;

namespace Realworld.IntegrationTests.Features.Person
{
    public class IndexTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Index_Persons()
        {
            var person = new RealWorld.Domain.Person()
            {
                Bio = "Bio",
                Email = "email",
                Username = "username"
            };

            var person2 = new RealWorld.Domain.Person()

            {
                Bio = "Bio",
                Email = "email",
                Username = "username"
            };

            await InsertAsync(person, person2);

            var query = new Index.Query();

            var results = await SendAsync(query);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.Equal(2, results.Count);
        }
    }
}
