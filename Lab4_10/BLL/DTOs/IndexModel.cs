using DAL.Models;

namespace BLL.DTOs;

public class IndexModel
{
    public IEnumerable<News> News { get; set; } = null!;

    public string SelectedOption { get; set; } = null!;
    public string Value { get; set; } = null!;
}
