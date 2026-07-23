namespace DAIS.CoreBusiness.Dtos
{
    public class DocumentCategoryDto
    {
        private string categoryName;
        public Guid Id { get; set; }
        public string CategoryName
        {
            get => categoryName;
            set => categoryName = value?.ToUpper();
        }

        public string? Remarks { get; set; }
    }
}
