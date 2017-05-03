namespace RealWorld.Features.Profiles
{
    public class Profile
    {
        public string Username { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        public bool Following { get; set; }
    }

    public class ProfileEnvelope
    {
        public ProfileEnvelope(Profile profile)
        {
            Profile = profile;
        }

        public Profile Profile { get; set; }
    }
}