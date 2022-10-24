using Microsoft.EntityFrameworkCore;
using NotAlone.WebApi.Infrastructure.DB;
using NotAlone.WebApi.Infrastructure.Models.Configuration;
using NotAlone.WebApi.ModelsDB;
using NotAlone.WebApi.Services.Abstractions;

namespace NotAlone.WebApi.Services.Implementation
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ILogger<RefreshTokenService> logger;
        private readonly JwtTokenConfiguration jwtTokenConfiguration;
        private readonly NotAloneDbContext dbContext;

        public RefreshTokenService(
            ILogger<RefreshTokenService> logger,
            JwtTokenConfiguration jwtTokenConfiguration,
            NotAloneDbContext dbContext)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.jwtTokenConfiguration = jwtTokenConfiguration ?? throw new ArgumentNullException(nameof(jwtTokenConfiguration));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
            var model = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid(),
                UserId = userId,
                CreatedDateUTC = now,
                ExpiredDateUTC = now.AddMinutes(jwtTokenConfiguration.RefreshTokenExpiresIn)
            };

            await this.dbContext.RefreshTokens.AddAsync(model);
            await this.dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<bool> CheckRefreshTokenAsync(Guid refreshToken, Guid userId)
        {
            var count = await this.dbContext.RefreshTokens.Where(rt => rt.Token == refreshToken && rt.UserId == userId && rt.ExpiredDateUTC >= DateTime.UtcNow).CountAsync();
            return count > 0 ? true : false;
        }
    }
}
