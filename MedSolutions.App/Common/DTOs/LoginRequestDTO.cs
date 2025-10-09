using System.ComponentModel.DataAnnotations;

namespace MedSolutions.App.Common.DTOs;

public class LoginRequestDTO
{
    [MaxLength(256)]
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(128, MinimumLength = 6)]
    public required string Password { get; set; }
    public bool? RememberMe { get; set; }
}
