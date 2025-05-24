using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polly;
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

        public async Task<List<Todolist>?> GetTodolistsByUserIdAsync(int userId)
        {
            List<Todolist>? todolist = await _repository.GetTodolistsByUserIdAsync(userId);
            return todolist;
        }

        public async Task<List<Todolist>?> GetTodolistByTodolistIdAsync(int todolistId, int userId)
        {
            List<Todolist>? todolist = await _repository.GetTodolistByTodolistIdAndUserIdAsync(todolistId, userId);
            return todolist;
        }

        public async Task<bool> CreateTodolistsAsync(List<TodolistCreatedDto> todolistsCreatedDto)
        {
            bool result = false;
            if (todolistsCreatedDto.Count > 0)
            {
                List<Todolist> addTodolists = todolistsCreatedDto.Select(dto => new Todolist
                {
                    USER_ID = dto.USER_ID,
                    TITLE = dto.TITLE,
                    CONTENT = dto.CONTENT,
                    CREATED_DATE = DateTime.Now
                }).ToList();
                result = await _repository.AddTodlistsAsync(addTodolists);
            }
            return result;
        }

        public async Task<bool> UpdateTodolistsAsync(List<Todolist> todolists)
        {
            bool result = false;
            return result = await _repository.UpdateTodlistsAsync(todolists);
        }

        public async Task<bool> DeleteTodolistsAsync(List<TodolistDeletedDto> TodolistsDeletedDto)
        {
            bool result = false;
            return result = await _repository.DeleteTodolistsAsync(TodolistsDeletedDto);
        }
    }
}
