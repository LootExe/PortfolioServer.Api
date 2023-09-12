using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortfolioServer.Api.Dto
{
    public record PortfolioDto(
        Guid Id,
        string Name,
        List<AssetDto> Assets) {}

    public record PortfolioCreateDto(
        [Required] string? Name) {}

    public record PortfolioUpdateDto(
        [Required] string? Name) {}
}