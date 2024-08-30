using System.ComponentModel.DataAnnotations.Schema;

namespace MVCLearn.Models
{
    public class BaseEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
    }
}
