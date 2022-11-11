using LibraryRegister.DTOs;
using LibraryRegister.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace LibraryRegister.DAL
{
	public interface IAuthorRepository
	{
		Task<PaginatedResult<Author>> GetAuthorsList(int pageIndex, int pageSize);
		Task<IEnumerable<Author>> GetMatchingAuthors(string name);
		Task<Author?> FindById(int id);
		Task InsertAuthor(Author author);
		Task DeleteAuthor(int id);
		void UpdateAuthor(int id, Author author);
		bool AuthorExists(Func<Author, bool> predicate);
		Task Save();
	}
}
