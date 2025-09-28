
using DAIS.DataAccess.Helpers;

namespace DAIS.CoreBusiness.Helpers
{
    public class ApprovalStatusDescriptions
    {
        private static readonly Dictionary<ApprovalStatus, string> _descriptions = new()
        {
            { ApprovalStatus.Submmitted, "Submitted" },
            { ApprovalStatus.Reviewed, "Reviewed" },
            { ApprovalStatus.ReviewerReturned, "Returned by Reviewer " },
            { ApprovalStatus.ReviewerRejected, "Rejected by Reviewer" },
            { ApprovalStatus.Approved, "Approved" },
            { ApprovalStatus.ApproverReturened, "Returened by Approver" },
            { ApprovalStatus.ApproverRejected, "Rejected by Approver" },
        };

        public static string GetDescription(ApprovalStatus status) =>
            _descriptions.TryGetValue(status, out var description) ? description : status.ToString();
    }
}
