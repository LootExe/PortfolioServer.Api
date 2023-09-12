using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioServer.Api.Entity;

namespace PortfolioServer.Api.Repository
{
    public class AssetRepository : RepositoryBase<PortfolioDbContext, Asset>, IAssetRepository
    {
        public AssetRepository(PortfolioDbContext context) : base(context) {}

        public async Task CreateAsync(Asset asset) => await CreateEntityAsync(asset);
        public async Task UpdateAsync(Asset asset) => await UpdateEntityAsync(asset);
        public async Task DeleteAsync(Asset asset) => await DeleteEntityAsync(asset);

        public async Task<Asset?> FindByIdAsync(Guid assetId, CancellationToken cancel = default)
        {
            return await Entities.Where(a => a.Id.Equals(assetId))
                                 .Include(a => a.Transactions)
                                 .SingleOrDefaultAsync(cancel);
        }

        public async Task<Asset?> FindByUserAsync(Guid userId, Guid assetId, CancellationToken cancel = default)
        { 
            return await Entities.Where(a => a.Id.Equals(assetId) && a.UserId.Equals(userId))
                                 .Include(a => a.Transactions)
                                 .SingleOrDefaultAsync(cancel);
        }

        public async Task<IList<Asset>> GetAllAsync(Guid userId, CancellationToken cancel = default)
        {
            return await Entities.Where(a => a.UserId.Equals(userId))
                                 .Include(a => a.Transactions)
                                 .AsSplitQuery()
                                 .ToListAsync(cancel);
        }

        public async Task<IList<Asset>> GetAllAsync(Guid userId, Guid portfolioId, CancellationToken cancel = default)
        {
            return await Entities.Where(a => a.UserId.Equals(userId) && a.PortfolioId.Equals(portfolioId))
                                 .Include(a => a.Transactions)
                                 .AsSplitQuery()
                                 .ToListAsync(cancel);
        }

        public async Task<bool> ExistsForUserAsync(Guid assetId, Guid userId, CancellationToken cancel = default)
        {
            var asset = await FindEntityByIdAsync(assetId, cancel);
            return asset?.UserId.Equals(userId) ?? false;
        }
    }
}