namespace Conduit.Domain;

public class ArticleFavorite
{
    public int ArticleId { get; init; }
    public Article? Article { get; init; }

    public int PersonId { get; init; }
    public Person? Person { get; init; }
}
