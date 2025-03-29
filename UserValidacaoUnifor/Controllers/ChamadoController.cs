using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserValidacaoUnifor.Data;
using UserValidacaoUnifor.Models;
using UserValidacaoUnifor.RabbitMQ;

namespace UserValidacaoUnifor.Controllers
{   
    [Route("api/[controller]")]
    [ApiController] 
    public class ChamadoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RabbitMQService _rabbitMQService;

        public ChamadoController(AppDbContext context, RabbitMQService rabbitMQService)
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
        }

        // POST: api/Chamado
        [HttpPost]
        public async Task<ActionResult<Chamado>> CriarChamado(Chamado chamado)
        {

            _context.Chamados.Add(chamado);
            await _context.SaveChangesAsync();

            var mensagem = $"Novo chamado de TI: {chamado.NomeSolicitante}, Setor: {chamado.Setor}, Mensagem: {chamado.Mensagem}";
            await _rabbitMQService.SendMessageChamadoAsync(mensagem);

            return CreatedAtAction(nameof(GetChamado), new { id = chamado.Id }, chamado);
        }

        // GET: api/Chamado/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chamado>> GetChamado(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);

            if (chamado == null)
            {
                return NotFound();
            }

            return chamado;
        }

        // GET: api/Chamado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chamado>>> GetAllChamados()
        {
            var chamados = await _context.Chamados.ToListAsync();

            if (chamados == null || !chamados.Any())
            {
                return NotFound("Nenhum chamado encontrado.");
            }

            return Ok(chamados);
        }
    }
}

