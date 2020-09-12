using IdentityToDoList.Data;
using IdentityToDoList.Entities;
using IdentityToDoList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
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
        private readonly SignInManager<ApplicationUser> signInManager;
        public ToDoController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            this.context = context;
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult Index()
        {
            var currentUser = context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            if (currentUser != null)
            {
                var items = context.TodoListData.Where(x => currentUser.Id == x.ApplicationUsersId).OrderBy(x => x.Priority);

                List<TodoListViewModel> todoListToReturn = items.Select(x => new TodoListViewModel()
                {
                    Content = x.Content,
                    Datetime = x.Datetime,
                    Id = x.Id,
                    Priority = x.Priority
                }).ToList();

                return View(todoListToReturn);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        public IActionResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoListViewModel item)
        {
            if (ModelState.IsValid)
            {
                var currentUser = context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

                TodoListData items = new TodoListData
                {
                    Content = item.Content,
                    Datetime = item.Datetime,
                    ApplicationUsersId = currentUser.Id.ToString(),
                    Priority = item.Priority
                };

                context.TodoListData.Add(items);

                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been added!";

                return RedirectToAction("Index");
            }

            return View(item);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var item = await context.TodoListData.FindAsync(id);

            TodoListViewModel modelToReturn = new TodoListViewModel
            {
                Content = item.Content,
                Datetime = item.Datetime,
                Id = item.Id,
                Priority = item.Priority
            };

            if (item == null)
            {
                return NotFound();
            }

            return View(modelToReturn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoListViewModel item)
        {
            if (ModelState.IsValid)
            {
                var currentUser = context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

                var TaskToUpdate = this.context.Set<TodoListData>().FirstOrDefault(x => x.Id == item.Id);

                TaskToUpdate.Content = item.Content;
                TaskToUpdate.Datetime = item.Datetime;
                TaskToUpdate.ApplicationUsersId = currentUser.Id.ToString();
                TaskToUpdate.Priority = item.Priority;

                context.TodoListData.Update(TaskToUpdate);

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
