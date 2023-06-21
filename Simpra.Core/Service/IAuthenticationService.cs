using Simpra.Schema.TokenRR;
using Simpra.Schema.UserRR;

namespace Simpra.Core.Service
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> SignIn(TokenRequest request);
        Task SignOut();
        Task ChangePassword(string userId, ChangePasswordRequest request);
    }
}
