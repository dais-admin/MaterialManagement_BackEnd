using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
    public class SubDivisionDto
    {
        private string subDivisionName;
        public Guid Id { get; set; }
        public string SubDivisionName
        {
            get => subDivisionName;
            set => subDivisionName = value?.ToUpper();
        }
        public string SubDivisionCode { get; set; }
        public string? Remarks { get; set; }
        public Guid DivisionId { get; set; }
        public DivisionDto? Division { get; set; }
    }
}
