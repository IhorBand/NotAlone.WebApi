using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NotAlone.WebApi.Infrastructure.Models.Claims;
using NotAlone.WebApi.Infrastructure.Models.Configuration;
using NotAlone.WebApi.ModelsDB;
using NotAlone.WebApi.ModelsWebApi.Authorize;
using NotAlone.WebApi.Services.Abstractions;

namespace NotAlone.WebApi.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> logger;
        private readonly IUserService userService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly JwtTokenConfiguration jwtTokenConfiguration;

        public TokenService(
            ILogger<TokenService> logger,
            IUserService userService,
            IRefreshTokenService refreshTokenService,
            JwtTokenConfiguration jwtTokenConfiguration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            this.jwtTokenConfiguration = jwtTokenConfiguration ?? throw new ArgumentNullException(nameof(jwtTokenConfiguration));
        }

        public async Task<AuthorizeUserOutputModel?> RefreshTokenAsync(Guid refreshToken, Guid userId)
        {
            var user = await this.userService.GetByIdAsync(userId).ConfigureAwait(false);

            if(user != null)
            {
                var isValid = await this.refreshTokenService.CheckRefreshTokenAsync(refreshToken, userId).ConfigureAwait(false);
                
                if (isValid)
                {
                    return await GenerateToken(user);
                }
            }

            return null;
        }

        public async Task<AuthorizeUserOutputModel?> GetTokenAsync(string email, string password)
        {
            var user = await this.userService.GetByEmailAndPasswordAsync(email, password).ConfigureAwait(false);

            if (user != null)
            {
                return await GenerateToken(user);
            }

            return null;
        }

        private async Task<AuthorizeUserOutputModel> GenerateToken(User user)
        {
            var now = DateTime.UtcNow;
            var expiredIn = now.AddMinutes(jwtTokenConfiguration.TokenExpiresIn);

            var claims = new[]
                {
                        new Claim(JwtRegisteredClaimNames.Sub, this.jwtTokenConfiguration.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, now.ToString()),
                        new Claim(JwtCustomClaimNames.UserId, user.Id.ToString()),
                        new Claim(JwtCustomClaimNames.DisplayName, user.DisplayName),
                        new Claim(JwtCustomClaimNames.UserName, user.UserName),
                        new Claim(JwtCustomClaimNames.Email, user.Email)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtTokenConfiguration.Key));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                this.jwtTokenConfiguration.Issuer,
                this.jwtTokenConfiguration.Audience,
                claims,
                expires: expiredIn,
                signingCredentials: signIn);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var tokenResult = jwtSecurityTokenHandler.WriteToken(token);

            var refreshToken = await this.refreshTokenService.CreateRefreshTokenAsync(user.Id).ConfigureAwait(false);

            var expiresIn = expiredIn - now;

            return new AuthorizeUserOutputModel()
            {
                ExpiresInSeconds = (int)expiresIn.TotalSeconds,
                ExpiresInUTC = expiredIn.ToString(),
                IssuedInUTC = now.ToString(),
                RefreshToken = refreshToken.Token.ToString(),
                TokenType = "Bearer",
                Token = tokenResult,
                UserId = user.Id.ToString(),
                UserName = user.UserName,
                RefreshTokenExpiresInSeconds = (int)(refreshToken.ExpiredDateUTC - refreshToken.CreatedDateUTC).TotalSeconds,
                RefreshTokenExpiresInUTC = refreshToken.ExpiredDateUTC.ToString(),
                RefreshTokenIssuedInUTC = refreshToken.CreatedDateUTC.ToString()
            };
        }
    }
}
