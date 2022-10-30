using LibraryRegister.DTOs;
using LibraryRegister.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryRegister.DAL
{
	public class AuthorRepository : IAuthorRepository
	{
		readonly LibraryDbContext db;

		public AuthorRepository(LibraryDbContext db)
		{
			this.db = db;
		}

		public bool AuthorExists(Func<Author, bool> predicate) {
			return db.Author.Any(predicate);
		}

		public async Task DeleteAuthor(int id)
		{
			var author = await db.Author.FindAsync(id);
			if (author != null) {
				db.Author.Remove(author);
			}
		}

		public async Task<ActionResult<Author>> FindById(int id)
		{
			return await db.Author.FindAsync(id);
		}

		public async Task<ActionResult<PaginatedResult<Author>>> 
			GetAuthorsList(int pageIndex, int pageSize)
		{
			var paginatedList = await PaginatedList<Author>.CreateAsync(
				db.Author,
				pageIndex,
				pageSize
			);

			return new PaginatedResult<Author>(paginatedList);
		}

		public async Task<ActionResult<IEnumerable<Author>>> GetMatchingAuthors(string name)
		{
			return await db.Author
				.Where(a => 
					a.Name.ToLower().StartsWith(name.ToLower())
				)
				.ToListAsync();
		}

		public async Task InsertAuthor(Author author)
		{
			await db.Author.AddAsync(author);
		}

		public async Task Save()
		{
			await db.SaveChangesAsync();
		}

		public void UpdateAuthor(int id, Author author)
		{
			db.Entry(author).State = EntityState.Modified;
		}
	}
}
