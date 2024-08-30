using System.ComponentModel.DataAnnotations.Schema;

namespace MVCLearn.Models
{
    [Table("users_messages")]
    public class UserMessage : BaseEntity
    {
        [Column("user_id")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Column("message_id")]
        public Guid MessageId { get; set; }
        public Message Message { get; set; }
    }
}
