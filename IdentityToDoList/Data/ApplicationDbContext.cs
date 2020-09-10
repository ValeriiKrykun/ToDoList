using System;
using System.Collections.Generic;
using System.Text;
using IdentityToDoList.Entities;
using IdentityToDoList.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityToDoList.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<TodoListData> TodoListData { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TodoListData>()
                .HasOne(e => e.ApplicationUsers)
                .WithMany(c => c.TodoListData);
            base.OnModelCreating(builder);
        }
    }
}
