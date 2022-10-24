using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NotAlone.WebApi.ModelsWebApi.Authorize;
using NotAlone.WebApi.Services.Abstractions;

namespace NotAlone.WebApi.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> logger;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;

        public TokenController(
            ILogger<TokenController> logger,
            IMapper mapper,
            ITokenService tokenService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenInputModel model)
        {
            if (model != null && model.RefreshToken != Guid.Empty && model.UserId != Guid.Empty)
            {
                var result = await this.tokenService.RefreshTokenAsync(model.RefreshToken, model.UserId).ConfigureAwait(false);
                if (result == null)
                {
                    return this.BadRequest("Invalid input.");
                }

                return this.Ok(result);
            }
            else
            {
                return this.BadRequest("Please, fill all fields.");
            }
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync(AuthorizeUserInputModel model)
        {
            if (model != null && model.Email != null && model.Password != null)
            {
                var result = await this.tokenService.GetTokenAsync(model.Email, model.Password).ConfigureAwait(false);
                if (result == null)
                {
                    return this.BadRequest("Invalid Credentials.");
                }

                return this.Ok(result);
            }
            else
            {
                return this.BadRequest("Please, fill email and password fields.");
            }
        }
    }
}
