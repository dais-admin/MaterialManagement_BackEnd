using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DAIS.DataAccess.Entities
{
    [Index(nameof(TableName))]
    [Index(nameof(DateTime))]
    [Index(nameof(UserId))]
    public record AuditLogs
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

        [Required]
        [MaxLength(200)]
        public string TableName { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public string OldValues { get; set; }

        public string NewValues { get; set; }

        public string AffectedColumns { get; set; }

        [Required]
        public string PrimaryKey { get; set; }
    }
}
