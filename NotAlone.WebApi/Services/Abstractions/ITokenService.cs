using NotAlone.WebApi.ModelsWebApi.Authorize;

namespace NotAlone.WebApi.Services.Abstractions
{
    public interface ITokenService
    {
        public Task<AuthorizeUserOutputModel?> RefreshTokenAsync(Guid refreshToken, Guid userId);
        public Task<AuthorizeUserOutputModel?> GetTokenAsync(string email, string password);
    }
}
