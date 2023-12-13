using DTO;
using DTO.Authentication;
using DTO.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BUS.IServices
{
    public interface IAuthentication
    {
        public Task<Response<LoginResponse>> Login(LoginDto model);
        public Task<RegisterResponse> Register(RegisterDto model);
        public Task<object> ForgotPassword(string email);
        public Task<object> ResetPassword(ResetPasswordDTO model);
        public Task<object> ChangePassword(ResetPasswordDTO model);
        public JwtSecurityToken GetToken(List<Claim> authClaims);

    }
}
