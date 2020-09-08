using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IdentityToDoList.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<TodoListData> TodoListData { get; set; }
    }
}
