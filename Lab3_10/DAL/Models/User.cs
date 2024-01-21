using Microsoft.AspNetCore.Identity;

namespace DAL.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}";

    public ICollection<News> News { get; set; } = null!;
}
