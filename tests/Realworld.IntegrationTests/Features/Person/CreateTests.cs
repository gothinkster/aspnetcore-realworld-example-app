using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Realworld.IntegrationTests.Features.Person
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_Person()
        {
            var command = new RealWorld.Features.Person.Create.Command()
            {
                Bio = "Bio",
                Email = "email",
                Username = "username"
            };

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Persons.Where(d => d.Username == command.Username).SingleOrDefaultAsync());

            Assert.NotNull(created);
        }
    }
}