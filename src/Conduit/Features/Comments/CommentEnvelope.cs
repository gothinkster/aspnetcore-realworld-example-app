using Conduit.Domain;

namespace Conduit.Features.Comments
{
    public class CommentEnvelope
    {
        public CommentEnvelope(Comment comment)
        {
            Comment = comment;
        }

        public Comment Comment { get; }
    }
}