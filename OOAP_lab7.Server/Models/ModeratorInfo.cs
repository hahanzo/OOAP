namespace OOAP_lab7.Server.Models
{
    public enum ModeratorRole { Junior, Senior, Admin }

    public class ModeratorInfo
    {
        public string UserId { get; set; } = string.Empty;
        public ModeratorRole Role { get; set; }
    }
}
