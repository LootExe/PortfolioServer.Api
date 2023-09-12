using System.Collections.Generic;
using System.Linq;
using PortfolioServer.Api.Dto;

namespace PortfolioServer.Api.Entity
{
    public static class EntityExtension
    {
        public static UserDto ToDto(this User user)
        {
            return new(
                GivenName: user.GivenName,
                Surname: user.Surname,
                Username: user.Username,
                Email: user.Email);
        }

        public static PortfolioDto ToDto(this Portfolio portfolio)
        {
            return new(
                Id: portfolio.Id,
                Name: portfolio.Name,
                Assets: portfolio.Assets.ToDtoList(false));
        }

        public static AssetDto ToDto(this Asset asset, bool includeTransactions = true)
        {
            return new(
                Id: asset.Id,
                Symbol: asset.Symbol,
                Name: asset.Name,
                Currency: asset.Currency,
                TransactionCount: asset.Transactions.Count,
                PortfolioId: asset.PortfolioId,
                Transactions: includeTransactions ? asset.Transactions.ToDtoList() : null);
        }

        public static TransactionDto ToDto(this Transaction transaction)
        {
            return new(
                Id: transaction.Id,
                Timestamp: transaction.Timestamp,
                Type: transaction.Type,
                Price: transaction.Price,
                Amount: transaction.Amount,
                AssetId: transaction.AssetId);
        }

        public static List<PortfolioDto> ToDtoList(this IList<Portfolio> portfolios)
        {
            return portfolios.Select(p => p.ToDto()).ToList();
        }

        public static List<AssetDto> ToDtoList(this IList<Asset> assets, bool includeTransactions = true)
        {   
            return assets.Select(a => a.ToDto(includeTransactions)).ToList();
        }

        public static List<TransactionDto> ToDtoList(this IList<Transaction> transactions)
        {
            return transactions.Select(t => t.ToDto()).ToList();
        }

        /// <summary>
        /// Called to create a new instance of a <see cref="LoginProvider"/>.
        /// </summary>
        /// <param name="user">The associated user.</param>
        /// <param name="login">The associated login.</param>
        /// <returns></returns>
        public static LoginProvider ToLoginProvider(this UserLogin login, User user)
        {
            return new(Provider: login.LoginProvider,
                       Key: login.ProviderKey,
                       DisplayName: login.ProviderDisplayName,
                       UserId: user.Id);
        }
    }
}