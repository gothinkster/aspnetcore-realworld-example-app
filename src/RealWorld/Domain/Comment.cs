
namespace RealWorld.Domain
{
   public class Comment
    {
        public int CommentId { get; set; }

        public string Body { get; set; }

        public Person Author { get; set; }

        public Article Article { get; set; }
    }
}