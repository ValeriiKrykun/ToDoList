using IdentityToDoList.Data;
using IdentityToDoList.Entities;
using IdentityToDoList.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityToDoList.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly ApplicationDbContext context;

        public UserTaskService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public int GetTasksCountForUser(string userId)
        {
            var currentUser = context.Users.Where(x => x.Id == userId).FirstOrDefault();

            var userTasks = context.TodoListData.Where(x => currentUser.Id == x.ApplicationUsersId).Count();

            return (userTasks);
        }

        public List<TodoListViewModel> GetTasksListForUser(string userId)
        {
            var currentUser = context.Users.Where(x => x.Id == userId).FirstOrDefault();

            var items = context.TodoListData.Where(x => currentUser.Id == x.ApplicationUsersId).OrderBy(x => x.Priority);

            List<TodoListViewModel> todoListToReturn = items.Select(x => new TodoListViewModel()
            {
                Content = x.Content,
                Datetime = x.Datetime,
                Id = x.Id,
                Priority = x.Priority,
                LeadTime = x.LeadTime,
                SpendTime = x.SpendTime
            }).ToList();

            return todoListToReturn;
        }

        public ApplicationUser UserAuthorizationCheck(string userName)
        {
            var currentUser = context.Users.Where(x => x.Email == userName).FirstOrDefault();

            return (currentUser);
        }

        public TodoListData CreateTaskForUser(TodoListViewModel item, string userId)
        {
            var currentUser = context.Users.Where(x => x.Id == userId).FirstOrDefault();

            TodoListData items = new TodoListData
            {
                Content = item.Content,
                Datetime = item.Datetime,
                ApplicationUsersId = currentUser.Id.ToString(),
                Priority = item.Priority,
                LeadTime = item.LeadTime
            };

            return (items);
        }

        public async Task<TodoListViewModel> GetTaskIdForEdit(int id)
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

            return modelToReturn;
        }

        public TodoListData EditTaskForUser(TodoListViewModel item, string userId)
        {
            var currentUser = context.Users.Where(x => x.Email == userId).FirstOrDefault();

            var TaskToUpdate = this.context.Set<TodoListData>().FirstOrDefault(x => x.Id == item.Id);

            TaskToUpdate.Content = item.Content;
            TaskToUpdate.Datetime = item.Datetime;
            TaskToUpdate.ApplicationUsersId = currentUser.Id.ToString();
            TaskToUpdate.Priority = item.Priority;
            TaskToUpdate.LeadTime = item.LeadTime;

            return (TaskToUpdate);
        }
    }
}
