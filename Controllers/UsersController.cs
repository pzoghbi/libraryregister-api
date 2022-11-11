using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryRegister.Models;
using MySql.Data.MySqlClient;
using LibraryRegister.DAL;

namespace LibraryRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IUserRepository userRepo;
        public UsersController(LibraryDbContext dbContext) {
            userRepo = new UserRepository(dbContext);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] string name)
        {
            var users = await userRepo.GetMatchingUsers(name);
            if (users == null) { return NoContent(); }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await userRepo.FindById(id);
            if (user == null) {
                return NotFound("User not found");
            }
            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id) {
                return BadRequest();
            }

            userRepo.UpdateUser(id, user);

            try {
                await userRepo.Save();
            }
            catch (DbUpdateConcurrencyException) {
                if (!UserExists(id)) {
                    return NotFound(); }
                else throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromForm] User user)
        {
            // Todo handle duplicate email
            await userRepo.InsertUser(user);
            return CreatedAtAction(
                nameof(GetUser), 
                new { id = user.Id }, 
                user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await userRepo.DeleteUser(id);
            return NoContent();
        }

        private bool UserExists(int id) 
            => userRepo.UserExists(e => e.Id == id);
    }
}
