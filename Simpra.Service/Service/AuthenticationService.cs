
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Simpra.Core.Entity;
using Simpra.Core.Jwt;
using Simpra.Core.Service;
using Simpra.Schema.TokenRR;
using Simpra.Schema.UserRR;
using Simpra.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.Service
{
    public class AuthenticationService:IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtConfig _jwtConfig;

        public AuthenticationService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, IOptionsMonitor<JwtConfig> jwtConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfig = jwtConfig.CurrentValue;
        }

        public async Task<TokenResponse> SignIn(TokenRequest request)
        {
            if (request is null)
                throw new ClientSideException($"Request was null!");

            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                throw new ClientSideException($"Request was null!");

            var loginResult = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, true, false);
            if (!loginResult.Succeeded)
                throw new ClientSideException($"Invalid user!");

            var user = await _userManager.FindByNameAsync(request.UserName);

            string token = Token(user);

            TokenResponse tokenResponse = new TokenResponse
            {
                AccessToken = token,
                ExpireTime = DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpiration),
                UserName = user.UserName
            };

            return tokenResponse;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task ChangePassword(ClaimsPrincipal User, ChangePasswordRequest request)
        {
            if (request is null)
                throw new ClientSideException($"Request was null!");

            if (string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.OldPassword))
                throw new ClientSideException($"Request was null!");

            var user = await _userManager.GetUserAsync(User);
            var response = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);

            if (!response.Succeeded)
                throw new ClientSideException($"Change password error");
        }

        private string Token(AppUser user)
        {
            Claim[] claims = GetClaims(user);
            var secret = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var jwtToken = new JwtSecurityToken(
                _jwtConfig.Issuer,
                _jwtConfig.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessToken;
        }

        private Claim[] GetClaims(AppUser user)
        {
            var claims = new[]
            {
            new Claim("UserName",user.UserName),
            new Claim("UserId",user.Id.ToString()),
            new Claim("FirstName",user.FirstName),
            new Claim("LastName",user.LastName),
            new Claim("UserName",user.UserName)
        };

            return claims;
        }


    }
}
