using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TODOLIST.DTO;
using TODOLIST.Domain;
using TODOLIST.Service;
using TODOLIST.Util;

namespace TODOLIST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodolistController : ControllerBase
    {
        private readonly TodolistService _todolistService;

        public TodolistController(TodolistService todolistService)
        {
            _todolistService = todolistService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Todolist>?>> GetTodolistsByUserIdAsync()
        {
            try
            {
                //認証されたユーザーIDを取得
                int? userId = User?.GetUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }
                List<Todolist>? todolist = await _todolistService.GetTodolistsByUserIdAsync(userId.Value);
                return Ok(todolist);
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

        [Authorize]
        [HttpGet("GetByTodolistId")]
        public async Task<ActionResult<List<Todolist>?>> GetTodolistByTodolistId([FromQuery] int todolistId)
        {
            try
            {
                //認証されたユーザーIDを取得
                int? userId = User?.GetUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }
                List<Todolist>? todolist = await _todolistService.GetTodolistByTodolistIdAsync(todolistId, userId.Value);
                return Ok(todolist);
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTodolist([FromBody] List<TodolistCreatedDto> todolistsCretedDto)
        {
            try
            {
                //認証されたユーザーIDを取得
                int? userId = User?.GetUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }
                todolistsCretedDto = ValidateHelper.SetUserId(todolistsCretedDto, userId.Value);
                bool result = await _todolistService.CreateTodolistsAsync(todolistsCretedDto);
                if (result == true)
                {
                    return Ok(todolistsCretedDto);
                }
                return BadRequest(result);
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

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateTodolist([FromBody] List<Todolist> todolists)
        {
            try
            {
                //認証されたユーザーIDを取得
                int? userId = User?.GetUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }
                todolists = ValidateHelper.SetUserId(todolists, userId.Value);
                bool result = await _todolistService.UpdateTodolistsAsync(todolists);
                if (result == true)
                {
                    return Ok(todolists);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "サーバーエラーが発生しました。",
                    error = ex.Message,
                    ex.StackTrace
                });
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteTodolist([FromBody] List<TodolistDeletedDto> TodolistsDeletedDto)
        {
            try
            {
                //認証されたユーザーIDを取得
                int? userId = User?.GetUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }
                TodolistsDeletedDto = ValidateHelper.SetUserId(TodolistsDeletedDto, userId.Value);
                bool result = await _todolistService.DeleteTodolistsAsync(TodolistsDeletedDto);
                if (result == true)
                {
                    return Ok(TodolistsDeletedDto);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "サーバーエラーが発生しました。",
                    error = ex.Message,
                    ex.StackTrace
                });
            }
        }

    }
}
