using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Dtos
{
    public class MaterialMeasuremetDto
    {
        private string measurementName;
        public Guid? Id { get; set; }
        public string MeasurementName
        {
            get => measurementName;
            set => measurementName = value?.ToUpper();
        }

        public string MeasurementCode { get; set; }

    }
}
