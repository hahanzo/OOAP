namespace OOAP_lab7.Server.Models
{
    public enum ActionType { Approve, Reject, MarkAsSpam}

    public class ModerationActionDto
    {
        public int CommentId { get; set; }
        public ActionType Action { get; set; }
    }

}
