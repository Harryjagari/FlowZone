
using FlowZone.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowZone.Api.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{ 
		}

		public DbSet<User> Users { get; set; }

		public DbSet<ToDo> ToDos { get; set; }

		public DbSet<Avatar> Avatars { get; set; }

		public DbSet<Distraction> Distractions { get; set; }

		public DbSet<Challenge> Challenges { get; set; }
	}
}
