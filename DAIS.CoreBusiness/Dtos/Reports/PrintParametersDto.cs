using System;

namespace DAIS.CoreBusiness.Dtos.Reports
{
    public class PrintParametersDto
    {
        public string Orientation { get; set; } // "Portrait" or "Landscape"
        public string Data { get; set; } // JSON string of data to print
        public int PageSize { get; set; } // Items per page
        public string ReportTitle { get; set; }
        public string[] Headers { get; set; } // Column headers
    }
}
