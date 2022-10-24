using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TO_DO.Models
{
    public partial class TododbContext : DbContext
    {
        public TododbContext()
        {
        }

        public TododbContext(DbContextOptions<TododbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tasks> Tasks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-NBFBG08\\SQLEXPRESS; Database=TodoAppDatabase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.Property(e => e.TaskID).HasColumnName("TaskID");

                entity.Property(e => e.TaskTitle).HasMaxLength(50);

                entity.Property(e => e.TaskDetails).HasColumnType("text"); 

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserID).HasColumnName("UserID");

                entity.Property(e => e.Username).HasMaxLength(225);

                entity.Property(e => e.Password).HasMaxLength(225);

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

