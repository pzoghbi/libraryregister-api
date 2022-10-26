using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibraryRegister.Models
{
	[Index(nameof(UserId), IsUnique = false)]
	public class Membership
	{
		[Key] public Guid Guid { get; set; }
		public DateTime? ValidUntil { get; set; }
		public User? User { get; set; }
		public int UserId { get; set; }

		public Membership()
		{
			ValidUntil = DateTime.Now.AddYears(1);
		}
	}
}
