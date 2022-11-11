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
    public class MembershipsController : ControllerBase
    {
        readonly IMembershipRepository membershipRepo;

        public MembershipsController(LibraryDbContext context)
        {
            membershipRepo = new MembershipRepository(context);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Membership>> GetUserMembership(int userId)
        {
            var memberShip = await membershipRepo.FindByUserId(userId);

            if (memberShip == null) {
                return NoContent();
            }

            return memberShip;
        }

        [HttpGet("{guid}")]
        public async Task<ActionResult<Membership>> GetMembership(Guid guid)
        {
            var memberShip = await membershipRepo.FindByGuid(guid);

            if (memberShip == null)
            {
                return NoContent();
            }

            return memberShip;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembership(Guid guid, Membership memberShip)
        {
            if (guid != memberShip.Guid)
            {
                return BadRequest();
            }

            await membershipRepo.UpdateMembership(memberShip);

            try
            {
                await membershipRepo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipExists(guid))
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

        [HttpPost]
        public async Task<ActionResult<Membership>> PostMembership(Membership membership)
        {
            if (membershipRepo.MembershipExists(m => 
                m.UserId == membership.UserId && 
                m.ValidUntil > DateTime.UtcNow)
            ) {
                return new StatusCodeResult(Services.LibraryStatusCodes.ActiveMembershipExists);
            }
            
            await membershipRepo.InsertMembership(membership);
            await membershipRepo.Save();

            return CreatedAtAction(nameof(GetMembership), new { guid = membership.Guid }, membership);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembership(Guid guid)
        {
            if (!MembershipExists(guid))
            {
                return NotFound();
            }

            await membershipRepo.DeleteMembership(guid);
            await membershipRepo.Save();

            return NoContent();
        }

        private bool MembershipExists(Guid guid) => 
            membershipRepo.MembershipExists(m => m.Guid == guid);
    }
}
