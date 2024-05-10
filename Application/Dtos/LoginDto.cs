#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
