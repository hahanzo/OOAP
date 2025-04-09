using OOAP_lab7.Server.Data;
using OOAP_lab7.Server.Models;

namespace OOAP_lab7.Server.Commands
{
    public class RejectCommentCommand : CommentStatusCommand
    {
        public RejectCommentCommand(Comment comment, ICommentRepository repository)
            : base(comment, repository) { }

        protected override CommentStatus GetTargetStatus() => CommentStatus.Rejected;
    }
}
