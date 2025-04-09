namespace OOAP_lab7.Server.Models
{
    public enum CommentStatus { Pending, Approved, Rejected, Spam }

    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public CommentStatus Status { get; set; } = CommentStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
