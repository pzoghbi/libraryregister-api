using LibraryRegister.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryRegister.DAL
{
	public interface IMembershipRepository
	{
		Task<Membership?> FindByUserId(int userId);
		Task<Membership?> FindByGuid(Guid guid);
		Task InsertMembership(Membership membership);
		Task UpdateMembership(Membership membership);
		Task DeleteMembership(Guid guid);
		Task<int> Save();
		bool MembershipExists(Func<Membership, bool> predicate);
	}
}
