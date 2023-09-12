using System;

namespace PortfolioServer.Api.Entity
{
    public abstract class EntityBase 
    { 
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
    }
}