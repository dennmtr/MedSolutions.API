using System.ComponentModel.DataAnnotations;

namespace MedSolutions.App.DTOs;

public class LoginRequestDTO
{
    [MaxLength(256, ErrorMessage = "validation.email.length")]
    [Required(ErrorMessage = "validation.email.required")]
    [EmailAddress(ErrorMessage = "validation.email")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "validation.password.length")]
    [StringLength(128, ErrorMessage = "", MinimumLength = 6)]
    public required string Password { get; set; }
    public bool? RememberMe { get; set; } = false;
}
