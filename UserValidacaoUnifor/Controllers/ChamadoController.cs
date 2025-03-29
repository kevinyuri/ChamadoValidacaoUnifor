using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

            var mensagem = new
            {
                Id = chamado.Id,
                NomeSolicitante = chamado.NomeSolicitante,
                Setor = chamado.Setor,
                Mensagem = chamado.Mensagem
            };

            var mensagemJson = JsonConvert.SerializeObject(mensagem);

            await _rabbitMQService.SendMessageChamadoAsync(mensagemJson);

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
                return NotFound(new { error = "Chamado não encontrado." });
            }

            return Ok(chamados);
        }

        [HttpGet("primeiro-chamado")]
        public async Task<ActionResult<Chamado>> GetPrimeiroChamadoDaFila()
        {
            var chamado = await _context.Chamados
                            .Where(c => c.Status == "Pendente")
                            .OrderBy(c => c.Id)
                            .FirstOrDefaultAsync();

            if (chamado == null)
            {
                return NotFound(new { error = "Chamado não encontrado." });
            }

            return Ok(chamado);
        }

        [HttpPost("resolver-chamado/{chamadoId}")]
        public async Task<IActionResult> ResolverChamado(int chamadoId)
        {
            var chamado = await _context.Chamados.FindAsync(chamadoId);

            if (chamado == null)
            {
                return NotFound("Chamado não encontrado.");
            }

            chamado.Status = "Resolvido";
            _context.Chamados.Update(chamado);
            _context.SaveChanges();

            // Consumir a mensagem da fila RabbitMQ
            var resultado = await _rabbitMQService.ResolveItemQueue();


            return Ok(new { message = "Chamado resolvido com sucesso e item removido da fila." }); 


        }
    }
}

