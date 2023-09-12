using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioServer.Api.Entity;

namespace PortfolioServer.Api.Repository
{
    public class TransactionRepository : RepositoryBase<PortfolioDbContext, Transaction>, ITransactionRepository
    {
        public TransactionRepository(PortfolioDbContext context) : base(context) {}

        public async Task CreateAsync(Transaction transaction) => await CreateEntityAsync(transaction);
        public async Task UpdateAsync(Transaction transaction) => await UpdateEntityAsync(transaction);
        public async Task DeleteAsync(Transaction transaction) => await DeleteEntityAsync(transaction);

        public async Task<Transaction?> FindByIdAsync(Guid transactionId, CancellationToken cancel = default)
        {
            return await Entities.FindAsync(new object[] { transactionId }, cancel);
        }

        public async Task<Transaction?> FindByUserAsync(Guid userId, Guid transactionId, CancellationToken cancel = default)
        {
            return await Entities.SingleOrDefaultAsync(t => t.Id.Equals(transactionId) && t.UserId.Equals(userId), cancel);
        }

        public async Task<IList<Transaction>> GetAllAsync(
            Guid userId,
            Guid assetId,
            PageParameters? parameters = null,
            CancellationToken cancel = default)
        {
            parameters ??= new PageParameters();
            // TODO: Add OrderBy function
            return await Entities.Where(t => t.AssetId.Equals(assetId) && t.UserId.Equals(userId))
                                 .Skip((parameters.Page - 1) * parameters.Size)
                                 .Take(parameters.Size)
                                 .ToListAsync(cancel);
        }
    }
}