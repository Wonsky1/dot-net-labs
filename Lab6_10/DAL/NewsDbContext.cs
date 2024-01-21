using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class NewsDbContext : IdentityDbContext<User>
{
    public DbSet<News> News { get; set; } = null!;

    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
    {
    }
}
