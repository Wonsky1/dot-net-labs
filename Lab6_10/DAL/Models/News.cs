namespace DAL.Models;

public class News
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime PublishedDate { get; set; }

    public virtual Author Author { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;

    public override string ToString()
    {
        return
            $"Title: {Title}, Content: {Content}, PublishedDate: {PublishedDate}, Author: {Author.Name}, Category: {Category.Name}";
    }
}
