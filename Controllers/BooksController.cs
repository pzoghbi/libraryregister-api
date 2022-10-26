using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryRegister.Models;

namespace LibraryRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook([FromQuery] string? bookSearchQuery)
        {
            return await _context.Book
                .Where(b => (
                    b.Author.Name.Contains(bookSearchQuery) ||
                    b.Title.Contains(bookSearchQuery)
                ))
                .Include(x => x.Author)
                .Take(10)
                .ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book
                .Where(b => b.Id == id)
                .Include(b => b.Author)
                .FirstOrDefaultAsync();

            if (book == null) { return NotFound(); }
            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id) { return BadRequest(); }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromForm] Book book)
        {
            Console.WriteLine(book);
            if (!_context.Author.Any(e => e.Id == book.AuthorId)) {
                // error
                throw new Exception("errorchich: u bazi nema covjeka kojeg trazis");
			} else {
                book.Author = _context.Author.Find(book.AuthorId);
			}

            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        { 
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/book/5/return
        [HttpGet("{bookId}/return")]
        public async Task<ActionResult<Book>> ReturnBook(int bookId)
        {
            var book = await _context.Book
                .Where(b => b.Id == bookId)
                .Include(b => b.Author)
                .FirstOrDefaultAsync();

            if (book == null) return NotFound("Book not found");
            if (book.IsAvailable) return BadRequest("Book already returned");
            book.IsAvailable = true;

            // Find last relevant leasing
            // Todo use service/repo to update leasing
            var leasing = await _context.Leasing
                .Where(
                    l => l.BookId == bookId &&
                    l.ReturnDate == default)
                .FirstOrDefaultAsync();

            if (leasing != null) leasing.ReturnDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return book;
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
