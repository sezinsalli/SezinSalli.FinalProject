using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
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
    public async Task<CustomResponse<List<AppUserResponse>>> GetAll()
    {
        var users = await _service.GetAllAsycn();
        var usersResponse = _mapper.Map<List<AppUserResponse>>(users);
        return CustomResponse<List<AppUserResponse>>.Success(200, usersResponse);
    }

    [HttpGet("{id}")]
    public async Task<CustomResponse<AppUserResponse>> GetById(string id)
    {
        var user = await _service.GetByIdAsync(id);
        var userResponse = _mapper.Map<AppUserResponse>(user);
        return CustomResponse<AppUserResponse>.Success(200, userResponse);
    }

    [HttpGet("GetUser")]
    public async Task<CustomResponse<AppUserResponse>> GetUser()
    {
        var user = await _service.GetUserAsync(HttpContext.User);
        var userResponse = _mapper.Map<AppUserResponse>(user);
        return CustomResponse<AppUserResponse>.Success(200, userResponse);
    }

    [HttpGet("GetUserId")]
    public CustomResponse<string> GetUserId()
    {
        var userId = _service.GetUserId(HttpContext.User);
        return CustomResponse<string>.Success(200, userId);
    }

    [HttpPost]
    public async Task<CustomResponse<AppUserResponse>> Post([FromBody] AppUserCreateRequest request)
    {
        var user = await _service.InsertAsync(_mapper.Map<AppUser>(request), request.Password);
        var userResponse = _mapper.Map<AppUserResponse>(user);
        return CustomResponse<AppUserResponse>.Success(201, userResponse);
    }

    [HttpPut("{id}")]
    public async Task<CustomResponse<NoContent>> Put(string id, [FromBody] AppUserUpdateRequest request)
    {
        await _service.UpdateAsync(_mapper.Map<AppUser>(request), id);
        return CustomResponse<NoContent>.Success(204);
    }

    [HttpDelete("{id}")]
    public async Task<CustomResponse<NoContent>> Delete(string id)
    {
        await _service.Delete(id);
        return CustomResponse<NoContent>.Success(204);
    }
}