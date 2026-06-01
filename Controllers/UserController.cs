using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;

namespace UserManagementAPI.Controllers
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
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<User> GetUserById(Guid id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] CreateUserDto dto)
        {
            if (_userRepository.EmailExists(dto.Email))
            {
                return Conflict(new { message = "A user with this email already exists." });
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Department = dto.Department,
                IsActive = dto.IsActive,
                CreatedAtUtc = DateTime.UtcNow
            };

            _userRepository.Create(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            var existingUser = _userRepository.GetById(id);

            if (existingUser == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            if (_userRepository.EmailExists(dto.Email, id))
            {
                return Conflict(new { message = "Another user with this email already exists." });
            }

            var updatedUser = new User
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Department = dto.Department,
                IsActive = dto.IsActive,
                CreatedAtUtc = existingUser.CreatedAtUtc
            };

            var updated = _userRepository.Update(id, updatedUser);

            if (!updated)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            var deleted = _userRepository.Delete(id);

            if (!deleted)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            return NoContent();
        }
    }
}