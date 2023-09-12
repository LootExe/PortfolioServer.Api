using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioServer.Api.Entity;

namespace PortfolioServer.Api.Repository
{
    public class PortfolioRepository : RepositoryBase<PortfolioDbContext, Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(PortfolioDbContext context) : base(context) {}

        public async Task CreateAsync(Portfolio portfolio) => await CreateEntityAsync(portfolio);
        public async Task UpdateAsync(Portfolio portfolio) => await UpdateEntityAsync(portfolio);
        public async Task DeleteAsync(Portfolio portfolio) => await DeleteEntityAsync(portfolio);

        public async Task<Portfolio?> FindByIdAsync(Guid portfolioId, CancellationToken cancel = default)
        {
            return await Entities.Where(p => p.Id.Equals(portfolioId))
                                 .Include(p => p.Assets)
                                 .ThenInclude(a => a.Transactions)
                                 .AsSplitQuery()
                                 .SingleOrDefaultAsync(cancel);
        }

        public async Task<Portfolio?> FindByUserAsync(Guid userId, Guid portfolioId, CancellationToken cancel = default)
        {
            return await Entities.Where(p => p.Id.Equals(portfolioId) && p.UserId.Equals(userId))
                                 .Include(p => p.Assets)
                                 .ThenInclude(a => a.Transactions)
                                 .AsSplitQuery()
                                 .SingleOrDefaultAsync(cancel);
        }

        public async Task<IList<Portfolio>> GetAllAsync(Guid userId, CancellationToken cancel = default)
        {
            return await Entities.Where(p => p.UserId.Equals(userId))
                                 .Include(p => p.Assets)
                                 .ThenInclude(a => a.Transactions)
                                 .AsSplitQuery()
                                 .ToListAsync(cancel);
        }

        public async Task<bool> ExistsForUserAsync(Guid portfolioId, Guid userId, CancellationToken cancel = default)
        {
            var portfolio = await FindEntityByIdAsync(portfolioId, cancel);
            return portfolio?.UserId.Equals(userId) ?? false;
        }
    }
}