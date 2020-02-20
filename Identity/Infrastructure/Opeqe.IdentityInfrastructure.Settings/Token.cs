namespace Opeqe.Identity.Infrastructure.Settings
{
    public class Token
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Key { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}