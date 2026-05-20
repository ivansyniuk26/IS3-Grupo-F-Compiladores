using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificadosController : ControllerBase
    {
        private readonly CertificadoService _certificadoService;
        private readonly ApplicationDbContext _context;

        public CertificadosController(CertificadoService certificadoService, ApplicationDbContext context)
        {
            _certificadoService = certificadoService;
            _context = context;
        }

        // GET: api/certificados/descargar/{inscripcionId}
        [HttpGet("descargar/{inscripcionId}")]
        public async Task<IActionResult> DescargarPdf(Guid inscripcionId)
        {
            if (inscripcionId == Guid.Empty)
                return BadRequest("ID inválido.");

            // Aquí idealmente buscarías el nombre real en tu base de datos
            string nombreParticipante = "Estudiante Ejemplo"; 
            string evento = "Seminario II - Sistemas";

            // 1. Usar el servicio para dibujar el PDF (Issue 7)
            var pdfBytes = _certificadoService.GenerarCertificado(nombreParticipante, evento);

            // 2. Registrar la emisión en la base de datos (Issue 6 & 8)
            var registro = new CertificadoEmitido
            {
                Id = Guid.NewGuid(),
                InscripcionId = inscripcionId,
                FechaEmision = DateTime.UtcNow,
                CodigoVerificacion = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            };
            
            _context.CertificadosEmitidos.Add(registro);
            await _context.SaveChangesAsync();

            // 3. Devolver el archivo al navegador para su descarga
            return File(pdfBytes, "application/pdf", $"Certificado_{inscripcionId}.pdf");
        }
    }
}