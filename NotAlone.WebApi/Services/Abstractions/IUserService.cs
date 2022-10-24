using NotAlone.WebApi.ModelsDB;

namespace NotAlone.WebApi.Services.Abstractions
{
    public interface IUserService
    {
        public Task<User?> GetByIdAsync(Guid id);
        public Task<User?> GetByEmailAndPasswordAsync(string email, string password);
    }
}
