using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PortfolioServer.Api.Entity;

namespace PortfolioServer.Api.Repository
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Creates the specified <paramref name="transaction"/> in the database.
        /// </summary>
        /// <param name="transaction">The transaction to create.</param>
        Task CreateAsync(Transaction transaction);
        /// <summary>
        /// Updates the specified <paramref name="transaction"/> in the database.
        /// </summary>
        /// <param name="transaction">The transaction to update.</param>
        Task UpdateAsync(Transaction transaction);
         /// <summary>
        /// Deletes the specified <paramref name="transaction"/> from database.
        /// </summary>
        /// <param name="transaction">The transaction to delete.</param>
        Task DeleteAsync(Transaction transaction);
        /// <summary>
        /// Finds and returns a transaction, if any, who has the specified <paramref name="transactionId"/>.
        /// </summary>
        /// <param name="transactionId">The transaction ID to search for.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the transaction matching the specified <paramref name="transactionId"/> if it exists.
        /// </returns>
        Task<Transaction?> FindByIdAsync(Guid transactionId, CancellationToken cancel = default);
        /// <summary>
        /// Finds and returns a transaction, if any, who has the specified <paramref name="userId"/> and <paramref name="transactionId"/>.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <param name="transactionId">The transaction ID to search for.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the transaction matching the specified <paramref name="transactionId"/> if it exists.
        /// </returns>
        Task<Transaction?> FindByUserAsync(Guid userId, Guid transactionId, CancellationToken cancel = default);
        /// <summary>
        /// Retrieves the associated transactions for the specified <paramref name="userId"/> and <paramref name="assetId"/>.
        /// </summary>
        /// <param name="userId">The user whose associated transactions to retrieve.</param>
        /// <param name="assetId">The asset whose associated transactions to retrieve.</param>
        /// <param name="parameters">The pagination parameters.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see cref="transaction"/> for the specified <paramref name="userId"/>, if any.
        /// </returns>
        Task<IList<Transaction>> GetAllAsync(Guid userId, Guid assetId, PageParameters? parameters = null, CancellationToken cancel = default);
    }
}