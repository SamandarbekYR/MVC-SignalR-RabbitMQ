using System.ComponentModel.DataAnnotations.Schema;

namespace MVCLearn.Models
{
    [Table("user")]
    public class User : BaseEntity
    {
        [Column("role_name")]
        public string RoleName { get; set; } = string.Empty;
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Column("gmail")]
        public string Gmail { get; set; } = string.Empty;
        public List<UserMessage> UserMessages { get; set; }
    }
}
