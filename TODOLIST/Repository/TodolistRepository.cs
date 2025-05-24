
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TODOLIST.Domain;
using TODOLIST.DTO;

namespace TODOLIST.Repository
{
    public class TodolistRepository
    {
        private readonly ApplicationDbContext _context;

        public TodolistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Todolist>> GetTodolistsByUserIdAsync(int userId)
        {
            List<Todolist>? todolist = await _context.Todolists.Where(t => t.USER_ID == userId).ToListAsync();
            return todolist;
        }

        public async Task<List<Todolist>> GetTodolistByTodolistIdAndUserIdAsync(int todolistId, int userId)
        {
            List<Todolist>? todolist = await _context.Todolists.Where(t => t.TODOLIST_ID == todolistId && t.USER_ID == userId).ToListAsync();
            return todolist;
        }

        public async Task<bool> AddTodlistsAsync(List<Todolist> addTodolists)
        {
            bool result = false;
            await _context.Todolists.AddRangeAsync(addTodolists);
            int savedResult = await _context.SaveChangesAsync();
            if (savedResult > 0)
            {
                result = true;
            }
            return result;
        }

        public async Task<bool> UpdateTodlistsAsync(List<Todolist> todolists)
        {
            bool result = false;
            List<(int todolistId, int userId)> todolistIds = todolists.Select(t => (t.TODOLIST_ID, t.USER_ID)).ToList();
            List<Todolist> existingTodolists = _context.Todolists.AsEnumerable()
                                   .Where(t => todolistIds.Contains(( t.TODOLIST_ID, t.USER_ID ))).ToList();

            int updatedRowCount = 0;
            foreach (Todolist todolist in todolists)
            {
                Todolist? selectedTodolist = existingTodolists
                                     .FirstOrDefault(t => t.TODOLIST_ID == todolist.TODOLIST_ID && t.USER_ID == todolist.USER_ID);
                if (selectedTodolist != null)
                {
                    // TODOリストの内容を更新
                    selectedTodolist.USER_ID = todolist.USER_ID;
                    selectedTodolist.TITLE = todolist.TITLE;
                    selectedTodolist.CONTENT = todolist.CONTENT;
                    selectedTodolist.UPDATED_DATE = DateTime.Now;
                    updatedRowCount++;
                    if (existingTodolists.Count == updatedRowCount)
                    {
                        result = true;
                    }
                }
            }

            if (result == true)
            {
                int savedResult = await _context.SaveChangesAsync();
                if (savedResult > 0)
                {
                    result = true;
                }
            }
            return result;
        }

        public async Task<bool> DeleteTodolistsAsync(List<TodolistDeletedDto> TodolistsDeletedDto)
        {
            bool result = false;
            List<(int todolist, int userId)> deletedIds = TodolistsDeletedDto
                                                          .Select(dto => (dto.TODOLIST_ID, dto.USER_ID)).ToList();
            List<Todolist> deletedTodolists = _context.Todolists.AsEnumerable()
                                              .Where(t => deletedIds.Contains(( t.TODOLIST_ID, t.USER_ID ))).ToList();

            _context.Todolists.RemoveRange(deletedTodolists);
            int deletedResult = await _context.SaveChangesAsync();
            if (deletedResult > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
