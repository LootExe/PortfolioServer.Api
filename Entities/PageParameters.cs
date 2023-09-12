using System;

namespace PortfolioServer.Api.Entity
{
    public record PageParameters
    {
        public PageParameters(int page, int size)
        {
            Page = page;
            Size = Math.Min(size, MaxSize);
        }
        public PageParameters()
        {
            Page = 1;
            Size = 10;
        } 

        public const int MaxSize = 50;

        public int Page { get; private set; }
        public int Size { get; private set; }
    }
}