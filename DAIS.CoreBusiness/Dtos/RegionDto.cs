namespace DAIS.CoreBusiness.Dtos
{
    public class RegionDto
    {
        private string regionName;
        public Guid Id { get; set; }
        public string RegionName
        {
            get => regionName;
            set => regionName = value?.ToUpper();
        }
        public string RegionCode { get; set; }
        public string Remarks {  get; set; }



    }
}
