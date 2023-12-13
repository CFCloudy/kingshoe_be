using BUS.IServices;
using DAL.Repositories.Interfaces;
using DTO.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using DTO.Utils;
using System.Security.Cryptography;
using System.Net.Mail;
using DAL.Repositories.Implements;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace BUS.Services
{
    public class Authentication 
    {
        public IGenericRepository<UserProfile> _repoUserProfile;
        public IGenericRepository<UserOtp> _repoOTp;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public Authentication(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager)
        {
            _repoUserProfile=new GenericRepository<UserProfile>();
            _repoOTp = new GenericRepository<UserOtp>();
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }
        //public async Task<Response<LoginResponse>> Login(LoginDto model)
        //{
        //    try
        //    {
        //        Response<LoginResponse> respon;
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //        {
        //            if (user.Status == 1)
        //            {
        //                return respon = new Response<LoginResponse>
        //                {
        //                    Message = "Bạn chưa confirm OTP",
        //                    Payload = new LoginResponse(),
        //                    Status = "Error",
        //                    TotalCount = 0
        //                };
        //            }
        //            else
        //            {
        //                var userRoles = await _userManager.GetRolesAsync(user);

        //                var authClaims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Email, user.Email),
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            };
        //                foreach (var userRole in userRoles)
        //                {
        //                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //                }

        //                var userProfiles = _repoUserProfile.GetAllDataQuery().FirstOrDefault(x => x.UserId == user.Id);
        //                var token = GetToken(authClaims);
        //                var refreshToken = GenerateRefreshToken();

        //                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

        //                user.RefreshToken = refreshToken;
        //                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

        //                var newResponse = new LoginResponse()
        //                {
        //                    Avartar = userProfiles?.Avatar,
        //                    Email = user.Email,
        //                    FullName = userProfiles?.FullName,
        //                    Gender = userProfiles.Gender,
        //                    LastName = userProfiles.LastName,
        //                    Id = user.Id,
        //                    PhoneNumber = userProfiles.PhoneNumber,
        //                    RefreshToken = refreshToken,
        //                    RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays),
        //                    Role = userProfiles.RoleId,
        //                    ProfilesID = userProfiles.Id,
        //                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
        //                };
        //                await _userManager.UpdateAsync(user);
        //                return respon=new Response<LoginResponse>
        //                {
        //                    Message = "Đăng nhập thành công ",
        //                    Payload = newResponse,
        //                    Status = "Succes"
        //                };
        //            }
        //        }
        //        return new Response<LoginResponse> { Status = "Error", Message = "Sai tên đăng nhập hoặc mật khẩu!" };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<object> ChangePassword(ResetPasswordDTO model)
        //{
        //    var user = await _userManager.FindByIdAsync(model.Id);
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    var refreshToken = GenerateRefreshToken();

        //    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

        //    user.RefreshToken = refreshToken;
        //    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    await _userManager.ResetPasswordAsync(user, token, model.Password);
        //    var result = await _userManager.UpdateAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        return new { error = "Co loi xay ra" };
        //    }
        //    else
        //    {
        //        var userProfiles = _repoUserProfile.GetAllDataQuery().FirstOrDefault(x => x.UserId == user.Id);
        //        var avatar = "";
        //        if (userProfiles != null && userProfiles.Avatar != null)
        //        {
        //            avatar = _context.Galleries.FirstOrDefault(x => x.Id == userProfiles.Avatar)?.Url;
        //        }
        //        var newResponse = new LoginResponse()
        //        {
        //            Avartar = userProfiles?.Avatar,
        //            Email = user.Email,
        //            FullName = userProfiles?.FullName,
        //            Gender = userProfiles.Gender,
        //            LastName = userProfiles.LastName,
        //            Id = user.Id,
        //            PhoneNumber = userProfiles.PhoneNumber,
        //            RefreshToken = refreshToken,
        //            RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays),
        //            Role = userProfiles.RoleId
        //        };
        //        return newResponse;
        //    }
        //}

        //public async Task<object> ForgotPassword(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    var newres = new
        //    {
        //        Id = user.Id,
        //    };
        //    var otp = new UserOtp()
        //    {
        //        IsUsed = false,
        //        Code = RandomCode(),
        //        ExpireTime = DateTime.Now.AddMinutes(5),
        //        UserId = user.Id,
        //        CreatedTime = DateTime.Now,
        //    };
        //    _repoOTp.AddDataCommand(otp);
        //    SendAsync(email, "", $"OTP:{otp.Code}");
        //    return newres;
        //}

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            throw new NotImplementedException();
        }

        
        public Task<RegisterResponse> Register(RegisterDto model)
        {
            throw new NotImplementedException();
        }

        //public async Task<object> ResetPassword(ResetPasswordDTO model)
        //{
        //    var user = await _userManager.FindByIdAsync(model.Id);
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    var refreshToken = GenerateRefreshToken();

        //    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

        //    user.RefreshToken = refreshToken;
        //    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    await _userManager.ResetPasswordAsync(user, token, model.Password);
        //    var result = await _userManager.UpdateAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        return new { error = "Co loi xay ra" };
        //    }
        //    else
        //    {
        //        var userProfiles = _repoUserProfile.GetAllDataQuery().FirstOrDefault(x => x.UserId == user.Id);

        //        var newResponse = new LoginResponse()
        //        {
        //            Avartar = userProfiles?.Avatar,
        //            Email = user.Email,
        //            FullName = userProfiles?.FullName,
        //            Gender = userProfiles.Gender,
        //            LastName = userProfiles.LastName,
        //            Id = user.Id,
        //            PhoneNumber = userProfiles.PhoneNumber,
        //            RefreshToken = refreshToken,
        //            RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays),
        //            Role = userProfiles.RoleId
        //        };
        //        return newResponse;
        //    }
        //}

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
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
            if (_repoOTp.GetAllDataQuery().Any(x => x.Code == res.ToString()))
            {
                RandomCode();
            }
            return res.ToString();
        }

        private bool SendAsync(string target, string subject, string body)
        {
            try
            {
                string EmailAddress = "clone9291@gmail.com";
                string Password = "";
                MailMessage message = new MailMessage(EmailAddress, target.Replace(" ", ""));
                message.Subject = subject;

                message.Body = body;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = false;
                string server;
                int port;
                if (target.Contains("@gmail"))
                {
                    server = "smtp.gmail.com";
                    port = 587;
                }
                else if (target.Contains("@fpt"))
                {
                    server = "omail.edu.fpt.vn";
                    port = 587;
                }
                else
                {
                    server = "smtp.live.com";
                    port = 587;
                }
                //using (SmtpClient smtp = new SmtpClient(server, port))
                using (SmtpClient smtp = new SmtpClient("sandbox.smtp.mailtrap.io", 2525))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("90fce4837ea2ba", "dbd3a730ad8562");
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
