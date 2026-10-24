using BibliotecaApi.Models;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevolucionesController(DevolucionesService devolucionesService, ILogger<DevolucionesController> logger) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevolucionGet>>> GetAll()
        {
            logger.LogInformation("Lista de Devoluciones");
            var listaDev = await devolucionesService.GetAllAsync();
            return Ok(listaDev);
        }

        [HttpGet("{idDevolucion:int}")]
        public async Task<ActionResult<IEnumerable<DevolucionGet>>> GetById(int idDevolucion)
        {
            var devolucion = await devolucionesService.GetByIdAsync(idDevolucion);

            if (devolucion == null)
            {
                return NotFound(new { message = $"No se encontró la Devolución con Id {idDevolucion}" });
            }
            return Ok(devolucion);
        }

        [HttpPost]
        public async Task<ActionResult<DevolucionGet>> Create([FromBody] DevolucionSet devolucion)
        {
            try
            {
                var idNuevo = await devolucionesService.CreateAsync(devolucion);
                var devolucionCreado = await devolucionesService.GetByIdAsync(idNuevo);
                return CreatedAtAction(nameof(GetById), new { idDevolucion = idNuevo }, devolucionCreado);
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


        [HttpDelete("{idDevolucion:int}")]
        public async Task<IActionResult> Delete(int idDevolucion)
        {
            var eliminarDevolucion = await devolucionesService.DeleteAsync(idDevolucion);
            if (!eliminarDevolucion)
                return NotFound(new { message = $"No se encontró la Devolución con Id {idDevolucion} para eliminar" });

            return Ok(new { message = "Devolución eliminado correctamente" });
        }
    }
}
