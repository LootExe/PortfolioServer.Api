using System;
using System.Collections.Generic;

namespace PortfolioServer.Api.Entity
{
    public class Portfolio : EntityBase
    {        
        public string Name { get; set; } = string.Empty;
        public List<Asset> Assets { get; set; } = new();
    }
}