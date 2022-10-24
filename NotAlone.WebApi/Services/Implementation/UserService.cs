using Microsoft.EntityFrameworkCore;
using NotAlone.WebApi.Infrastructure.DB;
using NotAlone.WebApi.Infrastructure.Models.Configuration;
using NotAlone.WebApi.ModelsDB;
using NotAlone.WebApi.Services.Abstractions;

namespace NotAlone.WebApi.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly ILogger<TokenService> logger;
        private readonly JwtTokenConfiguration jwtTokenConfiguration;
        private readonly NotAloneDbContext dbContext;

        public UserService(
            ILogger<TokenService> logger,
            JwtTokenConfiguration jwtTokenConfiguration,
            NotAloneDbContext dbContext)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.jwtTokenConfiguration = jwtTokenConfiguration ?? throw new ArgumentNullException(nameof(jwtTokenConfiguration));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        Task<User?> IUserService.GetByEmailAndPasswordAsync(string email, string password)
        {
            var user = dbContext.Users.Where(u => u.Email.ToLower() == email.ToLower() && u.Password == password).FirstOrDefaultAsync();
            return user;
        }

        Task<User?> IUserService.GetByIdAsync(Guid id)
        {
            var user = dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }
    }
}
