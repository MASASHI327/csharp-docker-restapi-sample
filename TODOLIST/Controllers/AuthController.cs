using Microsoft.AspNetCore.Mvc;
using TODOLIST.Service;
using Microsoft.AspNetCore.Authorization;
using TODOLIST.DTO;



namespace TODOLIST.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController(ApplicationDbContext context, IConfiguration configuration) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;

      
        [HttpPost("auth/sign_up")]
        public async Task<ActionResult<AuthResponseDto?>> Signup([FromBody] UserDto userDto)
        {
            AuthResponseDto? authResponseDto = null;
            try
            {
                UserService userService = new(_context, _configuration);
                authResponseDto = await userService.ResisterUserAsync(userDto);
                return Ok(authResponseDto);
            }
            catch (Exception ex)
            {
                var errorData = new
                {
                    message = "サーバー内部でエラーが発生しました。",
                    error = ex.Message,
                    statusCode = "500"
                };
                return StatusCode(500, errorData);
            }
        }


        [HttpPost("auth/sign_in")]
        public async Task<ActionResult<AuthResponseDto>> Signin([FromBody] UserDto userDto)
        {
            try
            {
                UserService userService = new(_context, _configuration);
                AuthResponseDto authResponse = await userService.UserValidateAsync(userDto);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                var errorData = new
                {
                    message = "サーバー内部でエラーが発生しました。",
                    error = ex.Message,
                    statusCode = "500"
                };
                return StatusCode(500, errorData);
            }
        }

        [HttpPost("auth/authentication")]
        [Authorize]
        public async Task<ActionResult<AuthResponseDto>> Authentication()
        {
             UserDto userDto = new ();
            if (User.Identity?.Name != null)
            {
                // トークンから自動復元されたユーザー名
                userDto.USER_NAME = User.Identity.Name;
            }

            // フロントエンドが期待しているデータ（AuthResponseType）を返す
            var authResponseDto = new AuthResponseDto
            {
                USER_TYPE = userDto
            };
            return Ok(authResponseDto);
        }
    }
}
