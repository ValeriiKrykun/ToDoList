using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityToDoList.Models
{
    public class TodoListData
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Datetime { get; set; }
        public int Priority { get; set; }
        public int UserId { get; set; }
        public ApplicationUser ApplicationUsers { get; set; }
    }
}
