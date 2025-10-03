using System.ComponentModel.DataAnnotations;

namespace MedSolutions.App.DTOs;

public class LoginRequestDTO
{
    [MaxLength(256, ErrorMessage = "error.validation.email_length")]
    [Required(ErrorMessage = "error.validation.email_required")]
    [EmailAddress(ErrorMessage = "error.validation.email_format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "error.validation.password_required")]
    [StringLength(128, ErrorMessage = "error.validation.password_length", MinimumLength = 6)]
    public required string Password { get; set; }
    public bool RememberMe { get; set; }
}
