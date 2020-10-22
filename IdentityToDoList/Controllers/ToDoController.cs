using IdentityToDoList.Data;
using IdentityToDoList.Entities;
using IdentityToDoList.Models;
using IdentityToDoList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace IdentityToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IUserTaskService userTask;

        [Obsolete]
        public ToDoController(ApplicationDbContext context, IUserTaskService userTask)
        {
            this.context = context;
            this.userTask = userTask;
        }
        [Authorize]
        public ActionResult Index()
        {
            var currentUser = context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            if (currentUser != null)
            {
                var items = context.TodoListData.Where(x => currentUser.Id == x.ApplicationUsersId).OrderBy(x => x.Priority);

                ViewBag.Message = userTask.GetTasksCountForUser(currentUser.Id);

                List<TodoListViewModel> todoListToReturn = items.Select(x => new TodoListViewModel()
                {
                    Content = x.Content,
                    Datetime = x.Datetime,
                    Id = x.Id,
                    Priority = x.Priority,
                    LeadTime = x.LeadTime,
                    SpendTime = x.SpendTime
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
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var item = await context.TodoListData.FindAsync(id);

            TodoListViewModel modelToReturn = new TodoListViewModel
            {
                Content = item.Content,
                Datetime = item.Datetime,
                Id = item.Id,
                Priority = item.Priority,
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
                var currentUser = context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

                var TaskToUpdate = this.context.Set<TodoListData>().FirstOrDefault(x => x.Id == item.Id);

                TaskToUpdate.Content = item.Content;
                TaskToUpdate.Datetime = item.Datetime;
                TaskToUpdate.ApplicationUsersId = currentUser.Id.ToString();
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
        [HttpGet]
        public async Task<ActionResult> Message(int id)
        {
            var item = await context.TodoListData.FindAsync(id);

            TodoListViewModel modelToReturn = new TodoListViewModel
            {
                Content = item.Content,
                Datetime = item.Datetime,
                Id = item.Id,
                Priority = item.Priority,
                LeadTime = item.LeadTime,
                SpendTime = item.SpendTime
            };

            if (item.SpendTime == DateTime.MinValue)
            {
                TempData["Success"] = "The task is not completed!";

                return RedirectToAction("Index");
            }

            return View(modelToReturn);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Message(TodoListViewModel item)
        {
            var TaskToUpdate = this.context.Set<TodoListData>().FirstOrDefault(x => x.Id == item.Id);

            TaskToUpdate.Message = item.Message;

            context.TodoListData.Update(TaskToUpdate);

            await context.SaveChangesAsync();

            TempData["Success"] = "The message has been added!";

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Start(int id)
        {
            var start = DateTime.Now;

            TodoListData item = await context.TodoListData.FindAsync(id);

            item.Start = start;

            context.TodoListData.Update(item);

            await context.SaveChangesAsync();

            TempData["Success"] = "The task has started";

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Stop(int id)
        {

            TodoListData item = await context.TodoListData.FindAsync(id);

            var spendTime = DateTime.Now - item.Start;

            var spendTimePlus = item.SpendTime + spendTime;

            item.SpendTime = spendTimePlus;

            context.TodoListData.Update(item);

            await context.SaveChangesAsync();

            TempData["Success"] = "The task has ended";

            return RedirectToAction("Index");
        }
    }
}
