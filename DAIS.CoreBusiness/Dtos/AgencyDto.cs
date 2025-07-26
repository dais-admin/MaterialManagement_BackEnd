using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
    public class AgencyDto
    {
        public string Id { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public string AgencyType { get; set; }
        public string? AgencyAddress { get; set; }
        public string? AgencyEmail { get; set; }
        public string? AgencyPhone { get; set; }
        public string? Remarks { get; set; }

    }
}
