using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibraryRegister.Models
{
	[Index(nameof(Email), IsUnique = true)]
	public class User
	{
		[Key] public int Id { get; set; }
		[Required] public string Name { get; set; } = null!;
		public string? Email { get; set; }
		public List<Leasing>? Leasings { get; set; } = null!;

		// todo add address, ID number, etc
	}
}
