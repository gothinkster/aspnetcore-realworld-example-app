using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string Hash { get; set; }

        public string Salt { get; set; }
    }
}