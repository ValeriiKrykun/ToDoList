using IdentityToDoList.Data;
using IdentityToDoList.Entities;
using IdentityToDoList.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityToDoList.Services
{
    public class AdminTaskService : IAdminTaskService
    {
        private readonly ApplicationDbContext context;

        public AdminTaskService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<TodoListViewModel> GetTasksListForAdmin(string userId)
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

            return todoListToReturn;
        }

        public List<TodoListViewModel> CompletedTasks(string userId)
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
                Message = x.Message,
                SpendTime = x.SpendTime
            }).ToList();

            return todoListToReturn;
        }

        public List<ApplicationUser> GetUserList()
        {
            var items = this.context.Set<ApplicationUser>().ToList();

            return items;
        }

        public async Task CreateTaskForUser(TodoListViewModel item)
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
        }

        public async Task<TodoListViewModel> GetUserTask(int id)
        {
            var item = await context.TodoListData.FindAsync(id);

            TodoListViewModel modelToReturn = new TodoListViewModel
            {
                Content = item.Content,
                Datetime = item.Datetime,
                Id = item.Id,
                Priority = item.Priority,
                ApplicationUsersId = item.ApplicationUsersId,
                LeadTime = item.LeadTime
            };

            return modelToReturn;
        }

        public async Task EditTaskForUser(TodoListViewModel item)
        {
            var TaskToUpdate = this.context.Set<TodoListData>().FirstOrDefault(x => x.Id == item.Id);

            TaskToUpdate.Content = item.Content;
            TaskToUpdate.Datetime = item.Datetime;
            TaskToUpdate.ApplicationUsersId = item.ApplicationUsersId;
            TaskToUpdate.Priority = item.Priority;
            TaskToUpdate.LeadTime = item.LeadTime;

            context.TodoListData.Update(TaskToUpdate);

            await context.SaveChangesAsync();
        }
        public async Task<TodoListData> GetUserTaskForDelete(int id)
        {
            var item = await context.TodoListData.FindAsync(id);

            return item;
        }
        public async Task DeleteTask(TodoListData item)
        {
            context.TodoListData.Remove(item);
            await context.SaveChangesAsync();
        }
    }
}
