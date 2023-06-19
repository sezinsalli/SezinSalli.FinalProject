using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Service;
using Simpra.Schema.TokenRR;
using Simpra.Schema.UserRR;
using Simpra.Service.Response;

namespace Simpra.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : CustomBaseController
{
    private readonly IAuthenticationService service;

    public AuthenticationController(IAuthenticationService service)
    {
        this.service = service;
    }

    [HttpPost("SignIn")]
    public async Task<CustomResponse<TokenResponse>> SignIn(TokenRequest request)
    {
        var response = await service.SignIn(request);
        return CustomResponse<TokenResponse>.Success(200,response);
    }

    [HttpPost("SignOut")]
    public async Task<CustomResponse<NoContent>> SignOut()
    {
        await service.SignOut();
        return CustomResponse<NoContent>.Success(204);
    }

    [HttpPost("ChangePassword")]
    public async Task<CustomResponse<NoContent>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        await service.ChangePassword(HttpContext.User,request);
        return CustomResponse<NoContent>.Success(204);
    }    
}