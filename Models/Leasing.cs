using System.ComponentModel.DataAnnotations;

namespace LibraryRegister.Models
{
	public class Leasing
	{
		[Key] public int Id { get; set; }
		[Required] public int UserId { get; set; }
		//public User? User { get; set; } = null!;
		[Required] public int BookId { get; set; }
		public Book? Book { get; set; } = null!;
		[Required] public DateTime LeaseDate { get; set; }
		public DateTime? ReturnDate { get; set; } = null;

		public Leasing()
		{
			LeaseDate = DateTime.UtcNow;
			ReturnDate = default;
		}
	}
}
