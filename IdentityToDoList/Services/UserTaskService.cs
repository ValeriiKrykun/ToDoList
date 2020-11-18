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
using System.Net.Mail;
using System.Net;

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

        public async Task<TodoListData> CreateTaskForUser(TodoListViewModel item, string userId)
        {
            var currentUser = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

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

        public async Task<TodoListData> EditTaskForUser(TodoListViewModel item, string userId)
        {
            var currentUser = context.Users.Where(x => x.Id == userId).FirstOrDefault();

            var TaskToUpdate = this.context.Set<TodoListData>().FirstOrDefault(x => x.Id == item.Id);

            TaskToUpdate.Content = item.Content;
            TaskToUpdate.Datetime = item.Datetime;
            TaskToUpdate.ApplicationUsersId = currentUser.Id.ToString();
            TaskToUpdate.Priority = item.Priority;
            TaskToUpdate.LeadTime = item.LeadTime;

            context.TodoListData.Update(TaskToUpdate);

            await context.SaveChangesAsync();

            return (TaskToUpdate);
        }

        public async Task<TodoListData> GetTaskId(int id)
        {
            TodoListData item = await context.TodoListData.FindAsync(id);

            return item;
        }

        public async Task DeleteTask(TodoListData item)
        {
            context.TodoListData.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task<TodoListViewModel> Message(int id)
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

            return modelToReturn;
        }

        public async Task MessageSend(TodoListViewModel item)
        {
            var TaskToUpdate = context.Set<TodoListData>().FirstOrDefault(x => x.Id == item.Id);

            TaskToUpdate.Message = item.Message;

            context.TodoListData.Update(TaskToUpdate);

            var fromAddress = new MailAddress("valeriikrykun@gmail.com", "From Name");
            var toAddress = new MailAddress("valeriikrykun@gmail.com", "To Name");
            const string fromPassword = "valera92valera";
            const string subject = "New message aboout completed task";
            const string body = "Read message and back to work bitch";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            await context.SaveChangesAsync();
        }

        public async Task Start(int id)
        {
            var start = DateTime.Now;

            TodoListData item = await context.TodoListData.FindAsync(id);

            item.Start = start;

            context.TodoListData.Update(item);

            await context.SaveChangesAsync();
        }

        public async Task Stop(int id)
        {
            TodoListData item = await context.TodoListData.FindAsync(id);

            var spendTime = DateTime.Now - item.Start;

            var spendTimePlus = item.SpendTime + spendTime;

            item.SpendTime = spendTimePlus;

            context.TodoListData.Update(item);

            await context.SaveChangesAsync();
        }
    }
}
