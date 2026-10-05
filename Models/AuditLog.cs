using System;
using System.ComponentModel.DataAnnotations;

namespace FreshSpinApi.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        public string Action { get; set; } = string.Empty;

        public string PerformedByRole { get; set; } = string.Empty;

        public string PerformedById { get; set; } = string.Empty;

        public int RelatedEntityId { get; set; }

        public string Details { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}