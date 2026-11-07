using Carniceria.API.Datos;
using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;

namespace Carniceria.API.Controllers
{
    [Route("api/empleados")]
    [ApiController]
    public class EmpleadosControllerAPI : ControllerBase
    {
        [HttpGet]//1. Consultar empleados (lista completa)
        public ActionResult<IEnumerable<Empleado>> GetEmpleados()
        {
            return Ok(DatosMemoria.Empleados);
        }

        [HttpGet("buscar/{identificacion}")]//2. Buscar empleado por identificación
        public ActionResult<Empleado> BuscarEmpleadoString(string identificacion)
        {
            var empleado = DatosMemoria.Empleados.FirstOrDefault(e => e.Identificacion == identificacion);
            if (empleado == null) return NotFound();
            return Ok(empleado);
        }

        //2. Método para buscar empleado por Guid
        [HttpGet("{id:guid}")]
        public ActionResult<Empleado> GetEmpleadoPorId(Guid id)
        {
            var empleado = DatosMemoria.Empleados.FirstOrDefault(e => e.Id == id);
            if (empleado == null) return NotFound();
            return Ok(empleado);
        }

        [HttpPost]//3. Registrar empleado
        public ActionResult<Empleado> RegistrarEmpleado(Empleado empleadoNuevo)
        {
            bool existe = DatosMemoria.Empleados.Any(e => e.Identificacion == empleadoNuevo.Identificacion);
            if (existe)
            {
                return BadRequest("Ya existe un empleado con esta identificación");
            }
            empleadoNuevo.Id = Guid.NewGuid();
            DatosMemoria.Empleados.Add(empleadoNuevo);

            return CreatedAtAction(nameof(GetEmpleadoPorId), new { id = empleadoNuevo.Id }, empleadoNuevo);
        }

        [HttpPut("{id:guid}")]//4. Actualizar empleado
        public IActionResult ActualizarEmpleado(Guid id, Empleado empleadoActualizado)
        {
            if (id != empleadoActualizado.Id) return BadRequest("Los IDs no coinciden");

            bool existeOtro = DatosMemoria.Empleados.Any(e => e.Identificacion == empleadoActualizado.Identificacion && e.Id != empleadoActualizado.Id);
            if (existeOtro)
            {
                return BadRequest("Ya existe otro empleado con la identificación ingresada");
            }

            var index = DatosMemoria.Empleados.FindIndex(e => e.Id == id);
            if (index == -1) return NotFound();

            DatosMemoria.Empleados[index] = empleadoActualizado;
            return NoContent(); //204
        }

        [HttpDelete("{id:guid}")]
        public IActionResult EliminarEmpleado (Guid id)
        {
            var empleado = DatosMemoria.Empleados.FirstOrDefault(e => e.Id == id);
            if (empleado == null) return NotFound();

            DatosMemoria.Empleados.Remove(empleado);
            return NoContent();//204
        }
    }
}
