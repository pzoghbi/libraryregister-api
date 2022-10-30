using LibraryRegister.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryRegister.DAL
{
	public class UserRepository : IUserRepository, IDisposable
	{
		readonly LibraryDbContext db;

		public UserRepository(LibraryDbContext db)
		{
			this.db = db;
		}

		public async Task<ActionResult<IEnumerable<User>>> GetMatchingUsers(string name)
		{
			return await db.User
				.Where(u =>
					u.Name.Contains(name) ||
					u.Email.Contains(name)
				)
				.ToListAsync();
		}

		public async Task<ActionResult<User>> FindById(int id)
		{
			return await db.User
				.Where(u => u.Id == id)
				.Include(u => u.Leasings
					.Where(l => l.ReturnDate == default))
				.ThenInclude(l => l.Book.Author)
				.FirstOrDefaultAsync();
		}

		public void UpdateUser(int id, User user)
		{
			db.Entry(user).State = EntityState.Modified;
		}

		public async Task InsertUser(User user)
		{
			db.User.Add(user);
			await db.SaveChangesAsync();
		}

		public bool UserExists(Func<User, bool> predicate) => db.User.Any(predicate);

		public async Task DeleteUser(int id) {
			var user = await db.User.FindAsync(id);
			if (user != null) {
				db.User.Remove(user);
			}
			await Save();
		}

		public async Task Save() => await db.SaveChangesAsync();

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed) {
				if (disposing) {
					db.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
