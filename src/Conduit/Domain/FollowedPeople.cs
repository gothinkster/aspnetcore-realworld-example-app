namespace Conduit.Domain;

public class FollowedPeople
{
    public int ObserverId { get; init; }
    public Person? Observer { get; init; }

    public int TargetId { get; init; }
    public Person? Target { get; init; }
}
