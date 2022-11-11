using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryRegister.Models;
using LibraryRegister.DAL;

namespace LibraryRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        readonly IBookRepository bookRepo;
        readonly IAuthorRepository authorRepo;

        public BooksController(LibraryDbContext context)
        {
            bookRepo = new BookRepository(context);
            authorRepo = new AuthorRepository(context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook([FromQuery] string bookSearchQuery)
        {
            return Ok(await bookRepo.SearchBooks(bookSearchQuery));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await bookRepo.FindById(id);
            if (book == null) { return NotFound(); }
            return book;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id) { return BadRequest(); }

            await bookRepo.UpdateBook(book);

            try {
                await bookRepo.Save();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromForm] Book book)
        {
            bool authorExists = authorRepo.AuthorExists(a => a.Id == book.AuthorId);

            if (!authorExists) {
                return NotFound("Author not found");
            }
            else {
                book.Author = await authorRepo.FindById(book.AuthorId);
            }

            await bookRepo.InsertBook(book);
            await bookRepo.Save();

            return CreatedAtAction(
                nameof(GetBook), 
                new { id = book.Id }, 
                book
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (!BookExists(id)) {
                return NotFound();
            }

            await bookRepo.DeleteBook(id);
            await bookRepo.Save();

            return NoContent();
        }

        [HttpGet("{bookId}/return")]
        public async Task<ActionResult<Book>> ReturnBook(int bookId)
        {
            var book = await bookRepo.ReturnBook(bookId);

            if (book == null) { 
                return NotFound("Book not found");
            }

            await bookRepo.Save();
            return book;
        }

        private bool BookExists(int id) => bookRepo.BookExists(b => b.Id == id);
    }
}
