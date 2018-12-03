using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Conduit.Features.Users;

namespace Conduit.IntegrationTests.Features.Users
{
    public static class UserHelpers
    {
        /// <summary>
        /// creates a default user to be used in different tests
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static async Task<User> CreateDefaultUser(SliceFixture fixture)
        {
            var command = new Create.Command()
            {
                User = new Create.UserData()
                {
                    Email = "email",
                    Password = "password",
                    Username = "username"
                }
            };

            var commandResult = await fixture.SendAsync(command);
            return commandResult.User;
        }
    }
}
