using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;
        private readonly ILogger<UserController> _logger;

        public UserController(UserServices userServices, ILogger<UserController> logger)
        {
            _userServices = userServices;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            _logger.LogInformation("Retrieving all users");
            return Ok(_userServices.GetUser(null));
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(string id)
        {
            var list = _userServices.GetUser(id);
            if (list.Count == 0)
            {
                _logger.LogInformation("User not found {UserId}", id);
                return NotFound();
            }

            return Ok(list[0]);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            if (user == null) return BadRequest();
            var created = _userServices.CreateUser(user);
            _logger.LogInformation("Created user {UserId}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] User user)
        {
            if (user == null) return BadRequest();
            var updated = _userServices.UpdateUser(id, user);
            if (!updated) return NotFound();
            _logger.LogInformation("Updated user {UserId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var removed = _userServices.DeleteUser(id);
            if (!removed) return NotFound();
            _logger.LogInformation("Deleted user {UserId}", id);
            return NoContent();
        }
    }
}
