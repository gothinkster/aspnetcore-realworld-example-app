namespace Conduit.Features.Users
{
    public class User
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        public string Token { get; set; }
    }


    public class UserEnvelope
    {
        public UserEnvelope(User user)
        {
            User = user;
        }

        public User User { get; set; }
    }
}