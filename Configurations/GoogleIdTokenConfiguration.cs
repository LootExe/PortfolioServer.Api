namespace PortfolioServer.Api.Configuration
{
    public record GoogleIdTokenConfiguration
    {
        public const string Section = "GoogleIdToken";
        
        public string[]? Issuers { get; set; }
        public string? ClientId { get; set; }
        public string? JwksUrl { get; set; }
    }
}