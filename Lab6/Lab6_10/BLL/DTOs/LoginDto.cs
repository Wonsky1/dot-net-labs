using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class LoginDto
{
    [MinLength(6)] public string Password { get; set; } = null!;
    [EmailAddress] public string Email { get; set; } = null!;
}
