using LibraryRegister.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryRegister.DAL
{
	public class BookRepository : IBookRepository
	{
		readonly LibraryDbContext db;

		public BookRepository(LibraryDbContext db)
		{
			this.db = db;
		}

		public async Task<Book?> FindById(int id)
		{
			return await db.Book
				.Include(b => b.Author)
				.FirstOrDefaultAsync(b => b.Id == id);
		}

		public async Task<IEnumerable<Book>> SearchBooks(string query)
		{
			return await db.Book
				.Where(b => (
					b.Author!.Name.Contains(query) ||
					b.Title.Contains(query)
				))
				.Include(x => x.Author)
				.Take(10)
				.ToListAsync();
		}

		public async Task InsertBook(Book book)
		{
			await db.Book.AddAsync(book);
		}

		public async Task<Book?> ReturnBook(int id)
		{
			var book = await FindById(id);

			if (book == null)
			{
				return null;
			}

			book.IsAvailable = true;

			var leasing = await db.Leasing
				.Where(
					l => l.BookId == id &&
					l.ReturnDate == default)
				.FirstOrDefaultAsync();

			if (leasing != null)
			{
				leasing.ReturnDate = DateTime.Now;
			}

			return book;
		}

		public async Task DeleteBook(int id)
		{
			var book = await FindById(id);
			if (book != null)
			{
				await Task.Run(() => {
					db.Book.Remove(book);
				});
			}
		}

		public async Task UpdateBook(Book book)
		{
			await Task.Run(() => {
				db.Entry(book).State = EntityState.Modified;
			});
		}

		public bool BookExists(Func<Book, bool> predicate)
			=> db.Book.Any(predicate);

		public async Task Save()
		{
			await db.SaveChangesAsync();
		}
	}
}
