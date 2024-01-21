using System.Linq.Expressions;
using BLL.DTOs;
using BLL.UnitOfWork;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab4_10.Controllers;

[Route("/")]
public class NewsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public NewsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var news = await _unitOfWork.News
            .GetAllAsync(null, "Author", "Category");

        var indexModel = new IndexModel
        {
            News = news,
            SelectedOption = "all",
            Value = ""
        };

        return View(indexModel);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search(string select, string value)
    {
        Expression<Func<News, bool>>? expression = null;

        switch (select)
        {
            case "all":
                break;
            case "title":
                expression = news => news.Title.Contains(value);
                break;
            case "author":
                expression = news => news.Author.Name.Contains(value);
                break;
            case "category":
                expression = news => news.Category.Name.Contains(value);
                break;
            case "date":
                expression = news => news.PublishedDate.Date == DateTime.Parse(value).Date;
                break;
            default:
                expression = news => false;
                break;
        }

        var news = await _unitOfWork.News
            .GetAllAsync(expression, "Author", "Category");

        var indexModel = new IndexModel
        {
            News = news,
            SelectedOption = select,
            Value = value
        };

        return View("Index", indexModel);
    }

    [HttpGet("add")]
    [Authorize]
    public IActionResult Create()
    {
        return View(new NewsDto());
    }

    [HttpPost("add")]
    [Authorize]
    public async Task<IActionResult> Create(NewsDto dto)
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

        return RedirectToAction("Index");
    }
}
