using System.Collections.Generic;

namespace RealWorld.Domain
{
    public class Article : IEntity
    {
        public int ArticleId { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public Person Author { get; set; }

        public List<Comment> Comments {get;set;}
    }
}