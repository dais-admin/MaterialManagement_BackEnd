namespace DAIS.CoreBusiness.Dtos
{
    public class SubDivisionMaterialTransferDto
    {
        public Guid? Id { get; set; }
        public string VoucherNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? Date { get; set; }
        public List<SubDivisionMaterialTransferItemDto> SubDivisionMaterialTransferItems { get; set; }
    }
}
