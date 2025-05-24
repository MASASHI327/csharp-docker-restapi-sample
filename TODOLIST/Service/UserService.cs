using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODOLIST.DTO;
using TODOLIST.Domain;

namespace TODOLIST.Service
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _hasher = new();

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> ResisterUserAsync(UserDto userDto)
        {

            if (string.IsNullOrEmpty(userDto.USER_NAME) == true ||
                string.IsNullOrEmpty(userDto.EMAIL_ADDRESS) == true ||
                string.IsNullOrEmpty(userDto.PASSWORD) == true)  
            {
                throw new Exception("ユーザーデータが存在しない");
            }

            // 重複チェック（メールアドレスが既に使われている場合）
            User? existingUser = await _context.Users.FirstOrDefaultAsync(u => u.EMAIL_ADDRESS == userDto.EMAIL_ADDRESS);
            if (existingUser != null)
            {
                throw new Exception("同じメールアドレスを使用しているユーザーが存在する"); // エラーメッセージを直接返す
            }

            User? user = new()
            {
                USER_NAME = userDto.USER_NAME,
                EMAIL_ADDRESS = userDto.EMAIL_ADDRESS
            };

            // パスワードをハッシュ化して保存
            user.PASSWORD = _hasher.HashPassword
                (user, userDto.PASSWORD);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
