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
    public class LeasingsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public LeasingsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Leasings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leasing>>> GetLeasing()
        {
            return await _context.Leasing.ToListAsync();
        }

        // GET: api/Leasings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Leasing>> GetLeasing(int id)
        {
            var leasing = await _context.Leasing
                .Where(l => l.Id == id)
                .Include(l => l.Book)
                .ThenInclude(b => b!.Author)
                .FirstOrDefaultAsync();

            if (leasing == null) return NotFound();

            return leasing;
        }

        // PUT: api/Leasings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeasing(int id, Leasing leasing)
        {
            if (id != leasing.Id)
            {
                return BadRequest();
            }

            _context.Entry(leasing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeasingExists(id))
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

        // POST: api/Leasings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Leasing>> LeaseBook([FromBody] Leasing leasing)
        {
            var user = await _context.User.FindAsync(leasing.UserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var membership = await _context.Membership
                .Where(m => m.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (membership == null)
			{
                return new ObjectResult(
                    new {
                        statusCode = 123, 
                        statusText = "No membership for user"
                    }
                );
			}

            var book = _context.Book
                .Where(b => b.Id == leasing.BookId)
                .Include(b => b.Author)
                .FirstOrDefault();

            if (book == null) return NotFound();

            // Todo respond better
            if (book != null && !book.IsAvailable) {
                return BadRequest("Book not available");
			}

            int activeLeasings = _context.Leasing
                .Where(l =>
                    l.UserId == leasing.UserId &&
                    l.ReturnDate == default
                )
                .Count();

            // Todo make administrative constants (etc: LeasesPerUserLimit = 3)
            // todo response better
            if (activeLeasings + 1 > 3) {
                return Ok(Services.LibraryStatusCodes.LeasingsLimitExceeded);
            }


            // Check active membership

            book!.IsAvailable = false;
            _context.Leasing.Add(leasing);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLeasing), new { id = leasing.Id }, leasing);
        }

        // POST: api/leasings/5/return
        [HttpGet("{id}/return")]
        public async Task<ActionResult<Leasing>> ReturnBook(int id)
        {
            var leasing = await _context.Leasing
                .Where(l => l.Id == id)
                .Include(l => l.Book!.Author)
                .FirstOrDefaultAsync();

            // Todo respond better
            if (leasing == null) return Ok("Leasing entity not found");

            var book = _context.Book.Find(leasing.BookId);

            if (book == null)       return Ok("Book not found");
            if (book.IsAvailable)   return Ok("Book already returned");

            book.IsAvailable = true;
            leasing.ReturnDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return leasing;
        }

        // DELETE: api/Leasings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeasing(int id)
        {
            var leasing = await _context.Leasing.FindAsync(id);
            if (leasing == null) {
                return NotFound();
            }
            
            _context.Leasing.Remove(leasing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeasingExists(int id)
        {
            return _context.Leasing.Any(e => e.Id == id);
        }
    }
}
