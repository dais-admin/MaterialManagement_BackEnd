
namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialIssueReceiveDto
    {
        public Guid? Id {  get; set; }
        public string VoucherNo { get; set; }
        public string Remarks {  get; set; }
        public DateTime? Date {  get; set; }
        public List<MaterialIssueReceiveItemDto> MaterialIssueReceiveItems { get; set; }
    
    }
}
