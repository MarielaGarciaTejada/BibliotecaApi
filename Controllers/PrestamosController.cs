using BibliotecaApi.Models;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController(PrestamosService prestamosService, ILogger<PrestamosController> logger) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrestamoGet>>> GetAll()
        {
            logger.LogInformation("Lista de Prestamos");
            var listaPrestamos = await prestamosService.GetAllAsync();
            return Ok(listaPrestamos);
        }

        [HttpGet("{idPrestamo:int}")]
        public async Task<ActionResult<IEnumerable<PrestamoGet>>> GetById(int idPrestamo)
        {
            var prestamo = await prestamosService.GetByIdAsync(idPrestamo);

            if (prestamo == null)
            {
                return NotFound(new { message = $"No se encontró el Prestamo con Id {idPrestamo}" });
            }
            return Ok(prestamo);
        }

        [HttpPost]
        public async Task<ActionResult<PrestamoGet>> Create([FromBody] PrestamoSet prestamo)
        {
            try
            {
                var idNuevo = await prestamosService.CreateAsync(prestamo);
                var prestamoCreado = await prestamosService.GetByIdAsync(idNuevo);
                return CreatedAtAction(nameof(GetById), new { idPrestamo = idNuevo }, prestamoCreado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{idPrestamo:int}")]
        public async Task<IActionResult> Delete(int idPrestamo)
        {
            var eliminarPrestamo = await prestamosService.DeleteAsync(idPrestamo);
            if (!eliminarPrestamo)
                return NotFound(new { message = $"No se encontró el Préstamo con Id {idPrestamo} para eliminar" });

            return Ok(new { message = "Préstamo eliminado correctamente" });
        }
    }
}
