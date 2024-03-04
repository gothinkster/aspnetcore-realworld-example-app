namespace Conduit.Domain;

public class ArticleTag
{
    public int ArticleId { get; init; }
    public Article? Article { get; init; }

    public string? TagId { get; init; }
    public Tag? Tag { get; init; }
}
