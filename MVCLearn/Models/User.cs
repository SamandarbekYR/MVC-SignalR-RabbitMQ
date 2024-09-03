using System.ComponentModel.DataAnnotations.Schema;

namespace MVCLearn.Models
{
    [Table("user")]
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Gmail { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
    }
}
