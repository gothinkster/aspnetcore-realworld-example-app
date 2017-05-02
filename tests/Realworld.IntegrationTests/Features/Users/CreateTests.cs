using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;
using Xunit;

namespace Realworld.IntegrationTests.Features.Users
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_User()
        {
            var command = new RealWorld.Features.Users.Create.Command()
            {
                Email = "email",
                Password = "password"
            };

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Persons.Where(d => d.Email == command.Email).SingleOrDefaultAsync());

            Assert.NotNull(created);
            Assert.Equal(created.Hash, new PasswordHasher().Hash("password", created.Salt));
        }
    }
}