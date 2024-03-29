﻿using LibraryRegister.DTOs;
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

		public bool AuthorExists(Func<Author, bool> predicate) => db.Author.Any(predicate);

		public async Task DeleteAuthor(int id)
		{
			var author = await db.Author.FindAsync(id);
			if (author != null) {
				db.Author.Remove(author);
			}
		}

		public async Task<Author?> FindById(int id) => await db.Author.FindAsync(id);

		public async Task<PaginatedResult<Author>> 
			GetAuthorsList(int pageIndex, int pageSize)
		{
			var paginatedList = await PaginatedList<Author>.CreateAsync(
				db.Author,
				pageIndex,
				pageSize
			);

			return new PaginatedResult<Author>(paginatedList);
		}

		public async Task<IEnumerable<Author>> GetMatchingAuthors(string name) {
			return await db.Author
				.Where(a => 
					a.Name.ToLower().StartsWith(name.ToLower())
				)
				.ToListAsync();
		}

		public async Task InsertAuthor(Author author) {
			await db.Author.AddAsync(author);
		}

		public void UpdateAuthor(int id, Author author) {
			Task.Run(() => {
				db.Entry(author).State = EntityState.Modified;
			});
		}

		public async Task Save() {
			await db.SaveChangesAsync();
		}

	}
}
