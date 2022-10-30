using LibraryRegister.DTOs;
using LibraryRegister.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace LibraryRegister.DAL
{
	public interface IAuthorRepository
	{
		Task<ActionResult<PaginatedResult<Author>>> GetAuthorsList(int pageIndex, int pageSize);
		Task<ActionResult<IEnumerable<Author>>> GetMatchingAuthors(string name);
		Task<ActionResult<Author>> FindById(int id);
		Task InsertAuthor(Author author);
		Task DeleteAuthor(int id);
		void UpdateAuthor(int id, Author author);
		bool AuthorExists(Func<Author, bool> predicate);
		Task Save();
	}
}
