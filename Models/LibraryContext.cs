using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MySql.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using LibraryRegister.Models;

namespace LibraryRegister.Models
{
	public class LibraryDbContext : DbContext
	{
		readonly IConfiguration _configuration;

		public LibraryDbContext (
			DbContextOptions<LibraryDbContext> dbContextOptions, 
			IConfiguration configuration
		) : base(dbContextOptions)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySQL(_configuration.GetConnectionString("DefaultConnection"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Author>().HasData(
				new { Id = 1, Name = "Unknown Author"},
				new { Id = 2, Name = "Dale Carnegie" },
				new { Id = 3, Name = "Richard Koch" },
				new { Id = 4, Name = "Robert Kiyosaki" },
				new { Id = 5, Name = "Wim Hof" },
				new { Id = 6, Name = "Julian Šribar, Boris Motik" }
			);

			modelBuilder.Entity<Book>().HasData(
				new { Id = 1, Title = "How to Win Friends and Influence People",	AuthorId = 2, IsAvailable = false },	
				new { Id = 2, Title = "The 80/20 Principle",						AuthorId = 3, IsAvailable = false },
				new { Id = 3, Title = "Rich Dad Poor Dad",							AuthorId = 4, IsAvailable = true },
				new { Id = 4, Title = "Communicate your way to success",			AuthorId = 2, IsAvailable = true },
				new { Id = 5, Title = "Metoda Wim Hof",								AuthorId = 5, IsAvailable = true },
				new { Id = 6, Title = "Demistificirani C++",						AuthorId = 6, IsAvailable = true }
			);

			modelBuilder.Entity<User>().HasData(
				new { Id = 1001, Name = "Default User", Email = "default@email.com" }	
			);

			modelBuilder.Entity<Leasing>().HasData(
				new { Id = 1, UserId = 1001, BookId = 1, LeaseDate = DateTime.UtcNow.AddDays(-1) },
				new { Id = 2, UserId = 1001, BookId = 2, LeaseDate = DateTime.UtcNow.AddDays(-1) }
			);
		}

		public DbSet<User> User { get; set; } = null!;
		public DbSet<Membership> Membership { get; set; } = null!;
		public DbSet<Author> Author { get; set; } = null!;
		public DbSet<Book> Book { get; set; } = null!;
		public DbSet<Leasing> Leasing { get; set; } = null!;
		 
	}


}
