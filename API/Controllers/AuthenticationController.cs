using BUS.IServices;
using BUS.Services;
using DAL.Models;
using DTO;
using DTO.Authentication;
using DTO.Customer;
using DTO.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthentication authentication;
        private readonly IConfiguration _configuration;
        private readonly ShoeStoreContext _context;
        private readonly MailServices _mailServices;
        public AuthenticationController(ICustomerServices customerServices,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ShoeStoreContext context,
            SignInManager<ApplicationUser> signInManager)
        {
            _customerServices = customerServices;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _signInManager = signInManager;
            _mailServices = new MailServices();
        }


        [HttpPost]
     
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (user.LockoutEnabled == false) {
                    return StatusCode(StatusCodes.Status302Found, new Response<string> { Status = "Error", Message = "Tài khoản của bạn đã bị khóa vui lòng liên hệ chủ cửa hàng đê được hõ trợ!", Payload = "Lỗi" });
                }

                if (user.Status == 1)
                {
                    return StatusCode(StatusCodes.Status302Found, new Response<object> { Status = "Error", Message = "Bạn chưa confirm OTP", Payload = new { Id = user.Id } });
                }
                else
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                   var authClaims = new List<Claim>
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                                new Claim(JwtRegisteredClaimNames.Email,user.Email)
                            };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var userProfiles = _context.UserProfiles.FirstOrDefault(x => x.UserId == user.Id);
                    var token = GetToken(authClaims);
                    var refreshToken = GenerateRefreshToken();

                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
                    var avatar = "";
                    if (userProfiles!=null&&userProfiles.Avatar != null) {
                        avatar = _context.Galleries.FirstOrDefault(x => x.Id == userProfiles.Avatar)?.Url;
                    }
                    var newResponse = new LoginResponse()
                    {
                        Avartar = avatar,
                        Email = user.Email,
                        FullName = userProfiles?.FullName,
                        Gender = userProfiles.Gender,
                        LastName = userProfiles.LastName,
                        Id = user.Id,
                        PhoneNumber = userProfiles.PhoneNumber,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays),
                        Role = userProfiles.RoleId,
                        ProfilesID = userProfiles.Id,
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    };
                    await _userManager.UpdateAsync(user);
                    return Ok(new Response<LoginResponse>
                    {
                        Message = "Đăng nhập thành công ",
                        Payload = newResponse,
                        Status = "Succes"
                    });
                }
            }
            return StatusCode(StatusCodes.Status302Found, new Response<List<LoginResponse>> { Status = "Error", Message = "Sai tên đăng nhập hoặc mật khẩu!" });
        }

        [HttpPost]

        [Route("login-admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userProfiles = _context.UserProfiles.FirstOrDefault(x => x.UserId == user.Id);
                if (userProfiles != null && userProfiles.RoleId == 1)
                {
                    return StatusCode(StatusCodes.Status302Found, new Response<string> { Status = "Error", Message = "Bạn không có quyền truy cập", Payload = "Lỗi" });
                
            }
                if (user.LockoutEnabled == false)
                {
                    return StatusCode(StatusCodes.Status302Found, new Response<string> { Status = "Error", Message = "Tài khoản của bạn đã bị khóa vui lòng liên hệ chủ cửa hàng đê được hõ trợ!", Payload = "Lỗi" });
                }

                if (user.Status == 1)
                {
                    return StatusCode(StatusCodes.Status302Found, new Response<object> { Status = "Error", Message = "Bạn chưa confirm OTP", Payload = new { Id = user.Id } });
                }
                else
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                                new Claim(JwtRegisteredClaimNames.Email,user.Email)
                            };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GetToken(authClaims);
                    var refreshToken = GenerateRefreshToken();

                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
                    var avatar = "";
                    if (userProfiles != null && userProfiles.Avatar != null)
                    {
                        avatar = _context.Galleries.FirstOrDefault(x => x.Id == userProfiles.Avatar)?.Url;
                    }
                    var newResponse = new LoginResponse()
                    {
                        Avartar = avatar,
                        Email = user.Email,
                        FullName = userProfiles?.FullName,
                        Gender = userProfiles.Gender,
                        LastName = userProfiles.LastName,
                        Id = user.Id,
                        PhoneNumber = userProfiles.PhoneNumber,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays),
                        Role = userProfiles.RoleId,
                        ProfilesID = userProfiles.Id,
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    };
                    await _userManager.UpdateAsync(user);
                    return Ok(new Response<LoginResponse>
                    {
                        Message = "Đăng nhập thành công ",
                        Payload = newResponse,
                        Status = "Succes"
                    });
                }
            }
            return StatusCode(StatusCodes.Status302Found, new Response<List<LoginResponse>> { Status = "Error", Message = "Sai tên đăng nhập hoặc mật khẩu!" });
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null && userExists.Status == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<int> { Status = "Error", Message = "Email này đã được đăng ký vui lòng chọn email khác" });
            }
            else if (userExists != null && userExists.Status == 1)
            {
                var result2 = await _signInManager.PasswordSignInAsync(userExists, model.Password, false, lockoutOnFailure: false);
                if (result2.Succeeded)
                {
                    return StatusCode(StatusCodes.Status302Found, new Response<int> { Status = "Error", Message = "Bạn chưa confirm OTP" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response<int> { Status = "Error", Message = "Email này đã được đăng ký vui lòng chọn email khác" });
                }
            }


            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                UserName = Guid.NewGuid().ToString(),
                Status = 1, // Chưa confirm OTP

            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var otp = new UserOtp()
                {
                    Code = RandomCode(),
                    CreatedTime = DateTime.UtcNow,
                    ExpireTime = DateTime.Now.AddMinutes(5),
                    UserId = user.Id,
                    IsUsed = false,
                };
                _context.UserOtps.Add(otp);
                await _context.SaveChangesAsync();
                //SendAsync(model.Email, "", $"OTP:{otp.Code}");
                var body = @"
                <h3>Chào bạn đến với King Shoes</h1>
                                                                    <p>Vui lòng nhập mã otp sau để xác nhận đăng ký tài khoản: </p>
                                                                    <p style=""color:black;"">{code}</p>
            ";
                body = body.Replace("{code}", otp.Code);
                _mailServices.SendMail(model.Email, "Đăng ký tài khoản King Shoes", body);
                var userProfile = new UserProfile()
                {
                    UserId = user.Id,
                    LastName = model.LastName,
                    FullName = model.FullName,
                    Email = model.Email,
                    RoleId = 1
                };
                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();
            }
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<int> { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response<RegisterResponse>
            {
                Status = "Success",
                Message = "User created successfully!",
                Payload = new RegisterResponse()
                {
                    UserId = user.Id
                }
            });
        }
        [HttpPost]
        [Route("send-otp")]
        public async Task<IActionResult> SendOTP(SendOTPDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var otp = _context.UserOtps.FirstOrDefault(x => x.UserId == model.Id && x.IsUsed == false);
            if (otp != null)
            {
                otp.IsUsed = true;
                _context.UserOtps.Update(otp);
                await _context.SaveChangesAsync();
            };
            user.Status = 0;
            otp.IsUsed = true;
            await _userManager.UpdateAsync(user);
            _context.UserOtps.Update(otp);
            await _context.SaveChangesAsync();
            return Ok(new Response<int> { Status = "Success", Message = "Xác nhận thành công" });

        }
        [HttpPost]
        [Route("confirm-otp")]
        public async Task<IActionResult> ConfirmOTP([FromBody] ConfirmOTPModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var otp = _context.UserOtps.ToList().FirstOrDefault(x => x.Code == model.Code && x.UserId == model.UserId && x.IsUsed == false);

            if (otp == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response<int> { Status = "Error", Message = "Mã OTP bạn vừa nhập không chính xác!" });
            }
            else
            {
                user.Status = 0;
                otp.IsUsed = true;
                await _userManager.UpdateAsync(user);
                _context.UserOtps.Update(otp);
                await _context.SaveChangesAsync();
                return Ok(new Response<int> { Status = "Success", Message = "Xác nhận thành công" });
            }
        }
        [HttpPost]
        [Route("resend-otp")]
        public async Task<IActionResult> ResendOTP(ConfirmOTPModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var otp = _context.UserOtps.ToList().FirstOrDefault(x => x.Code == model.Code && x.UserId == model.UserId && x.IsUsed == false);

            if (otp == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response<int> { Status = "Error", Message = "Mã OTP bạn vừa nhập không chính xác!" });
            }
            else
            {
                user.LockoutEnabled = true;
                otp.IsUsed = true;
                await _userManager.UpdateAsync(user);
                _context.UserOtps.Update(otp);
                await _context.SaveChangesAsync();
                return Ok(new Response<int> { Status = "Success", Message = "Xác nhận thành công" });
            }
        }
        private string RandomCode()
        {
            const string valid = "123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            int length = 6;
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            if (_context.UserOtps.Any(x => x.Code == res.ToString()))
            {
                RandomCode();
            }
            return res.ToString();
        }


        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> Revoke(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return Ok("Đăng xuất thành công");
        }
        [HttpGet]
        [Route("get-customer")]
        public async Task<IActionResult> GetCustomer([FromQuery] CustomerFilterDto input)
        {
            var res = _customerServices.GetAllCustomer(input);
            if(res is not null)
            {
                return Ok(new { Payload=res.Result});
            }
            else
            {
                return BadRequest();             }
        }

        [HttpPost]
        [Route("change-status")]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatus input)
        {
            var user = await _userManager.FindByIdAsync(input.Id);
            if (user == null) return BadRequest("Có lỗi xảy ra");

            user.LockoutEnabled = input.Status;
            await _userManager.UpdateAsync(user);

            return Ok("Cập nhật thành công");

        }

        [HttpPost]
        [Route("forgotpass")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response<List<LoginResponse>>()
                {
                    Message = "Not Found",
                    Status = "Không tìm thấy tài khoản"
                });
            }
            var newres = new
            {
                Id = user.Id,
            };
            var otp = new UserOtp()
            {
                IsUsed = false,
                Code = RandomCode(),
                ExpireTime = DateTime.Now.AddMinutes(5),
                UserId = user.Id,
                CreatedTime = DateTime.Now,
            };
            _context.UserOtps.Add(otp);
            await _context.SaveChangesAsync();
            var body = @"
                <h3>Bạn đã sử dụng chức năng quên mật khẩu</h1>
                                                                    <p>Vui lòng nhập mã otp sau để xác nhận: </p>
                                                                    <p style=""color:black;"">{code}</p>
            ";
            body = body.Replace("{code}", otp.Code);
            _mailServices.SendMail(email, "Lấy lại mật khẩu tài khoản King Shoes", body);
            return Ok(new Response<object>()
            {
                Message = "",
                Status = "Succes",
                Payload = newres
            });
        }
        [HttpPost]
        [Route("changepass")]
        public async Task<IActionResult> ChangePassword([FromBody]ResetPasswordDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response<List<LoginResponse>>()
                {
                    Message = "Not Found",
                    Status = "Không tìm thấy tài khoản"
                });
            }
            if (user != null && !await  _userManager.CheckPasswordAsync(user,model.OldPassword))
            {
                return StatusCode(StatusCodes.Status502BadGateway, new Response<List<LoginResponse>>()
                {
                    Message = "Not Found",
                    Status = "Mật khẩu cũ không chính xác"
                });
            }
            var refreshToken = GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
            var authClaims = new List<Claim>
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                                new Claim(JwtRegisteredClaimNames.Email,user.Email)
                            };

            var token = GetToken(authClaims);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
            var token2 = await _userManager.GeneratePasswordResetTokenAsync(user);
            //await _userManager.ChangePasswordAsync(user, token2, model.Password);
            var result = await _userManager.ChangePasswordAsync(user,model.OldPassword,model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<List<LoginResponse>>()
                {
                    Message = "Not Found",
                    Status = "Có lỗi xảy ra!"
                });
            }
            else
            {
                var userProfiles = _context.UserProfiles.FirstOrDefault(x => x.UserId == user.Id);
                var avatar = "";
                if (userProfiles != null && userProfiles.Avatar != null)
                {
                    avatar = _context.Galleries.FirstOrDefault(x => x.Id == userProfiles.Avatar)?.Url;
                }
                var newResponse = new LoginResponse()
                {
                    Avartar = avatar,
                    Email = user.Email,
                    FullName = userProfiles?.FullName,
                    Gender = userProfiles.Gender,
                    LastName = userProfiles.LastName,
                    Id = user.Id,
                    PhoneNumber = userProfiles.PhoneNumber,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays),
                    Role = userProfiles.RoleId,
                    ProfilesID = userProfiles.Id,
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                };
                return Ok(new Response<object>()
                {
                    Message = "",
                    Status = "Succes",
                    Payload = newResponse
                });
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Đã xảy ra lỗi");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            var x = principal.Claims;
            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken);
            var tokenS = jsonToken as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == "email").Value;
            if (jti == null)
            {
                return BadRequest("Đã xảy ra lỗi");
            }

            //string username = principal.Identity.E;

            var user = await _userManager.FindByEmailAsync(jti);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Đã xảy ra lỗi");
            }
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            var newAccessToken = GetToken(tokenS.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _userManager.UpdateAsync(user);
            var userProfiles = _context.UserProfiles.FirstOrDefault(x => x.UserId == user.Id);
            var avatar = "";
            if (userProfiles != null && userProfiles.Avatar != null)
            {
                avatar = _context.Galleries.FirstOrDefault(x => x.Id == userProfiles.Avatar)?.Url;
            }
            var newResponse = new LoginResponse()
            {
                Avartar = avatar,
                Email = user.Email,
                FullName = userProfiles?.FullName,
                Gender = userProfiles.Gender,
                LastName = userProfiles.LastName,
                Id = user.Id,
                PhoneNumber = userProfiles.PhoneNumber,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays),
                Role = userProfiles.RoleId,
                ProfilesID = userProfiles.Id,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            };
            await _userManager.UpdateAsync(user);
            return Ok(new Response<LoginResponse>
            {
                Message = "",
                Payload = newResponse,
                Status = "Succes"
            });
        }
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);



            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
