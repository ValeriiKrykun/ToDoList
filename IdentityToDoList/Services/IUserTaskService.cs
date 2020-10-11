using IdentityToDoList.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityToDoList.Services
{
    public interface IUserTaskService
    {
        int GetTasksCountForUser(string userId);
    }
}
