using FlowZoneApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowZoneApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // DbSet properties
        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<UserAvatar> UserAvatars { get; set; }
        public DbSet<UserChallenge> UserChallenges { get; set; }
        public DbSet<UserToDo> UserToDos { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    AdminId = Guid.NewGuid(),
                    AdminUserName = "Harendra",
                    AdminPassword = "Harendra123"
                }
            );

            modelBuilder.Entity<UserToDo>()
                .HasKey(ut => new { ut.UserId, ut.ToDoId });

            modelBuilder.Entity<UserToDo>()
                .HasOne(ut => ut.ToDo)
                .WithMany(t => t.UserTodos)
                .HasForeignKey(ut => ut.ToDoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserToDo>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTodos)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
