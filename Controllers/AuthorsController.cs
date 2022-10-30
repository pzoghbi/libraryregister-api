using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryRegister.Models;
using LibraryRegister.DAL;
using LibraryRegister.Services;
using LibraryRegister.DTOs;

namespace LibraryRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        readonly IAuthorRepository authorRepo;

        public AuthorsController(LibraryDbContext context)
        {
            authorRepo = new AuthorRepository(context);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<Author>>> GetAuthorsList(int pageIndex = 1, int pageSize = 5)
        {
            var paginatedList = await authorRepo.GetAuthorsList(pageIndex, pageSize);
            return paginatedList;
        }

		[HttpGet("search")]
		public async Task<ActionResult<IEnumerable<Author>>> SearchAuthors(string name = "")
		{
            var authors = await authorRepo.GetMatchingAuthors(name);
            return Ok(authors);
		}

		[HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await authorRepo.FindById(id);

            if (author == null) {
                return NotFound();
            }

            return author;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.Id) {
                return BadRequest();
            }

            authorRepo.UpdateAuthor(id, author);

            try {
                await authorRepo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id)) {
                    return NotFound();
                }
                else { 
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor([FromForm] Author author)
        {
            if (authorRepo.AuthorExists(a => a.Name == author.Name)) {
                return StatusCode(LibraryStatusCodes.DuplicateAuthor);
			}

            await authorRepo.InsertAuthor(author);
            await authorRepo.Save();

            return CreatedAtAction(
                nameof(GetAuthor), 
                new { id = author.Id }, 
                author
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await authorRepo.FindById(id);
            if (author == null) {
                return NotFound();
            }

            await authorRepo.DeleteAuthor(id);
            await authorRepo.Save();

            return NoContent();
        }

        private bool AuthorExists(int id) 
            => authorRepo.AuthorExists(a => a.Id == id);
    }
}
