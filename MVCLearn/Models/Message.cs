using System.ComponentModel.DataAnnotations.Schema;

namespace MVCLearn.Models
{
    [Table("message")]
    public class Message : BaseEntity
    {
        [Column("content")]
        public string Content { get; set; } = string.Empty;
        [Column("send_at")]
        public DateTime SendAt { get; set; } = DateTime.UtcNow.AddHours(5);
        public List<UserMessage> UserMessages { get; set; }
    }
}
