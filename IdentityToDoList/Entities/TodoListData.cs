using System;
using IdentityToDoList.Models;

namespace IdentityToDoList.Entities
{
    public class TodoListData
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Datetime { get; set; }
        public int Priority { get; set; }
        public int ApplicationUsersId { get; set; }
        public ApplicationUser ApplicationUsers { get; set; }
    }
}
