using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public class Author
{
    public int Id { get; set; }

    [MaxLength(50)] public string Name { get; set; } = default!;

    public int NewsId { get; set; }
    public News News { get; set; } = default!;
}
