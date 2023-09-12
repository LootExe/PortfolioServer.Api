using System;
using System.ComponentModel.DataAnnotations;
using PortfolioServer.Api.Entity;

namespace PortfolioServer.Api.Dto
{
    public record TransactionDto(
        Guid Id,
        DateTime Timestamp,
        TransactionType Type,
        decimal Price,
        decimal Amount,
        Guid AssetId) {}

    public record TransactionCreateDto(
        [Required] DateTime? Timestamp,
        [Required] TransactionType? Type,
        [Required] decimal? Price,
        [Required] decimal? Amount,
        [Required] Guid? AssetId) {} 

    public record TransactionUpdateDto(
        [Required] DateTime? Timestamp,
        [Required] TransactionType? Type,
        [Required] decimal? Price,
        [Required] decimal? Amount) {}
}
