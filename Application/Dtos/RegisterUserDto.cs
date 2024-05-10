#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record RegisterUserDto
{
    [Required]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required, Compare(nameof(Password))]
    public string PasswordHash { get; set; }
}
