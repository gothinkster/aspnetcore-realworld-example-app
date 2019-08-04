using Conduit.Features.Users;
using System.Threading.Tasks;

namespace Conduit.IntegrationTests.Features.Users
{
    public static class UserHelpers
    {
        public const string DefaultUserName = "username";

        /// <summary>
        /// creates a default user to be used in different tests
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static async Task<User> CreateDefaultUser(SliceFixture fixture)
        {
            var command = new Create.Command
            {
                User = new Create.UserData
                {
                    Email = "email",
                    Password = "password",
                    Username = DefaultUserName
                }
            };

            var commandResult = await fixture.SendAsync(command);
            return commandResult.User;
        }
    }
}
