﻿using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using TaskList.DAL.Models;

namespace TaskList.DAL
{
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class TaskListContext : DbContext
    {
        public TaskListContext(string connectionString) : base(connectionString)
        {
            //MySqlProviderServices se = null;
        }

        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<AttachmentType> AttachmentTypes { get; set; }
        public DbSet<PriorityType> PriorityTypes { get; set; }
        public DbSet<RolesType> RolesTypes { get; set; }
        public DbSet<TeamInfo> TeamsInfo { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<TodoAndAttaches> TodoAndAttachments { get; set; }
        public DbSet<TodoAndUsers> TodoAndusers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attachments>();
            modelBuilder.Entity<AttachmentType>();
            modelBuilder.Entity<PriorityType>();
            modelBuilder.Entity<RolesType>();
            modelBuilder.Entity<TeamInfo>();
            modelBuilder.Entity<Teams>();
            modelBuilder.Entity<Todo>();
            modelBuilder.Entity<TodoAndAttaches>();
            modelBuilder.Entity<TodoAndUsers>();
            modelBuilder.Entity<User>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
