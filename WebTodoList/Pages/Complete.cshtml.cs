using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTodoList.Authorization;
using WebTodoList.Data;
using WebTodoList.Models;

namespace WebTodoList.Pages
{
    public class CompleteModel : TodoModel
    {
        private readonly WebTodoList.Data.ApplicationDbContext _context;

        public CompleteModel(ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager)
        : base(context, authorizationService, userManager)
        {
            _context = context;
        }

        [BindProperty]
        public TodoItem TodoItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            TodoItem = await _context.Blogs.FirstOrDefaultAsync(
                                             m => m.ItemId == id);

            if (TodoItem == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                      User, TodoItem,
                                                      TodoOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            // Fetch Item from DB to get UserID.
            var todo = await _context
                .Blogs.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ItemId == id);

            if (todo == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, todo,
                                                     TodoOperations.Complete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            TodoItem.ItemId = todo.ItemId;

            _context.Attach(TodoItem).State = EntityState.Modified;

            if (TodoItem.Done == TodoStatus.NotCompleted)
            {
                // If task is edited after completion,
                // set it back to not completed.
                var canApprove = await AuthorizationService.AuthorizeAsync(User,
                                        TodoItem,
                                        TodoOperations.Complete);
                TodoItem.Done = TodoStatus.Completed;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(TodoItem.ItemId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TodoItemExists(int id)
        {
            return _context.Blogs.Any(e => e.ItemId == id);
        }
    }
}
