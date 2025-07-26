using System;
using System.Collections.Generic;

namespace DAIS.CoreBusiness.Dtos
{
    public class DivisionLocationMaterialTransferDto
    {
        public Guid? Id { get; set; }
        public string VoucherNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? Date { get; set; }
        public List<DivisionLocationMaterialTransferItemDto> DivisionLocationMaterialTransferItems { get; set; }
    }
}
