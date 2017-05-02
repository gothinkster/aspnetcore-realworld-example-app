using System.Collections.Generic;

namespace RealWorld.Domain
{
    public class Person : IEntity
    {
        public int PersonId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        public List<Article> Favorites { get; set; }

        public List<Person> Following { get; set; }

        public byte[] Hash { get; set; }

        public byte[] Salt { get; set; }
    }
}