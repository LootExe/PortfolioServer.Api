using System;

namespace PortfolioServer.Api.Entity
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string GivenName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}