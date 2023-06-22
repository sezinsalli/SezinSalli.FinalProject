using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Jwt;
using Simpra.Core.Service;
using Simpra.Schema.TokenRR;
using Simpra.Schema.UserRR;
using Simpra.Service.Response;

namespace Simpra.Api.Controllers;

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
    public async Task<CustomResponse<TokenResponse>> SignIn([FromBody] TokenRequest request)
    {
        var response = await _service.SignIn(request);
        return CustomResponse<TokenResponse>.Success(200, response);
    }

    [HttpPost("SignOut")]
    public async Task<CustomResponse<NoContent>> SignOut()
    {
        await _service.SignOut();
        return CustomResponse<NoContent>.Success(204);
    }

    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<CustomResponse<NoContent>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;
        await _service.ChangePassword(userId, request);
        return CustomResponse<NoContent>.Success(204);
    }

}
