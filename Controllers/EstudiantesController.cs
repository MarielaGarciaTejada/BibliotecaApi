using BibliotecaApi.Models;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController(EstudiantesService estudiantesService, ILogger<EstudiantesController> logger) : Controller
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudianteGet>>> GetAll()
        {
            logger.LogInformation("Lista de los estudiantes");
            var lista = await estudiantesService.GetAllAsync();
            return Ok(lista);
        }

        [HttpGet("{idEstudiante:int}")]
        public async Task<ActionResult<IEnumerable<EstudianteGet>>> GetById(int idEstudiante)
        {
            var estudiante = await estudiantesService.GetByIdAsync(idEstudiante);

            if (estudiante == null)
            {
                return NotFound(new { message = $"No se encontró el estudiante con ID {idEstudiante}" });
            }
            return Ok(estudiante);
        }

        [HttpPost]
        public async Task<ActionResult<EstudianteGet>> Create([FromBody] EstudianteSet estudiante)
        {
            var idNuevo = await estudiantesService.CreateAsync(estudiante);
            var estudianteCreado = await estudiantesService.GetByIdAsync(idNuevo);
            return CreatedAtAction(nameof(GetById), new { idEstudiante = idNuevo }, estudianteCreado);
        }

        [HttpPut("{idEstudiante:int}")]
        public async Task<ActionResult> Update(int idEstudiante, [FromBody] EstudianteSet estudiante)
        {
            var actualizarEst = await estudiantesService.UpdateAsync(idEstudiante, estudiante);
            if (!actualizarEst)
            {
                return NotFound(new { message = $"No se encontró el estudiante con ID {idEstudiante} para actualizar" });
            }
            return Ok(new {message = "Estudiante actualizado correctamente"});
        }

        [HttpDelete("{idEstudiante:int}")]
        public async Task<IActionResult> Delete(int idEstudiante)
        {
            var eliminarEst = await estudiantesService.DeleteAsync(idEstudiante);
            if (!eliminarEst)
                return NotFound(new { message = $"No se encontró el estudiante con ID {idEstudiante} para eliminar" });

            return Ok(new { message = "Estudiante eliminado correctamente" });
        }


    }
}
