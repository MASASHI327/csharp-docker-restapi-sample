using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TODOLIST.Service;
using Microsoft.AspNetCore.Authorization;
using TODOLIST.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using TODOLIST.Util;
using System.Diagnostics;
using TODOLIST.Domain;


namespace TODOLIST.Controllers
{
    public class AuthController(ApplicationDbContext context, IConfiguration configuration) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        [AllowAnonymous]
        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> UserRegister(UserDto userDto)
        {
            try
            {
                UserService userService = new(_context);
                User createdUser = await userService.ResisterUserAsync(userDto);
                return Ok(new { message = "ユーザー登録成功" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { message = "サーバーエラーが発生しました。" ,
                  error = ex.Message, ex.StackTrace
                });
            }
        }
       
        [AllowAnonymous]
        [HttpPost]
        [Route("api/login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.EMAIL_ADDRESS == userDto.EMAIL_ADDRESS);
            if(user == null)
            {
               return BadRequest("ユーザーが存在しません");
            }

            //パスワードの検証
            PasswordHasher<User> hasher = new();
            PasswordVerificationResult resultVeriticate = hasher.VerifyHashedPassword(user, user.PASSWORD, userDto.PASSWORD);
            if (resultVeriticate != PasswordVerificationResult.Success)
            {
                return BadRequest("パスワードが違います");
            }

            string SecretKey = _configuration["JwtSettings:SecretKey"]!;
            string issuer = _configuration["JwtSettings:Issuer"]!;
            string audience = _configuration["JwtSettings:Audience"]!;
            string token = JwtHelper.GenerateJwtToken(user, SecretKey, issuer, audience);
            return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "サーバーエラーが発生しました。",
                    error = ex.Message, ex.StackTrace
                });
            }
        }
    }
}
