using System.Collections.Generic;
using Conduit.Domain;

namespace Conduit.Features.Comments
{
    public record CommentsEnvelope(List<Comment> Comments);
}
