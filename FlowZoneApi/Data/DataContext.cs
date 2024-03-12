
using FlowZoneApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowZoneApi.Data
{
	public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
	{
        public DbSet<User> Users { get; set; }

		public DbSet<ToDo> ToDos { get; set; }

		public DbSet<Avatar> Avatars { get; set; }

		public DbSet<Distraction> Distractions { get; set; }

		public DbSet<Challenge> Challenges { get; set; }

        public DbSet<UserAvatar> UserAvatars { get; set; }

        public DbSet<UserChallenge> UserChallenges { get; set; }
    }
}
