using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using WebApplication7.Repositories.Interfaces;

namespace WebApplication7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetDataAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetDataAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Login))
            {
                return BadRequest("UserName and Login are required");
            }

            try
            {
                await _userRepository.AddAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error creating user: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user)
        {
            try
            {
                await _userRepository.EditAsync(user);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error updating user: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error deleting user: {ex.Message}");
            }
        }
    }
}