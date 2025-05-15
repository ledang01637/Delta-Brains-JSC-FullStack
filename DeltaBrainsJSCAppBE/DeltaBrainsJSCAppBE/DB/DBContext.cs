using DeltaBrainsJSCAppBE.Models;
using Microsoft.EntityFrameworkCore;
using Task = DeltaBrainsJSCAppBE.Models.Task;

namespace DeltaBrainJSC.DB
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Role
            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(255);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasColumnType("char(64)");

                entity.HasOne(u => u.Role)
                    .WithMany(u => u.Users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            
            // Task
            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(t => t.Title)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.HasOne(t => t.Assignee)
                      .WithMany(u => u.AssignedTasks)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.AssignedByUser)
                      .WithMany(u => u.CreatedTasks)
                      .HasForeignKey(t => t.AssignedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(n => n.Message)
                .IsRequired();

                entity.HasOne(n => n.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.Task)
                    .WithMany()
                    .HasForeignKey(n => n.RelatedTaskId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            base.OnModelCreating(modelBuilder);
        }

    }
}
