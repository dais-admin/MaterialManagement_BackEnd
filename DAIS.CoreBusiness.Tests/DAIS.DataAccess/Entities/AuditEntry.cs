using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAIS.DataAccess.Entities
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entity)
        {
            Entity = entity;
        }

        public EntityEntry Entity { get; }
        public string UserId { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();

        public bool HasChanges()
        {
            return AuditType != AuditType.None && 
                   (AuditType == AuditType.Create || 
                    AuditType == AuditType.Delete || 
                    (AuditType == AuditType.Update && ChangedColumns.Any()));
        }

        public AuditLogs ToAudit()
        {
            var audit = new AuditLogs
            {
                UserId = string.IsNullOrEmpty(UserId) ? "System" : UserId.Length > 450 ? UserId.Substring(0, 450) : UserId,
                Type = AuditType.ToString(),
                TableName = TableName.Length > 200 ? TableName.Substring(0, 200) : TableName,
                DateTime = DateTime.UtcNow,
                PrimaryKey = JsonConvert.SerializeObject(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
                AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns)
            };

            return audit;
        }
    }

    public enum AuditType
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }
}
