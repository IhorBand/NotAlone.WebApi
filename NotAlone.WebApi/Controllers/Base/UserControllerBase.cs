using Microsoft.AspNetCore.Mvc;
using NotAlone.WebApi.Infrastructure.Models.Claims;

namespace NotAlone.WebApi.Controllers.Base
{
    public class UserControllerBase : ControllerBase
    {
        public Guid UserId
        {
            get
            {
                var idStr = this.GetClaimValue(JwtCustomClaimNames.UserId);
                return new Guid(idStr);
            }
        }

        public string Email { get => this.GetClaimValue(JwtCustomClaimNames.Email); }
        public string UserName { get => this.GetClaimValue(JwtCustomClaimNames.UserName); }
        public string DisplayName { get => this.GetClaimValue(JwtCustomClaimNames.DisplayName); }

        private string GetClaimValue(string claimName)
        {
            if (this.User != null)
            {
                var claim = this.User.Claims.FirstOrDefault(c => c.Type == claimName);
                if (claim != null)
                {
                    return claim.Value;
                }
            }

            return string.Empty;
        }
    }
}
