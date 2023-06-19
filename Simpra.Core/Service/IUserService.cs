
using Simpra.Core.Entity;
using Simpra.Schema.UserRR;
using System.Security.Claims;

namespace Simpra.Core.Service;

public interface IUserService
{
    Task<List<AppUser>> GetAllAsycn();
    Task<AppUser> GetByIdAsync(string id);
    Task<AppUser> InsertAsync(AppUser request, string password);
    Task UpdateAsync(AppUser request, string id);
    Task Delete(string id);
    Task<AppUser> GetUserAsync(ClaimsPrincipal User);
    string GetUserId(ClaimsPrincipal User);

}
