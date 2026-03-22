using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODOLIST.DTO;
using TODOLIST.Domain;
using Azure.Core;
using TODOLIST.Util;



namespace TODOLIST.Service
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<UserEntity> _hasher = new();
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> ResisterUserAsync(UserDto userDto)
        {
            // 重複チェック（メールアドレスが既に使われている場合）
            UserEntity? existingUser = await _context.Users.FirstOrDefaultAsync(u => u.EMAIL_ADDRESS == userDto.EMAIL_ADDRESS);
            if (existingUser != null)
            {
                var authResponseNull = new AuthResponseDto
                {
                    USER_TYPE = userDto,
                    CODE = 400
                };
                return authResponseNull;
            }

            UserEntity? user = new()
            {
                USER_NAME = userDto.USER_NAME,
                EMAIL_ADDRESS = userDto.EMAIL_ADDRESS
            };

            // パスワードをハッシュ化して保存
            user.PASSWORD = _hasher.HashPassword
                (user, userDto.PASSWORD);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            userDto.USER_ID = user.USER_ID;
            string accessToken = JwtHelper.GenerateJwtToken(user, _configuration);
            var authResponse = new AuthResponseDto
            {
                USER_TYPE = userDto,
                ACCESSTOKEN = accessToken,
                CODE = 200
            };

            return authResponse;
        }

        public async Task<AuthResponseDto> UserValidateAsync(UserDto userDto)
        {
            bool rezult = false;
            string message = string.Empty;
            //メールアドレス認証
            UserEntity? user = await _context.Users.FirstOrDefaultAsync(u => u.EMAIL_ADDRESS == userDto.EMAIL_ADDRESS);
            if (user == null)
            {
                var authResponseNull = new AuthResponseDto
                {
                    USER_TYPE = userDto,
                    CODE = 401
                };
                return authResponseNull;
            }

            //パスワードの検証
            PasswordHasher<UserEntity> hasher = new();
            PasswordVerificationResult resultVeriticate = hasher.VerifyHashedPassword(user, user.PASSWORD, userDto.PASSWORD);
            if (resultVeriticate != PasswordVerificationResult.Success)
            {
                var authResponseNull = new AuthResponseDto
                {
                    USER_TYPE = userDto,
                    CODE = 401
                };
                return authResponseNull;
            }

            string accessToken = JwtHelper.GenerateJwtToken(user, _configuration);
            var authResponse = new AuthResponseDto
            {
                USER_TYPE = userDto,
                ACCESSTOKEN = accessToken,
                CODE = 200
            };
            return authResponse;
        }
    }
}
