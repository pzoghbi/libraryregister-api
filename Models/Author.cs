using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibraryRegister.Models
{
	[Index(nameof(Name), IsUnique = true)]
	public sealed class Author
	{
		[Key] public int Id { get; set; }
		[Required] public string Name { get; set; } = null!;
	}
}