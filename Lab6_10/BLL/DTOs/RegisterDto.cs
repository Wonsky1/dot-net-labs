using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class RegisterDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    [MinLength(6)] public string Password { get; set; } = null!;
    [EmailAddress] public string Email { get; set; } = null!;
}
