using System;
using System.Collections.Generic;

namespace PortfolioServer.Api.Entity
{
    public class Asset : EntityBase
    {
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public List<Transaction> Transactions { get; set; } = new();
        public Guid PortfolioId { get; set; }
    }
}