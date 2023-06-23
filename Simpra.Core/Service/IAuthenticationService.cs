using Simpra.Schema.TokenRR;
using Simpra.Schema.UserRR;

namespace Simpra.Core.Service
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> SignInAsync(TokenRequest request);
        Task SignOutAsync();
        Task ChangePasswordAsync(string userId, ChangePasswordRequest request);
    }
}
