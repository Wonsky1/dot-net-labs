using BLL.DTOs;
using BLL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Lab6_10.Controllers;

[ApiController]
[Route("api/auth/[action]")]
public class AuthController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    [ActionName("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _unitOfWork.UserAuthRepository.ValidateUser(dto);

        if (user == null)
            return BadRequest("Invalid credentials");

        var token = await _unitOfWork.UserAuthRepository.CreateTokenAsync(user);

        return Json(token);
    }

    [HttpPost]
    [ActionName("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var res = await _unitOfWork.UserAuthRepository.RegisterUserAsync(dto);

        if (!res.Succeeded)
            return BadRequest(string.Join("\n", res.Errors.Select(e => e.Description)));

        var user = await _unitOfWork.UserAuthRepository
            .ValidateUser(new LoginDto { Email = dto.Email, Password = dto.Password });

        var token = await _unitOfWork.UserAuthRepository.CreateTokenAsync(user!);

        return Json(token);
    }
}
