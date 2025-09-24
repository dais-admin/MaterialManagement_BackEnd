using DAIS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
    public  class MaterialServiceProviderDto
    {
       
        private string serviceProviderName;
        public Guid Id { get; set; }
        public string ServiceProviderName
        {
            get => serviceProviderName;
            set => serviceProviderName = value?.ToUpper();
        }

        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string ContactEmail { get; set; }
        public string? Remarks {  get; set; }
        public string? ServiceProviderDocument { get; set; }
        public Guid ManufacturerId { get; set; }
        public Guid? ContractorId { get; set; }
        public  ContractorDto? Contractor { get; set; }
  

    }
}
