using BTBackendOnline2.Configurations;
using DeltaBrainJSC.DB;
using DeltaBrainsJSCAppBE.DTOs;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using DeltaBrainsJSCAppBE.Handle;
using DeltaBrainsJSCAppBE.Models;
using DeltaBrainsJSCAppBE.Services.Interfaces;
using DotNetEnv;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace DeltaBrainsJSCAppBE.Services.Implements
{
    public class AuthService : IAuth
    {
        private readonly DBContext _context;
        private readonly TokenRequiment jwt;

        public AuthService(DBContext context)
        {
            _context = context;
            jwt = new()
            {
                SecretKey = Env.GetString("JWT_SECRET_KEY"),
                Issuer = Env.GetString("JWT_ISSUER"),
                Audience = Env.GetString("JWT_AUDIENCE"),
                Subject = Env.GetString("JWT_SUBJECT")
            };

        }
        public ApiResponse<LoginRes> Login(LoginReq request)
        {
            try
            {
                var user = ValidateUser(request.Username, request.Password);

                if (user == null)
                {
                    return ApiResponse<LoginRes>.Fail("Tên đăng nhập hoặc mật khẩu không đúng");
                }

                if(user.Id == 0)
                {
                    return ApiResponse<LoginRes>.Fail("Lỗi xác thực người dùng");
                }

                var token = GenerateJwtToken(user);

                if (token == null)
                {
                    return ApiResponse<LoginRes>.Fail("Lỗi tạo token");
                }

                return ApiResponse<LoginRes>.Success(new LoginRes
                {
                    SuccsessFull = true,
                    Token = token,
                    Expiration = DateTime.UtcNow.AddMinutes(75),
                    Error = null
                });
            }
            catch
            {
                return ApiResponse<LoginRes>.Error();
            }
            

        }

        private string? GenerateJwtToken(User user, int accessExpire = 75)
        {
            try
            {
                var role = _context.Roles.FirstOrDefault(a => a.Id == user.RoleId);

                if (role == null) return null;


                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, role.Name),
                    new Claim("Id", user.Id.ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenAccess = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(accessExpire),
                    signingCredentials: signIn
                );

                return new JwtSecurityTokenHandler().WriteToken(tokenAccess);
            }
            catch
            {
                return null;
            }   

        }

        private User? ValidateUser(string username, string password)
        {
            try
            {
                password = CompareSHA256.ToSHA256(password);

                var user = _context.Users.FirstOrDefault(u =>
                    u.Email.Equals(username) && u.Password.Trim().Equals(password.Trim()));

                return user ?? null;
            }
            catch
            {
                return new User
                {
                    Id = 0
                };
            }
        }
    }
}
