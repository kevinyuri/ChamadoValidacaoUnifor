using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserValidacaoUnifor.Data;
using UserValidacaoUnifor.Models;

namespace SeuProjeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            // Verifica se o usuário já existe
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return Conflict("O usuário com este e-mail já está cadastrado.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
