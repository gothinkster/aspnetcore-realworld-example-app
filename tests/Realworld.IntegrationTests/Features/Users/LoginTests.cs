using System;
using System.Threading.Tasks;
using RealWorld.Domain;
using RealWorld.Infrastructure;
using Xunit;

namespace Realworld.IntegrationTests.Features.Users
{
    public class LoginTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Login()
        {
            var salt = Guid.NewGuid().ToByteArray();
            var person = new Person
            {
                Email = "email",
                Hash = new PasswordHasher().Hash("password", salt),
                Salt = salt
            };
            await InsertAsync(person);

            var command = new RealWorld.Features.Users.Login.Command()
            {
                Email = "email",
                Password = "password"
            };

            var user = await SendAsync(command);

            Assert.NotNull(user);
            Assert.Equal(user.Email, command.Email);
        }
    }
}