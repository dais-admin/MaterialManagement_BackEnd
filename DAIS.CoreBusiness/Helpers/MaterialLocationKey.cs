namespace DAIS.CoreBusiness.Helpers
{
    public class MaterialLocationKey
    {
        public Guid MaterialId { get; set; }
       
        // Override Equals and GetHashCode for dictionary key comparison
        public override bool Equals(object obj)
        {
            if (obj is MaterialLocationKey other)
            {
                return MaterialId == other.MaterialId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MaterialId);
        }
    }

}
