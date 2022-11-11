using LibraryRegister.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryRegister.DAL
{
	public interface IBookRepository
	{
		Task<Book?> FindById(int id);
		Task<IEnumerable<Book>> SearchBooks(string query);
		Task InsertBook(Book book);
		Task UpdateBook(Book book);
		Task DeleteBook(int id);
		bool BookExists(Func<Book, bool> predicate);
		Task<Book?> ReturnBook(int id);
		Task Save();
	}
}
