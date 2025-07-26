using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
    public class AgencyDto
    {
        private string agencyCode;
        private string agencyName;

        public Guid Id { get; set; }

        public string AgencyCode
        {
            get => agencyCode;
            set => agencyCode = value?.ToUpper();
        }

        public string AgencyName
        {
            get => agencyName;
            set => agencyName = value?.ToUpper();
        }

        public string AgencyType { get; set; }
        public string? AgencyAddress { get; set; }
        public string? AgencyEmail { get; set; }
        public string? AgencyPhone { get; set; }
        public string? Remarks { get; set; }
    }
}
