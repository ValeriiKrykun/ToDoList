using IdentityToDoList.Data;
using IdentityToDoList.Entities;
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
    }
}
