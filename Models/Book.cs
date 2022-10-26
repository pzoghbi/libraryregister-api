using System.ComponentModel.DataAnnotations;

namespace LibraryRegister.Models
{
	public class Book
	{
		[Key] public int Id { get; set; }
		[Required] public string Title { get; set; } = null!;
		public string? Genre { get; set; }
		public int AuthorId { get; set; }
		public Author? Author { get; set; } = null!;
		public bool IsAvailable { get; set; } = true;
	}
}
