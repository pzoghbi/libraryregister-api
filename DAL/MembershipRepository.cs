using LibraryRegister.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryRegister.DAL
{
	public class MembershipRepository : IMembershipRepository
	{
		readonly LibraryDbContext db;

		public MembershipRepository(LibraryDbContext db)
		{
			this.db = db;
		}

		public async Task<Membership?> FindByUserId(int userId)
		{
			return await db.Membership
				.Where(m => m.UserId == userId)
				.OrderByDescending(m => m.ValidUntil)
				.FirstOrDefaultAsync();
		}

		public async Task<Membership?> FindByGuid(Guid guid)
		{
			return await db.Membership.FindAsync(guid);
		}

		public async Task InsertMembership(Membership membership)
		{
			await db.Membership.AddAsync(membership);
		}

		public async Task UpdateMembership(Membership membership)
		{
			await Task.Run(() => {
				db.Entry(membership).State = EntityState.Modified;
			});
		}

		public async Task DeleteMembership(Guid guid)
		{
			var membership = await FindByGuid(guid);
			if (membership != null)
			{
				db.Membership.Remove(membership);
			}
		}

		public async Task<int> Save()
		{
			return await db.SaveChangesAsync();
		}

		public bool MembershipExists(Func<Membership, bool> predicate)
		{
			return /*await Task<bool>.Run(() => */db.Membership.Any(predicate);
		}
	}
}
