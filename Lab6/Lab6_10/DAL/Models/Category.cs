namespace DAL.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int NewsId { get; set; }
    public News News { get; set; } = null!;
}
