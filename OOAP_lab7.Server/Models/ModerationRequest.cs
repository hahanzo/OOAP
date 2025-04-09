namespace OOAP_lab7.Server.Models
{
    public class ModerationRequest
    {
        public ModeratorInfo Moderator { get; set; }
        public Comment CommentToModerate { get; set; }
        public ActionType Action { get; set; }
    }
}
