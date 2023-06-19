
using Simpra.Core.Entity;
using Simpra.Schema.UserRR;
using System.Security.Claims;

namespace Simpra.Core.Service;

public interface IUserService
{
    List<AppUser> GetAll();
    AppUser GetById(string id);
    Task<AppUser> InsertAsync(AppUser request, string password);
    Task<AppUser> UpdateAsync(AppUser request, string id);
    Task DeleteAsync(string id);
    Task<AppUser> GetUserAsync(ClaimsPrincipal User);
    string GetUserId(ClaimsPrincipal User);

}
