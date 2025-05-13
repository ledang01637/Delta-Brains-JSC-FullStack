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
        public DbSet<DeviceSession> DeviceSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            // User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasColumnType("char(64)");

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithOne()
                .HasForeignKey<User>(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);



            // Task
            modelBuilder.Entity<Task>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Task>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification
            modelBuilder.Entity<Notification>()
                .HasKey(n => n.Id);

            modelBuilder.Entity<Notification>()
                .Property(n => n.Message)
                .IsRequired();

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // DeviceSession
            modelBuilder.Entity<DeviceSession>()
                .HasKey(ds => ds.SessionId);

            modelBuilder.Entity<DeviceSession>()
                .HasOne(ds => ds.User)
                .WithMany(u => u.DeviceSessions)
                .HasForeignKey(ds => ds.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}
