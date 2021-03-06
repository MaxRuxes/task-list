﻿using System.Data.Entity;
using TaskList.DAL.Entities;

namespace TaskList.DAL
{
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class TaskListContext : DbContext
    {
        public TaskListContext(string connectionString) 
            : base(connectionString) { }

        public DbSet<PriorityType> PriorityTypes { get; set; }
        public DbSet<ProjectInfo> ProjectInfo { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<TodoAndUsers> TodoAndUsers { get; set; }
        public DbSet<TodoAndProjects> TodoAndProjects { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriorityType>();
            modelBuilder.Entity<ProjectInfo>();
            modelBuilder.Entity<Projects>();
            modelBuilder.Entity<Todo>();
            modelBuilder.Entity<TodoAndUsers>();
            modelBuilder.Entity<TodoAndProjects>();
            modelBuilder.Entity<User>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
