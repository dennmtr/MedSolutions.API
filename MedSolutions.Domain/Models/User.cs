using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MedSolutions.Domain.Models;

public class User : IdentityUser
{
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    [MaxLength(20)]
    public string? MobileNumber { get; set; }
    public bool? IsActive { get; set; } = true;
    public MedicalProfile? MedicalProfile { get; set; }
}
