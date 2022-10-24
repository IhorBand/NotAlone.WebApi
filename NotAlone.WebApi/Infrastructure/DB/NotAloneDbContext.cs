using Microsoft.EntityFrameworkCore;
using NotAlone.WebApi.ModelsDB;

namespace NotAlone.WebApi.Infrastructure.DB
{
    public class NotAloneDbContext : DbContext
    {
        //Constructor with DbContextOptions and the context class itself
        public NotAloneDbContext(DbContextOptions<NotAloneDbContext> options) : base(options)
        {
        }

        //Create the DataSet for the Employee         
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
