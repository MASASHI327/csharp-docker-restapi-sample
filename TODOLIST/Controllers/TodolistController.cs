using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TODOLIST.DTO;
using TODOLIST.Domain;
using TODOLIST.Service;
using TODOLIST.Util;

namespace TODOLIST.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class TodoListController : ControllerBase
    {
        private readonly TodolistService _todoListService;

        public TodoListController(TodolistService todolistService)
        {
            _todoListService = todolistService;
        }

        [HttpGet("todolist")]
        public async Task<ActionResult<List<TodoDto>>> GetTodoListByUserIdAsync()
        {
            try
            {
               int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
               List<TodoDto> todolistDto = await _todoListService.GetTodoListByUserIdAsync(userId);
               return Ok(todolistDto);
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

        [HttpGet("todo/{todoId}")]
        public async Task<ActionResult<TodoDto>> GetTodoByTodoId(int todoId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                TodoDto todoDto = await _todoListService.GetTodoByTodoIdAndUserIdAsync(userId, todoId);
                return Ok(todoDto);
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

        [HttpPost("todo")]
        public async Task<IActionResult> CreateTodo([FromBody] TodoDto todoDto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _todoListService.CreateTodoAsync(userId, todoDto);
                return Ok(todoDto);
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

        [HttpPatch("todo/{todolistId}")]
        public async Task<ActionResult<TodoDto>> UpdateTodo([FromBody] TodoDto todoDto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _todoListService.UpdateTodoAsync(userId, todoDto);
                return Ok(todoDto);
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

        [HttpDelete("todo/{todolistId}")]
        public async Task<IActionResult> DeleteTodo(int todolistId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _todoListService.DeleteTodoAsync(userId, todolistId);
                return Ok();
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

    }
}
