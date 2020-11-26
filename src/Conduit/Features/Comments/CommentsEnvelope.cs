using System.Collections.Generic;
using Conduit.Domain;

namespace Conduit.Features.Comments
{
    public class CommentsEnvelope
    {
        public CommentsEnvelope(List<Comment> comments)
        {
            Comments = comments;
        }
        public CommentsEnvelope()
        {
        }
        public List<Comment> Comments { get; set; }
        public int CommentsCount { get; set; }
    }
}