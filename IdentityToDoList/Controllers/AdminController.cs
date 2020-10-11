using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityToDoList.Data;
using IdentityToDoList.Entities;
using IdentityToDoList.Models;
using IdentityToDoList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityToDoList.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;

        public AdminController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public ActionResult Index()
        {
            var items = context.TodoListData.OrderBy(x => x.Priority).Include(x => x.ApplicationUsers);

            List<TodoListViewModel> todoListToReturn = items.Select(x => new TodoListViewModel()
            {
                Content = x.Content,
                Datetime = x.Datetime,
                Id = x.Id,
                Priority = x.Priority,
                UserName = x.ApplicationUsers.UserName,
                LeadTime = x.LeadTime
            }).ToList();

            return View(todoListToReturn);
        }
        public ActionResult CompletedTasks()
        {
            var items = context.TodoListData.Where(x => x.Message != null).OrderBy(x => x.Priority).Include(x => x.ApplicationUsers);

            List<TodoListViewModel> todoListToReturn = items.Select(x => new TodoListViewModel()
            {
                Content = x.Content,
                Datetime = x.Datetime,
                Id = x.Id,
                Priority = x.Priority,
                UserName = x.ApplicationUsers.UserName,
                LeadTime = x.LeadTime,
                Message = x.Message
            }).ToList();

            return View(todoListToReturn);
        }
        public IActionResult Create()
        {
            var items = this.context.Set<ApplicationUser>().ToList();
            ViewBag.Users = new SelectList(items, "Id", "UserName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoListViewModel item)
        {
            if (ModelState.IsValid)
            {
                TodoListData items = new TodoListData
                {
                    Content = item.Content,
                    Datetime = item.Datetime,
                    ApplicationUsersId = item.ApplicationUsersId,
                    Priority = item.Priority,
                    LeadTime = item.LeadTime
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

            var items = this.context.Set<ApplicationUser>().ToList();
            ViewBag.Users = new SelectList(items, "Id", "UserName");

            TodoListViewModel modelToReturn = new TodoListViewModel
            {
                Content = item.Content,
                Datetime = item.Datetime,
                Id = item.Id,
                Priority = item.Priority,
                ApplicationUsersId = item.ApplicationUsersId,
                LeadTime = item.LeadTime
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
                var TaskToUpdate = this.context.Set<TodoListData>().FirstOrDefault(x => x.Id == item.Id);

                TaskToUpdate.Content = item.Content;
                TaskToUpdate.Datetime = item.Datetime;
                TaskToUpdate.ApplicationUsersId = item.ApplicationUsersId;
                TaskToUpdate.Priority = item.Priority;
                TaskToUpdate.LeadTime = item.LeadTime;

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
