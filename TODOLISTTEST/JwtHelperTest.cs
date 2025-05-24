using System.Security.Claims;
using Xunit;
using TODOLIST.Util;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TODOLIST.Domain;

namespace TODOLISTTEST
{
    public class JwtHelperTest
    {
        /// <summary>
        /// 正常系
        /// </summary>
        [Fact]
        public void GetUserId_ReturnsUserId()
        {
            var userId = 869;
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);
            var result = principal.GetUserId();
            Assert.Equal(userId, result);
        }

        /// <summary>
        /// 異常系
        /// </summary>
        [Fact]
        public void GetUserId_ThrowsUnauthorizedAccessException()
        {

            var identity = new ClaimsIdentity(); // クレームなし
            var principal = new ClaimsPrincipal(identity);
            Assert.Throws<UnauthorizedAccessException>(() => principal.GetUserId());
        }

        /// <summary>
        /// 正常系
        /// </summary>
        [Fact]
        public void GenerateJwtToken_Returns_ValidToken_WithExpectedClaims()
        {

            var user = new User { USER_ID = 1, USER_NAME = "testuser" };
            var secretKey = "MySuperSecretKeyt43596u49teortok435654otyw45365kjtwe";
            var issuer = "testIssuer";
            var audience = "testAudience";
            var tokenString = JwtHelper.GenerateJwtToken(user, secretKey, issuer, audience);
            Assert.False(string.IsNullOrEmpty(tokenString));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var nameIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Assert.Equal(user.USER_NAME, nameClaim);
            Assert.Equal(user.USER_ID.ToString(), nameIdClaim);
        }

        /// <summary>
        /// 異常系
        /// </summary>
        [Fact]
        public void GenerateJwtToken_InvalidAudience_ThrowsException()
        {
            
            var user = new User { USER_ID = 1, USER_NAME = "testuser" };
            var secretKey = "MySuperSecretKeyt43596u49teortok435654otyw45365kjtwe";
            var issuer = "testIssuer";
            var audience = "invalidAudience";
            var token = JwtHelper.GenerateJwtToken(user, secretKey, issuer, audience);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            Assert.Throws<SecurityTokenInvalidAudienceException>(() =>
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = "expectedAudience",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);
            });
        }
    }
}

