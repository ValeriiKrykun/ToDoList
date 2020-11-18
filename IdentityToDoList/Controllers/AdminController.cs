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
        private readonly IAdminTaskService adminTask;

        public AdminController(ApplicationDbContext context, IAdminTaskService adminTask)
        {
            this.context = context;
            this.adminTask = adminTask;
        }
        public ActionResult Index()
        {
            var todoListToReturn = adminTask.GetTasksListForAdmin(User.Identity.Name);

            return View(todoListToReturn);
        }
        public ActionResult CompletedTasks()
        {
            var todoListToReturn = adminTask.CompletedTasks(User.Identity.Name);

            return View(todoListToReturn);
        }
        public IActionResult Create()
        {
            var items = adminTask.GetUserList();
            ViewBag.Users = new SelectList(items, "Id", "UserName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoListViewModel item)
        {
            if (ModelState.IsValid)
            {
                await adminTask.CreateTaskForUser(item);

                TempData["Success"] = "The item has been added!";

                return RedirectToAction("Index");
            }

            return View(item);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var item = await adminTask.GetUserTask(id);

            var items = adminTask.GetUserList();
            ViewBag.Users = new SelectList(items, "Id", "UserName");

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoListViewModel item)
        {
            if (ModelState.IsValid)
            {
                await adminTask.EditTaskForUser(item);

                TempData["Success"] = "The item has been updated!";

                return RedirectToAction("Index");
            }

            return View(item);
        }
        public async Task<ActionResult> Delete(int id)
        {
            var item = await adminTask.GetUserTaskForDelete(id);

            if (item == null)
            {
                TempData["Error"] = "The item does not exist!";
            }
            else
            {
                await adminTask.DeleteTask(item);

                TempData["Success"] = "The item has been deleted!";

                return RedirectToAction("CompletedTasks");
            }

            return RedirectToAction("Index");
        }
    }
}
