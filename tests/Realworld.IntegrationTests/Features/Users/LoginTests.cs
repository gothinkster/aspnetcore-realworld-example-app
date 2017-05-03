using System;
using System.Threading.Tasks;
using RealWorld.Domain;
using RealWorld.Features.Users;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Security;
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
                Username = "username",
                Email = "email",
                Hash = new PasswordHasher().Hash("password", salt),
                Salt = salt
            };
            await InsertAsync(person);

            var command = new RealWorld.Features.Users.Login.Command()
            {
                User = new Login.UserData()
                {
                    Email = "email",
                    Password = "password"
                }
            };

            var user = await SendAsync(command);

            Assert.NotNull(user?.User);
            Assert.Equal(user.User.Email, command.User.Email);
            Assert.Equal(user.User.Username, "username");
            Assert.NotNull(user.User.Token);
        }
    }
}