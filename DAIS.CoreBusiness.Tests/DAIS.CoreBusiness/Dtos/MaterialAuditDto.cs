using System;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialAuditDto
    {
        public string MaterialCode {  get; set; }
        public string MaterialName { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public string UpdatedColumn { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
