using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcreditacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AcreditacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/acreditaciones (Resuelve el Issue 2)
        [HttpPost]
        public async Task<IActionResult> RegistrarAcreditacion([FromBody] Acreditacion request)
        {
            if (request.InscripcionId == Guid.Empty)
                return BadRequest("El ID de inscripción es obligatorio.");

            // Generamos un nuevo ID y registramos la hora exacta de entrada
            request.Id = Guid.NewGuid();
            request.FechaHoraIngreso = DateTime.UtcNow;

            _context.Acreditaciones.Add(request);
            await _context.SaveChangesAsync();

            return Ok(request);
        }

        // GET: api/acreditaciones/buscar?termino=xyz (Resuelve el Issue 3)
        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarParticipante([FromQuery] string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return BadRequest("Ingrese un término de búsqueda válido.");

            // Buscamos cualquier acreditación que coincida con el término
            var resultados = await _context.Acreditaciones
                .Where(a => a.InscripcionId.ToString().Contains(termino))
                .ToListAsync();

            return Ok(resultados);
        }
    }
}