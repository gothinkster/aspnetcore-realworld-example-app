namespace Conduit.Domain
{
    public class ArticleTag
    {
        public int ArticleId { get; set; }
        public Article? Article { get; set; }

        public string? TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
