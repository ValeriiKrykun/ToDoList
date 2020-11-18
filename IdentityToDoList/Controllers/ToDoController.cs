using IdentityToDoList.Models;
using IdentityToDoList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IdentityToDoList.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly IUserTaskService userTask;
        public ToDoController(IUserTaskService userTask)
        {
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

                var items = await userTask.CreateTaskForUser(item, currentUser.Id);

                TempData["Success"] = "The item has been added!";

                return RedirectToAction("Index");
            }

            return View(item);
        }
        [HttpGet]
        public async Task<ActionResult>  Edit(int id)
        {
            var modelToReturn = await userTask.GetTaskIdForEdit(id);

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

                await userTask.EditTaskForUser(item, currentUser.Id);

                TempData["Success"] = "The item has been updated!";

                return RedirectToAction("Index");
            }

            return View(item);
        }
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var item = await userTask.GetTaskId(id);

            if (item == null)
            {
                TempData["Error"] = "The item does not exist!";
            }
            else
            {
                await userTask.DeleteTask(item);

                TempData["Success"] = "The item has been deleted!";
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Message(int id)
        {
            var item = await userTask.Message(id);

            if (item.SpendTime == DateTime.MinValue)
            {
                TempData["Error"] = "The task is not completed!";

                return RedirectToAction("Index");
            }

            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Message(TodoListViewModel item)
        {
            await userTask.MessageSend(item);

            TempData["Success"] = "The message has been added!";

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Start(int id)
        {
            await userTask.Start(id);

            TempData["Success"] = "The task has started";

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Stop(int id)
        {

            await userTask.Stop(id);

            TempData["Success"] = "The task has ended";

            return RedirectToAction("Index");
        }
    }
}
