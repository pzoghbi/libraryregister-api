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
    public class MembershipsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public MembershipsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Memberships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Membership>>> GetMembership()
        {
            return await _context.Membership.ToListAsync();
        }

        // GET: api/Memberships/{userId]
        // Gets the latest membership for user
        [HttpGet("{userId}")]
        public async Task<ActionResult<Membership>> GetMembership(int userId)
        {
            var memberShip = await _context.Membership
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.ValidUntil)
                .FirstOrDefaultAsync();

            if (memberShip == null) 
            {
                return NoContent();
            }

            return memberShip;
        }

        // PUT: api/Memberships/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembership(Guid id, Membership memberShip)
        {
            if (id != memberShip.Guid)
            {
                return BadRequest();
            }

            _context.Entry(memberShip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipExists(id))
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

        // POST: api/Memberships
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Membership>> PostMembership(Membership membership)
        {
            Console.WriteLine("Hello");
            if (_context.Membership.Any(m => 
                m.UserId == membership.UserId && 
                m.ValidUntil > DateTime.UtcNow)) {
                return new StatusCodeResult(Services.LibraryStatusCodes.ActiveMembershipExists);
            }

            _context.Membership.Add(membership);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMembership", new { id = membership.Guid }, membership);
        }

        // DELETE: api/Memberships/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembership(Guid id)
        {
            var memberShip = await _context.Membership.FindAsync(id);
            if (memberShip == null)
            {
                return NotFound();
            }

            _context.Membership.Remove(memberShip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MembershipExists(Guid id)
        {
            return _context.Membership.Any(e => e.Guid == id);
        }
    }
}
