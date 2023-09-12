namespace PortfolioServer.Api.Configuration
{
    public record DatabaseConfiguration
    {
        public const string Section = "Database";

        public string? Server { get; set; }
        public int Port { get; set; }
        public string? User { get; set; }
        // TODO: Move Pass to secure credential issuer (Google, Azure etc.)
        // Only for Development purpose
        public string? Pass { get; set; }
        public string? DatabaseName { get; set; }

        public string ConnectionString 
        { 
            get => $"server={Server};" +
                   $"port={Port};" +
                   $"user={User};" +
                   $"password={Pass};" +
                   $"database={DatabaseName}";
        }
    }
}