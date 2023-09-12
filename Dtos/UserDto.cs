using System.ComponentModel.DataAnnotations;

namespace PortfolioServer.Api.Dto
{
    public record UserDto(
        [Required] string GivenName,
        [Required] string Surname,
        [Required] string Username,
        [Required] string Email) {}
}