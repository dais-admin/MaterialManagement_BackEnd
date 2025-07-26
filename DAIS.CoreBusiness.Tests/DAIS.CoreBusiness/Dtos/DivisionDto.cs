namespace DAIS.CoreBusiness.Dtos
{
    public class DivisionDto
    {
        private string divisionName;
        public Guid Id {  get; set; }
        public string DivisionName
        {
            get => divisionName;
            set => divisionName = value?.ToUpper();
        }
        public string DivisionCode { get; set; }
        public string Remarks {  get; set; }
        public Guid? LocationId { get; set; }
        public Guid? SubDivisionId { get; set; }
        public LocationOperationDto? Location { get; set; }
        public SubDivisionDto?  SubDivision { get; set; }

    }
}
