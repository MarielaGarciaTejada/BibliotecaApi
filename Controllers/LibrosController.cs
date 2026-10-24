using BibliotecaApi.Models;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController(LibrosService librosService, ILogger<LibrosController> logger) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroGet>>> GetAll()
        {
            logger.LogInformation("Catálogo de Libros");
            var lista = await librosService.GetAllAsync();
            return Ok(lista);
        }

        [HttpGet("{idLibro:int}")]
        public async Task<ActionResult<IEnumerable<LibroGet>>> GetById(int idLibro)
        {
            var libro = await librosService.GetByIdAsync(idLibro);

            if (libro == null)
            {
                return NotFound(new { message = $"No se encontró el Libro con Id {idLibro}" });
            }
            return Ok(libro);
        }

        [HttpPost]
        public async Task<ActionResult<LibroGet>> Create([FromBody] LibroSet libro)
        {
            var idNuevo = await librosService.CreateAsync(libro);
            var libroCreado = await librosService.GetByIdAsync(idNuevo);
            return CreatedAtAction(nameof(GetById), new { idLibro = idNuevo }, libroCreado);
        }

        [HttpPut("{idLibro:int}")]
        public async Task<ActionResult> Update(int idLibro, [FromBody] LibroSet libro)
        {
            var actualizarLibro = await librosService.UpdateAsync(idLibro, libro);
            if (!actualizarLibro)
            {
                return NotFound(new { message = $"No se encontró el Libro con Id {idLibro} para actualizar" });
            }
            return Ok(new { message = "Libro actualizado correctamente" });
        }

        [HttpDelete("{idLibro:int}")]
        public async Task<IActionResult> Delete(int idLibro)
        {
            var eliminarLibro = await librosService.DeleteAsync(idLibro);
            if (!eliminarLibro)
                return NotFound(new { message = $"No se encontró el Libro con Id {idLibro} para eliminar" });

            return Ok(new { message = "Libro eliminado correctamente" });
        }
    }
}
