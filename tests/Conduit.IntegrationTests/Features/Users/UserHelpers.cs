using System;
using System.Threading.Tasks;
using Conduit.Features.Users;

namespace Conduit.IntegrationTests.Features.Users
{
    public static class UserHelpers
    {
        public static readonly string DefaultUserName = "username" + new Random().Next(0, 1000).ToString();

        /// <summary>
        /// creates a default user to be used in different tests
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static async Task<User> CreateDefaultUser(SliceFixture fixture)
        {
            var command = new Create.Command(new Create.UserData()
            {
                Email = "email@" + new Random().Next(0, 1000).ToString() + ".com",
                Password = "password",
                Username = DefaultUserName
            });

            var commandResult = await fixture.SendAsync(command);
            return commandResult.User;
        }
    }
}
