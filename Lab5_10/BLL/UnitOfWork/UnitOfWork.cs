using BLL.Services;
using DAL;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BLL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly NewsDbContext _context;

    public UnitOfWork(NewsDbContext context, UserManager<User> userManager, IConfiguration configuration)
    {
        _context = context;
        UserAuthRepository = new UserAuthenticationRepository(userManager, configuration);
        News = new GenericRepository<News>(_context);
    }

    public IUserAuthenticationRepository UserAuthRepository { get; }
    public IRepository<News> News { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
