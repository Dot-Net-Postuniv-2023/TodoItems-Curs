using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.DTOs;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems
                                                    .Include(t => t.TodoSubItems)
                                                    .FirstOrDefaultAsync(t => t.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, [FromBody] TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            TodoItem todoItem = new TodoItem
            {
                Id = todoItemDTO.Id.Value,
                Name = todoItemDTO.Name,
                Description = todoItemDTO.Description,
                TodoSubItems = todoItemDTO.TodoSubItems.Select(subItem => new TodoSubItem
                {
                    Id = subItem.Id == null ? 0 : subItem.Id.Value,
                    Name = subItem.Name,
                    Description = subItem.Description,
                    DateCompleted = subItem.DateCompleted,
                    Priority = subItem.Priority,
                    TodoItemId = todoItemDTO.Id.Value
                }).ToList()
            };

            for (int i = 0; i < todoItem.TodoSubItems.Count; i++)
            {
                if (todoItem.TodoSubItems[i].Id == 0)
                {
                    todoItem.TodoSubItems[i].DateAdded = DateTime.Now;
                    _context.TodoSubItems.Add(todoItem.TodoSubItems[i]);
                }
                else
                {
                    _context.Entry(todoItem.TodoSubItems[i]).State = EntityState.Modified;
                }
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem([FromBody] TodoItemDTO todoItemDTO)
        {
            TodoItem todoItem = new TodoItem
            {
                Name = todoItemDTO.Name,
                Description = todoItemDTO.Description,
                TodoSubItems = todoItemDTO.TodoSubItems.Select(subItem => new TodoSubItem
                {
                    Name = subItem.Name,
                    Description = subItem.Description,
                    DateAdded = DateTime.Now,
                    DateCompleted = subItem.DateCompleted,
                    Priority = subItem.Priority
                }).ToList()
            };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
