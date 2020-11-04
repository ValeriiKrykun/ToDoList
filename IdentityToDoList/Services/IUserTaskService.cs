using IdentityToDoList.Entities;
using IdentityToDoList.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityToDoList.Services
{
    public interface IUserTaskService
    {
        int GetTasksCountForUser(string userId);

        ApplicationUser UserAuthorizationCheck(string userName);

        List<TodoListViewModel> GetTasksListForUser(string userId);

        TodoListData CreateTaskForUser(TodoListViewModel item, string userId);

        Task<TodoListViewModel> GetTaskIdForEdit(int id);

        TodoListData EditTaskForUser(TodoListViewModel item, string userId);

        Task<TodoListData> GetTaskId(int id);

        Task<TodoListData> DeleteTask(TodoListData item);
    }
}
