using OOAP_lab7.Server.Models;

namespace OOAP_lab7.Server.Data
{
    public class InMemoryCommentRepository : ICommentRepository
    {
        private readonly List<Comment> _comments = new List<Comment>
    {
        new Comment { Id = 1, Author = "User1", Text = "This is a pending comment.", Status = CommentStatus.Pending, CreatedAt = DateTime.UtcNow.AddDays(-1) },
        new Comment { Id = 2, Author = "User2", Text = "Another pending one.", Status = CommentStatus.Pending, CreatedAt = DateTime.UtcNow.AddHours(-5) },
        new Comment { Id = 3, Author = "User3", Text = "Already approved.", Status = CommentStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-2) },
    };
        private int _nextId = 4;
        private readonly object _lock = new object(); // Simple lock for thread safety

        public Task<Comment?> GetByIdAsync(int id)
        {
            lock (_lock)
            {
                return Task.FromResult(_comments.FirstOrDefault(c => c.Id == id));
            }
        }

        public Task<IEnumerable<Comment>> GetPendingAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_comments.Where(c => c.Status == CommentStatus.Pending).ToList().AsEnumerable());
            }
        }

        public Task AddAsync(Comment comment)
        {
            lock (_lock)
            {
                comment.Id = _nextId++;
                _comments.Add(comment);
            }
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Comment comment)
        {
            lock (_lock)
            {
                var index = _comments.FindIndex(c => c.Id == comment.Id);
                if (index != -1)
                {
                    _comments[index] = comment;
                }
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            lock (_lock)
            {
                _comments.RemoveAll(c => c.Id == id);
            }
            return Task.CompletedTask;
        }
    }
}
