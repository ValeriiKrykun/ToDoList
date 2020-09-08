using System;
using System.Collections.Generic;
using System.Text;
using IdentityToDoList.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityToDoList.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<TodoList> ToDoList { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasOne(e => e.TodoListData)
                .WithOne(c => c.ApplicationUsers);
            base.OnModelCreating(builder);
        }
    }
}
