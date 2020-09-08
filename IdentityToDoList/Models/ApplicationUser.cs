using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityToDoList.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<TodoListData> TodoListData { get; set; }
    }
}
