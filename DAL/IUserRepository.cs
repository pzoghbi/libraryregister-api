using LibraryRegister.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryRegister.DAL
{
	public interface IUserRepository 
	{
		Task<IEnumerable<User>> GetMatchingUsers(string search);
		Task<User?> FindById(int id);
		Task InsertUser(User user);
		Task DeleteUser(int id);
		void UpdateUser(int id, User user);
		bool UserExists(Func<User, bool> predicate);
		Task Save();
	}
}
