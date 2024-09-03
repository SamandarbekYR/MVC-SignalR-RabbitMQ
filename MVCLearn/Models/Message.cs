﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MVCLearn.Models
{
    [Table("message")]
    public class Message : BaseEntity
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string MessageContent { get; set; } = string.Empty;
        public DateTime SendTime { get; set; }
        public DateTime? ReadTime { get; set; }
        public bool? IsRead { get; set; }
    }
}
