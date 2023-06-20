using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Core.Role;
using Simpra.Core.Service;
using Simpra.Schema.UserRR;
using Simpra.Service.Response;

namespace Simpra.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : CustomBaseController
{
    private readonly IUserService _service;
    private readonly IMapper _mapper;

    public UserController(IUserService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = Role.Admin)]
    public CustomResponse<List<AppUserResponse>> GetAll()
    {
        var users = _service.GetAll();
        var usersResponse = _mapper.Map<List<AppUserResponse>>(users);
        return CustomResponse<List<AppUserResponse>>.Success(200, usersResponse);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Role.User)]
    public CustomResponse<AppUserResponse> GetById(string id)
    {
        var user = _service.GetById(id);
        var userResponse = _mapper.Map<AppUserResponse>(user);
        return CustomResponse<AppUserResponse>.Success(200, userResponse);
    }

    [HttpGet("[action]")]
    public async Task<CustomResponse<AppUserResponse>> GetUser()
    {
        var user = await _service.GetUserAsync(HttpContext.User);
        var userResponse = _mapper.Map<AppUserResponse>(user);
        return CustomResponse<AppUserResponse>.Success(200, userResponse);
    }

    [HttpGet("[action]")]
    public CustomResponse<string> GetUserId()
    {
        var userId = _service.GetUserId(HttpContext.User);
        return CustomResponse<string>.Success(200, userId);
    }

    [HttpPost("[action]")]
    public async Task<CustomResponse<AppUserResponse>> CreateUser([FromBody] AppUserCreateRequest request)
    {
        var user = await _service.InsertAsync(_mapper.Map<AppUser>(request), request.Password,Role.User);
        var userResponse = _mapper.Map<AppUserResponse>(user);
        return CustomResponse<AppUserResponse>.Success(201, userResponse);
    }

    [HttpPost("[action]")]
    public async Task<CustomResponse<AppUserResponse>> CreateAdmin([FromBody] AdminAppUserCreateRequest request)
    {
        var user = await _service.InsertAsync(_mapper.Map<AppUser>(request), request.Password,Role.Admin);
        var userResponse = _mapper.Map<AppUserResponse>(user);
        return CustomResponse<AppUserResponse>.Success(201, userResponse);
    }


    [HttpPut("{id}")]
    public async Task<CustomResponse<AppUserResponse>> Put(string id, [FromBody] AppUserUpdateRequest request)
    {
        var user = await _service.UpdateAsync(_mapper.Map<AppUser>(request), id);
        var userResponse = _mapper.Map<AppUserResponse>(user);
        return CustomResponse<AppUserResponse>.Success(200, userResponse);
    }

    [HttpDelete("{id}")]
    public async Task<CustomResponse<NoContent>> Delete(string id)
    {
        await _service.DeleteAsync(id);
        return CustomResponse<NoContent>.Success(204);
    }

    [HttpGet("[action]")]
    [Authorize]
    public CustomResponse<decimal> GetPointBalance()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        var user=_service.GetById(userId);
        return CustomResponse<decimal>.Success(200, user.DigitalWalletBalance);
    }
}