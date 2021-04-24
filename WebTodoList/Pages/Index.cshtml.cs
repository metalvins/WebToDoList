using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTodoList.Data;
using WebTodoList.Models;

namespace WebTodoList.Pages
{
    [AllowAnonymous]
    public class IndexModel : TodoModel
    {
        private readonly WebTodoList.Data.ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager)
        : base(context, authorizationService, userManager) {
            _context = context;
        }

        public IList<TodoItem> TodoItem { get;set; }

        public async Task OnGetAsync()
        {
            var todoItems = from c in Context.Blogs
                           select c;

            var currentUserId = UserManager.GetUserId(User);

            // Only items owned by user will be shown

            todoItems = todoItems.Where(c => c.Email == currentUserId);
            TodoItem = await _context.Blogs.ToListAsync();
        }
    }
}
