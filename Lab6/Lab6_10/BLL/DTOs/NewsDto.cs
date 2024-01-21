namespace BLL.DTOs;

public class NewsDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public string Author { get; set; } = null!;
    public string Category { get; set; } = null!;
}
