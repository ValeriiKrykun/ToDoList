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
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IUserTaskService userTask;
        public ToDoController(ApplicationDbContext context, IUserTaskService userTask)
        {
            this.context = context;
            this.userTask = userTask;
        }
        public ActionResult Index()
        {
            var currentUser = userTask.UserAuthorizationCheck(User.Identity.Name);

            if (currentUser != null)
            {
                ViewBag.Message = userTask.GetTasksCountForUser(currentUser.Id);

                var todoListToReturn = userTask.GetTasksListForUser(currentUser.Id);

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
                var currentUser = userTask.UserAuthorizationCheck(User.Identity.Name);

                var items = userTask.CreateTaskForUser(item, currentUser.Id);

                context.TodoListData.Add(items);

                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been added!";

                return RedirectToAction("Index");
            }

            return View(item);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var modelToReturn = userTask.GetTaskIdForEdit(id);

            if (modelToReturn == null)
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
                var currentUser = userTask.UserAuthorizationCheck(User.Identity.Name);

                var TaskToUpdate = userTask.EditTaskForUser(item, currentUser.Id);

                context.TodoListData.Update(TaskToUpdate);

                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been updated!";

                return RedirectToAction("Index");
            }

            return View(item);
        }
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var item = userTask.GetTaskId(id);

            if (item == null)
            {
                TempData["Error"] = "The item does not exist!";
            }
            else
            {
                var task = await userTask.DeleteTask(item);

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
