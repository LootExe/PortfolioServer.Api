using System;

namespace PortfolioServer.Api.Entity
{
    public class Transaction : EntityBase
    {
        public DateTime Timestamp { get; set; }
        public TransactionType Type { get; set; } = TransactionType.Unknown;
        public decimal Price { get; set; } = 0;
        public decimal Amount { get; set; } = 0;
        public Guid AssetId { get; set; }
    }

    
}