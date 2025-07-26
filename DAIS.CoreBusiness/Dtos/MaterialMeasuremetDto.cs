using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialMeasuremetDto
    {
        public Guid? Id { get; set; }
        public string MeasurementName { get; set; }
        public string MeasurementCode { get; set; }

    }
}
