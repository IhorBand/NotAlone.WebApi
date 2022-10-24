using NotAlone.WebApi.ModelsDB;
using NotAlone.WebApi.ModelsWebApi.Authorize;

namespace NotAlone.WebApi.Services.Abstractions
{
    public interface IRefreshTokenService
    {
        public Task<RefreshToken> CreateRefreshTokenAsync(Guid userId);
        public Task<bool> CheckRefreshTokenAsync(Guid refreshToken, Guid userId);
    }
}
