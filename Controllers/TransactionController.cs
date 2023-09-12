using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortfolioServer.Api.Dto;
using PortfolioServer.Api.Entity;
using PortfolioServer.Api.Repository;

namespace PortfolioServer.Api.Controller
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : UserControllerBase
    {
        public TransactionController(
            IUserRepository userRepository,
            ITransactionRepository transactionRepository,
            IAssetRepository assetRepository) : base(userRepository)
        {
            repository = transactionRepository;
            this.assetRepository = assetRepository;
        }

        private readonly ITransactionRepository repository;
        private readonly IAssetRepository assetRepository;

        public const string QueryParameterError = "query parameter is missing. parameter: ";
        public const string ParentNotFoundError = "parent asset not found. id: ";

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateAsync([FromBody] TransactionCreateDto data)
        {
            var exists = await assetRepository.ExistsForUserAsync(data.AssetId!.Value,
                                                                  CurrentUser.Id);

            if (exists is false)
                return NotFound($"{ParentNotFoundError}{data.AssetId!.Value}");

            var transaction = new Transaction()
            {
                Timestamp = data.Timestamp!.Value,
                Type = data.Type!.Value,
                Price = data.Price!.Value,
                Amount = data.Amount!.Value,
                AssetId = data.AssetId!.Value,
                UserId = CurrentUser.Id
            };
            await repository.CreateAsync(transaction);

            return CreatedAtAction(actionName: nameof(GetAsync),
                                   routeValues: new { id = transaction.Id },
                                   value: transaction.ToDto());
        }

        [HttpGet]
        public async Task<ActionResult<IList<TransactionDto>>> GetAllAsync(
            [FromQuery] Guid assetId, [FromQuery] PageParameters parameters)
        {
            if (assetId.Equals(Guid.Empty))
                return BadRequest($"{QueryParameterError}{nameof(assetId)}");

            var exists = await assetRepository.ExistsForUserAsync(assetId,
                                                                  CurrentUser.Id);

            if (exists is false)
                return NotFound($"{ParentNotFoundError}{assetId}");

            var transactions = await repository.GetAllAsync(userId: CurrentUser.Id,
                                                            assetId: assetId,
                                                            parameters: parameters);
            return Ok(transactions.ToDtoList());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<TransactionDto>> GetAsync(Guid id)
        {
            var transaction = await repository.FindByUserAsync(CurrentUser.Id, id);
            return transaction is null ? NotFound() : Ok(transaction.ToDto()); 
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, TransactionUpdateDto data)
        {
            var transaction = await repository.FindByUserAsync(CurrentUser.Id, id);

            if (transaction is null)
                return NotFound();

            transaction.Timestamp = data.Timestamp!.Value;
            transaction.Type = data.Type!.Value;
            transaction.Price = data.Price!.Value;
            transaction.Amount = data.Amount!.Value;

            await repository.UpdateAsync(transaction);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var transaction = await repository.FindByUserAsync(CurrentUser.Id, id);

            if (transaction is null)
                return NotFound();

            await repository.DeleteAsync(transaction);
            return NoContent();
        }
    }
}