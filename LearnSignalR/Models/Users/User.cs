namespace LearnSignalR.Models.Users
{
    public class User : BaseEntity
    {
        public string RoleName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Gmail {  get; set; } = string.Empty;
        public bool IsOnline { get; set; }
    }
}
