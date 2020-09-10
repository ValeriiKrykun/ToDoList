using IdentityToDoList.Data;
using IdentityToDoList.Entities;
using IdentityToDoList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ApplicationDbContext context;
        public ToDoController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var currentUser = context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            if (currentUser != null)
            {
                var items = context.TodoListData.Where(x => x.ApplicationUsersId == currentUser.Id).OrderBy(x => x.Priority);

                List<TodoListData> todolist = await items.ToListAsync();

                return View(todolist);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        public IActionResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoListData item)
        {
            if (ModelState.IsValid)
            {
                context.Add(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been added!";

                return RedirectToAction("Index");
            }

            return View(item);
        }

        public async Task<ActionResult> Edit(int id)
        {
            TodoListData item = await context.TodoListData.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoListData item)
        {
            if (ModelState.IsValid)
            {
                context.Update(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been updated!";

                return RedirectToAction("Index");
            }

            return View(item);
        }
        public async Task<ActionResult> Delete(int id)
        {
            TodoListData item = await context.TodoListData.FindAsync(id);

            if (item == null)
            {
                TempData["Error"] = "The item does not exist!";
            }
            else
            {
                context.TodoListData.Remove(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been deleted!";
            }

            return RedirectToAction("Index");
        }
    }
}
