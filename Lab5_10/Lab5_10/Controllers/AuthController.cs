using BLL.DTOs;
using BLL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Lab4_10.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _unitOfWork.UserAuthRepository.ValidateUser(dto);

        if (user == null)
            return BadRequest("Invalid credentials");

        var token = await _unitOfWork.UserAuthRepository.CreateTokenAsync(user);

        HttpContext.Session.SetString("token", token);

        return RedirectToAction("Index", "News");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var res = await _unitOfWork.UserAuthRepository.RegisterUserAsync(dto);

        if (!res.Succeeded)
            return BadRequest(string.Join("\n", res.Errors.Select(e => e.Description)));

        var user = await _unitOfWork.UserAuthRepository
            .ValidateUser(new LoginDto { Email = dto.Email, Password = dto.Password });

        var token = await _unitOfWork.UserAuthRepository.CreateTokenAsync(user!);

        HttpContext.Session.SetString("token", token);

        return RedirectToAction("Index", "News");
    }
}
