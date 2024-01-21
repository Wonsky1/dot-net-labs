using BLL.Services;
using DAL.Models;
using DAL.Repositories;

namespace BLL.UnitOfWork;

public interface IUnitOfWork
{
    IUserAuthenticationRepository UserAuthRepository { get; }
    IRepository<News> News { get; }
    Task<int> SaveChangesAsync();
}
