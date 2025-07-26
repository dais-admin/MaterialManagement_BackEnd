namespace DAIS.CoreBusiness.Helpers
{
    public class JwtTokenSettings
    {
        public string SecurityKey {  get; set; }
        public string Issuer {  get; set; }
        public string Audience { get; set; }
    }
}
