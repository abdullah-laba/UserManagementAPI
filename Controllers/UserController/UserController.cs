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

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            return Ok(_userServices.GetUser(null));
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(string id)
        {
            var list = _userServices.GetUser(id);
            if (list.Count == 0) return NotFound();
            return Ok(list[0]);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            if (user == null) return BadRequest();
            var created = _userServices.CreateUser(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] User user)
        {
            if (user == null) return BadRequest();
            var updated = _userServices.UpdateUser(id, user);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var removed = _userServices.DeleteUser(id);
            if (!removed) return NotFound();
            return NoContent();
        }
    }
}
