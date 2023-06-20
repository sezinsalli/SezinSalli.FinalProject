
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
        private readonly IUserService _userService;
        private readonly JwtConfig _jwtConfig;

        public AuthenticationService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, IOptionsMonitor<JwtConfig> jwtConfig, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfig = jwtConfig.CurrentValue;
            _userService = userService;
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
            var roles = await _userManager.GetRolesAsync(user);
            if (roles is null)
                throw new ClientSideException($"User role was null!");

            string token = Token(user,roles.FirstOrDefault());

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

        public async Task ChangePassword(string userId, ChangePasswordRequest request)
        {
            if (request is null)
                throw new ClientSideException($"Request was null!");

            if (string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.OldPassword))
                throw new ClientSideException($"Request was null!");

            var user = _userService.GetById(userId);

            var response = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);

            if (!response.Succeeded)
                throw new ClientSideException($"Change password error");
        }

        private string Token(AppUser user,string role)
        {
            Claim[] claims = GetClaims(user,role);
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

        private Claim[] GetClaims(AppUser user, string role)
        {
            var claims = new[]
            {
            new Claim("UserName",user.UserName),
            new Claim("UserId",user.Id.ToString()),
            new Claim("FirstName",user.FirstName),
            new Claim("LastName",user.LastName),
            new Claim(ClaimTypes.Role,role),
            };

            return claims;
        }


    }
}
