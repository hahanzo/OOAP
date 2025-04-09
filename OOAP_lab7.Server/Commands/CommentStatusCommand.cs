using OOAP_lab7.Server.Data;
using OOAP_lab7.Server.Models;

namespace OOAP_lab7.Server.Commands
{
    public abstract class CommentStatusCommand : ICommand
    {
        protected readonly Comment _comment;
        protected readonly ICommentRepository _repository;
        protected CommentStatus _previousStatus;

        protected CommentStatusCommand(Comment comment, ICommentRepository repository)
        {
            _comment = comment;
            _repository = repository;
        }

        protected abstract CommentStatus GetTargetStatus();

        public virtual async Task ExecuteAsync()
        {
            _previousStatus = _comment.Status;
            _comment.Status = GetTargetStatus();
            await _repository.UpdateAsync(_comment);
        }

        public virtual async Task UndoAsync()
        {
            _comment.Status = _previousStatus;
            await _repository.UpdateAsync(_comment);
        }
    }
}
