using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TODOLIST.DTO;
using TODOLIST.Domain;

namespace TODOLIST.Util
{
    public static class JwtHelper
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            return userIdClaim == null ? throw new UnauthorizedAccessException("ユーザーIDのクレームが見つからない") : int.Parse(userIdClaim.Value);
        }
        

        public static string GenerateJwtToken(User user, string SecretKey, string issuer, string audience)
        {
            SymmetricSecurityKey? key = new(Encoding.UTF8.GetBytes(SecretKey));
            SigningCredentials? creds = new(key, SecurityAlgorithms.HmacSha256);

            Claim[]? claims =
             [
                new Claim(ClaimTypes.Name, user.USER_NAME),
                new Claim(ClaimTypes.NameIdentifier, user.USER_ID.ToString())
             ];

            var token = new JwtSecurityToken(
                issuer: issuer,  // 発行者
                audience: audience, // 想定される受信者
                claims: claims,
                expires: DateTime.Now.AddHours(1), // 有効期限（1時間）
                signingCredentials: creds // 署名情報
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
