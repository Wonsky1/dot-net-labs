using DAL.Models;

namespace BLL.DTOs;

public class SearchDto
{
    public string Select { get; set; } = null!;
    public string Value { get; set; } = null!;
}
