//using IdentityToDoList.Data;
//using IdentityToDoList.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace IdentityToDoList.Controllers
//{
//    public class ToDoController : Controller
//    {
//        private readonly ApplicationDbContext context;
//        public ToDoController(ApplicationDbContext context)
//        {
//            this.context = context;
//        }
//        [Authorize]
//        public async Task<ActionResult> Index()
//        {
//            var currentUser = context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

//            if (currentUser != null)
//            {
//                var items = context.ToDoListData.Where(x => x. == currentUser.Id).OrderBy(x => x.Priority);

//                List<TodoList> todolist = await items.ToListAsync();

//                return View(todolist);
//            }
//            else
//            {
//                return RedirectToAction("Index");
//            }

//        }
//        public IActionResult Create() => View();
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Create(TodoList item)
//        {
//            if (ModelState.IsValid)
//            {
//                context.Add(item);
//                await context.SaveChangesAsync();

//                TempData["Success"] = "The item has been added!";

//                return RedirectToAction("Index");
//            }

//            return View(item);
//        }

//        public async Task<ActionResult> Edit(int id)
//        {
//            TodoList item = await context.ToDoList.FindAsync(id);

//            if (item == null)
//            {
//                return NotFound();
//            }

//            return View(item);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit(TodoList item)
//        {
//            if (ModelState.IsValid)
//            {
//                context.Update(item);
//                await context.SaveChangesAsync();

//                TempData["Success"] = "The item has been updated!";

//                return RedirectToAction("Index");
//            }

//            return View(item);
//        }
//        public async Task<ActionResult> Delete(int id)
//        {
//            TodoList item = await context.ToDoList.FindAsync(id);

//            if (item == null)
//            {
//                TempData["Error"] = "The item does not exist!";
//            }
//            else
//            {
//                context.ToDoList.Remove(item);
//                await context.SaveChangesAsync();

//                TempData["Success"] = "The item has been deleted!";
//            }

//            return RedirectToAction("Index");
//        }
//    }
//}
