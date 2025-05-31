namespace Infrastructure.Options
{
    public class JwtOptions
    {
        public const string JwtOptionsKey = "JwtOptions"; // this string val must be the same as the name of the obj specified in appsettings.json

        // these prop names must be the same as the prop names of the JwtOptions specified in appsettings.json
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationTimeInMinutes { get; set; }
    }
}
