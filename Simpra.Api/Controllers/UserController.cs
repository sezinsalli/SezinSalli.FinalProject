using Autofac.Core;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Core.Jwt;
using Simpra.Core.Role;
using Simpra.Core.Service;
using Simpra.Schema.ProductRR;
using Simpra.Schema.UserRR;
using Simpra.Service.Response;

namespace Simpra.Api.Controllers
{
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
        public IActionResult GetAll()
        {
            var users = _service.GetAll();
            var usersResponse = _mapper.Map<List<AppUserResponse>>(users);
            return CreateActionResult(CustomResponse<List<AppUserResponse>>.Success(200,usersResponse));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult GetById([FromRoute] string id)
        {
            var user = _service.GetById(id);
            var userResponse = _mapper.Map<AppUserResponse>(user);
            return CreateActionResult(CustomResponse<AppUserResponse>.Success(200, userResponse));
        }

        [HttpGet("[action]")]
        [Authorize]
        public IActionResult GetUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;
            var user = _service.GetById(userId);
            var userResponse = _mapper.Map<AppUserResponse>(user);
            return CreateActionResult(CustomResponse<AppUserResponse>.Success(200, userResponse));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser([FromBody] AppUserCreateRequest request)
        {
            var user = await _service.InsertAsync(_mapper.Map<AppUser>(request), request.Password, Role.User);
            var userResponse = _mapper.Map<AppUserResponse>(user);
            return CreateActionResult(CustomResponse<AppUserResponse>.Success(201, userResponse));
        }

        [HttpPost("[action]")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminAppUserCreateRequest request)
        {
            var user = await _service.InsertAsync(_mapper.Map<AppUser>(request), request.Password, Role.Admin);
            var userResponse = _mapper.Map<AppUserResponse>(user);
            return CreateActionResult(CustomResponse<AppUserResponse>.Success(201, userResponse));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] AppUserUpdateRequest request)
        {
            var user = await _service.UpdateAsync(_mapper.Map<AppUser>(request), id);
            var userResponse = _mapper.Map<AppUserResponse>(user);
            return CreateActionResult(CustomResponse<AppUserResponse>.Success(200, userResponse));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _service.DeleteAsync(id);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

        [HttpGet("[action]")]
        [Authorize]
        public IActionResult GetPointBalance()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;
            var user = _service.GetById(userId);
            return CreateActionResult(CustomResponse<decimal>.Success(200, user.DigitalWalletBalance));
        }
    }
}
