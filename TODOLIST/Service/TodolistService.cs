using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Security.Claims;
using TODOLIST.Domain;
using TODOLIST.DTO;
using TODOLIST.Repository;

namespace TODOLIST.Service
{
    public class TodolistService
    {
        private readonly TodolistRepository _repository;

        public TodolistService(TodolistRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TodoDto>> GetTodoListByUserIdAsync(int userId)
        {
            List<TodoEntity> todoEntity = await _repository.GetTodoListByUserIdAsync(userId);
            List<TodoDto> todoListDto = todoEntity.Select(todoEntity => new TodoDto
            {
                TODO_ID = todoEntity.TODO_ID,
                TITLE = todoEntity.TITLE ?? "",
                CONTENT = todoEntity.CONTENT ?? "",
                CODE = 200
            }).ToList();
            return todoListDto;
        }

        public async Task<TodoDto> GetTodoByTodoIdAndUserIdAsync(int userId, int todoId)
        {
            TodoEntity? todoEntity = await _repository.GetTodoByTodoIdAndUserIdAsync(userId, todoId);
            var todoDto = new TodoDto
            {
                TODO_ID = todoEntity?.TODO_ID ?? 0,
                TITLE = todoEntity?.TITLE ?? "",
                CONTENT = todoEntity?.CONTENT ?? "",
                CODE = 200
            };
            return todoDto;
        }

        public async Task CreateTodoAsync(int userId, TodoDto todoDto)
        {
            bool result = false;
            TodoEntity newTodo = new TodoEntity();
            newTodo.USER_ID = userId;
            newTodo.TITLE = todoDto.TITLE;
            newTodo.CONTENT = todoDto.CONTENT;
            newTodo.CREATED_DATE = DateTime.Now;
            result = await _repository.AddTodoAsync(newTodo);
            if (result == false)
            {
                throw new Exception("レコードが保存できない");
            }
        }

        public async Task UpdateTodoAsync(int userId,TodoDto todoDto)
        {
            bool result = false;

            TodoEntity updateTodo = new TodoEntity();
            updateTodo.TODO_ID = todoDto.TODO_ID;
            updateTodo.USER_ID = userId;
            updateTodo.TITLE = todoDto.TITLE;
            updateTodo.CONTENT = todoDto.CONTENT;
            updateTodo.CREATED_DATE = DateTime.Now;
            result = await _repository.UpdateTodoAsync(updateTodo);

            if (result == false)
            {
                throw new Exception("レコードが保存できない");
            }
        }

        public async Task<bool> DeleteTodoAsync(int userId, int todolistId)
        {
            return  await _repository.DeleteTodoAsync(userId, todolistId);
        }
    }
}
