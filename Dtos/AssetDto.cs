using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortfolioServer.Api.Dto
{
    public record AssetDto(
        Guid Id,
        string Symbol,
        string Name,
        string Currency,
        int TransactionCount,
        Guid PortfolioId,
        List<TransactionDto>? Transactions = null) {}

    public record AssetCreateDto(
        [Required] string? Symbol,
        [Required] string? Name,
        [Required] string? Currency,
        [Required] Guid? PortfolioId) {}

    public record AssetUpdateDto(
        [Required] string? Symbol,
        [Required] string? Name,
        [Required] string? Currency) {}
}