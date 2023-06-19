using Simpra.Schema.TokenRR;
using Simpra.Schema.UserRR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Service
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> SignIn(TokenRequest request);
        Task SignOut();
        Task ChangePassword(ClaimsPrincipal User, ChangePasswordRequest request);
    }
}
