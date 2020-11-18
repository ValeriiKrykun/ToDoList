using IdentityToDoList.Data;
using IdentityToDoList.Entities;
using IdentityToDoList.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityToDoList.Services
{
    public interface IAdminTaskService
    {
        List<TodoListViewModel> GetTasksListForAdmin(string userId);

        List<TodoListViewModel> CompletedTasks(string userId);

        List<ApplicationUser> GetUserList();

        Task CreateTaskForUser(TodoListViewModel item);

        Task<TodoListViewModel> GetUserTask(int id);

        Task EditTaskForUser(TodoListViewModel item);

        Task<TodoListData> GetUserTaskForDelete(int id);

        Task DeleteTask(TodoListData item);
    }
}
