using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Data;
using DAIS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialAuditService : IMaterialAuditService
    {
        private readonly AppDbContext _context;

        public MaterialAuditService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<List<MaterialAuditDto>> CreateMaterialAuditDto(AuditLogs audit, Material material)
        {
            List<MaterialAuditDto> materialAuditDtos = new List<MaterialAuditDto>();
            var affectedColumns = audit.AffectedColumns != null 
                ? JsonConvert.DeserializeObject<List<string>>(audit.AffectedColumns)
                : new List<string>();

            var oldValues = audit.OldValues != null 
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(audit.OldValues)
                : new Dictionary<string, object>();

            var newValues = audit.NewValues != null 
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(audit.NewValues)
                : new Dictionary<string, object>();

            // Get the first changed column and its values
            foreach(string changedColumn in affectedColumns)
            {
                
                var materialaudit= new MaterialAuditDto
                {
                    MaterialCode = material.MaterialCode,
                    MaterialName=material.MaterialName,
                    TableName = audit.TableName,
                    Action = audit.Type,
                    UpdatedColumn = changedColumn,
                    OldValue = Convert.ToString(oldValues[changedColumn]),
                    NewValue = Convert.ToString(newValues[changedColumn]),
                    UpdatedDate = audit.DateTime,
                    UpdatedBy = audit.UserId
                };
                materialAuditDtos.Add(materialaudit);
            }
            /*var firstChange = affectedColumns.FirstOrDefault();
            string oldValue = firstChange != null && oldValues.ContainsKey(firstChange) 
                ? oldValues[firstChange]?.ToString() 
                : null;
            string newValue = firstChange != null && newValues.ContainsKey(firstChange) 
                ? newValues[firstChange]?.ToString() 
                : null;

            return new MaterialAuditDto
            {
                MaterialCode=material.MaterialCode,
                TableName = audit.TableName,
                Action = audit.Type,
                UpdatedColumn = firstChange,
                OldValue = oldValue,
                NewValue = newValue,
                UpdatedDate = audit.DateTime,
                UpdatedBy = audit.UserId
            };*/
            return materialAuditDtos;
        }

        public async Task<IEnumerable<MaterialAuditDto>> GetMaterialAuditsByUserAsync(string userId)
        {
            var audits = await _context.AuditLogs
                .Where(a => a.UserId == userId && a.TableName == "Material")
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();

            var result = new List<MaterialAuditDto>();

            foreach (var audit in audits)
            {
                var primaryKey = JsonConvert.DeserializeObject<Dictionary<string, object>>(audit.PrimaryKey);
                var materialId = Guid.Parse(primaryKey["Id"].ToString());
                
                var material = await _context.Materials.FindAsync(materialId);
                if (material != null)
                {
                    result.AddRange(await CreateMaterialAuditDto(audit, material));
                }
            }

            return result;
        }

        public async Task<IEnumerable<MaterialAuditDto>> GetMaterialAuditsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var audits = await _context.AuditLogs
                .Where(a => a.TableName == "Materials" && 
                           a.DateTime >= startDate && 
                           a.DateTime <= endDate)
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();

            var result = new List<MaterialAuditDto>();

            foreach (var audit in audits)
            {
                var primaryKey = JsonConvert.DeserializeObject<Dictionary<string, object>>(audit.PrimaryKey);
                var materialId = Guid.Parse(primaryKey["Id"].ToString());
                
                var material = await _context.Materials.FindAsync(materialId);
                if (material != null)
                {
                    result.AddRange(await CreateMaterialAuditDto(audit, material));
                }
            }

            return result;
        }
        public async Task<IEnumerable<MaterialAuditDto>> GetMaterialAuditsByFilterAsync(MaterialFilterDto auditFilterDto)
        {
            var result = new List<MaterialAuditDto>();
            try
            {
                var audits = await _context.AuditLogs
                .Where(a => a.TableName == auditFilterDto.TableName &&
                           a.DateTime >= auditFilterDto.FromDate &&
                           a.DateTime <= auditFilterDto.ToDate &&
                           a.Type!= "Create")
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();              

                foreach (var audit in audits)
                {
                    var primaryKey = JsonConvert.DeserializeObject<Dictionary<string, object>>(audit.PrimaryKey);
                    var materialId = Guid.Parse(primaryKey["Id"].ToString());

                    var material = await _context.Materials.FindAsync(materialId);
                    if (material != null)
                    {
                        result.AddRange(await CreateMaterialAuditDto(audit, material));
                    }
                }
            }
            catch(Exception ex) { }
            {
                
            }
            return result;
        }
        public async Task<IEnumerable<MaterialAuditDto>> GetMaterialAuditsByMaterialCodeAsync(string materialCode)
        {
            // First get the material to get its ID
            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.MaterialCode == materialCode);

            if (material == null)
                return Enumerable.Empty<MaterialAuditDto>();

            var audits = await _context.AuditLogs
                .Where(a => a.TableName == "Materials" && 
                           a.PrimaryKey.Contains(material.Id.ToString()))
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();

            var result = new List<MaterialAuditDto>();

            foreach (var audit in audits)
            {
                result.AddRange(await CreateMaterialAuditDto(audit, material));
            }

            return result;
        }
    }
}
