
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Jwt;
using Simpra.Core.Service;
using Simpra.Schema.TokenRR;
using Simpra.Schema.UserRR;
using Simpra.Service.Response;

namespace Simpra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : CustomBaseController
    {
        private readonly IAuthenticationService _service;
        public AuthenticationController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] TokenRequest request)
        {
            var response = await _service.SignInAsync(request);
            return CreateActionResult(CustomResponse<TokenResponse>.Success(200, response));
        }

        [HttpPost("SignOut")]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await _service.SignOutAsync();
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;
            await _service.ChangePasswordAsync(userId, request);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

    }
}
