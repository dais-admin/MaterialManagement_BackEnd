using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Dtos.Reports
{
    public class ApprovalInformationDto
    {
        public int ApprovalStatus { get; set; }
        public string? CurrentUserId {  get; set; }
        public string?[] ReviewerApproverIds {  get; set; }       
        public Guid MaterialId {  get; set; }
        public string? Comment { get; set; }
    }
}
