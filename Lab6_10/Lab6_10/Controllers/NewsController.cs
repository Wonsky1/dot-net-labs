using System.Linq.Expressions;
using BLL.DTOs;
using BLL.UnitOfWork;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab6_10.Controllers;

[ApiController]
[Route("/api/news/[action]")]
public class NewsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public NewsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [ActionName("")]
    public async Task<IActionResult> Index()
    {
        var news = await _unitOfWork.News
            .GetAllAsync(null, "Author", "Category");

        return Json(new { news, auth = User.Identity?.IsAuthenticated });
    }

    [HttpPost]
    [ActionName("search")]
    public async Task<IActionResult> Search([FromBody] SearchDto dto)
    {
        Expression<Func<News, bool>>? expression = null;

        switch (dto.Select)
        {
            case "all":
                break;
            case "title":
                expression = news => news.Title.Contains(dto.Value);
                break;
            case "author":
                expression = news => news.Author.Name.Contains(dto.Value);
                break;
            case "category":
                expression = news => news.Category.Name.Contains(dto.Value);
                break;
            case "date":
                expression = news => news.PublishedDate.Date == DateTime.Parse(dto.Value).Date;
                break;
            default:
                expression = news => false;
                break;
        }

        var news = await _unitOfWork.News
            .GetAllAsync(expression, "Author", "Category");

        return Json(news);
    }

    [HttpPost]
    [Authorize]
    [ActionName("add")]
    public async Task<IActionResult> Create([FromBody] NewsDto dto)
    {
        var user = await _unitOfWork.UserAuthRepository
            .GetUserByEmailAsync(User.Identity!.Name!);

        if (user is null)
            return NotFound("User not found.");

        var news = new News
        {
            Title = dto.Title,
            Content = dto.Content,
            PublishedDate = DateTime.Now,
            Author = new Author { Name = dto.Author },
            Category = new Category { Name = dto.Category },
            User = user
        };

        await _unitOfWork.News.AddAsync(news);
        await _unitOfWork.SaveChangesAsync();

        return Ok("Новину успішно додано.");
    }
}
