using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;
using Carniceria.API.Datos;

namespace Carniceria.API.Controllers
{
    [Route("api/proveedores")]
    [ApiController]
    public class ProveedoresControllerAPI : ControllerBase
    {
        [HttpGet]//1. Consultar proveedores
        public ActionResult<IEnumerable<Proveedor>> GetProveedores()
        {
            return Ok(DatosMemoria.Proveedores);
        }

        [HttpGet("buscar/{termino}")]//2. Buscar proveedores
        public ActionResult<IEnumerable<Proveedor>> BuscarProveedor(string termino)
        {
           //Coincidencias por identificación o razón social
            var proveedoresEncontrados = DatosMemoria.Proveedores
                .Where(p => p.Identificacion == termino ||
                            p.RazonSocial.Contains(termino, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!proveedoresEncontrados.Any())
            {
                return NotFound();
            }

            return Ok(proveedoresEncontrados);
        }

        //Buscar proveedor por Guid
        [HttpGet("{id:guid}")]
        public ActionResult<Proveedor> GetProveedorPorId(Guid id)
        {
            var proveedor = DatosMemoria.Proveedores.FirstOrDefault(p => p.Id == id);
            if (proveedor == null) return NotFound();
            return Ok(proveedor);
        }

        
        [HttpPost]//3. Registrar proveedores
        public ActionResult<Proveedor> RegistrarProveedor(Proveedor proveedorNuevo)
        {
            bool existe = DatosMemoria.Proveedores.Any(p => p.Identificacion == proveedorNuevo.Identificacion);
            if (existe)
            {
                return BadRequest("Ya existe un proveedor con esa identificación.");
            }

            proveedorNuevo.Id = Guid.NewGuid();
            DatosMemoria.Proveedores.Add(proveedorNuevo);

            return CreatedAtAction(nameof(GetProveedorPorId), new { id = proveedorNuevo.Id }, proveedorNuevo);
        }

        [HttpPut("{id:guid}")]//4. Actualizar proveedores
        public IActionResult ActualizarProveedor(Guid id, Proveedor proveedorActualizado)
        {
            if (id != proveedorActualizado.Id) return BadRequest("Los IDs no coinciden.");

           //Identificación actualizada no puede pertenecer a otro proveedor
            bool existeOtro = DatosMemoria.Proveedores.Any(p => p.Identificacion == proveedorActualizado.Identificacion && p.Id != proveedorActualizado.Id);
            if (existeOtro)
            {
                return BadRequest("Ya existe otro proveedor con esa identificación.");
            }

            var index = DatosMemoria.Proveedores.FindIndex(p => p.Id == id);
            if (index == -1) return NotFound();

            DatosMemoria.Proveedores[index] = proveedorActualizado;
            return NoContent();
        }

        [HttpDelete("{id:guid}")]//5. Eliminar proveedores
        public IActionResult EliminarProveedor(Guid id)
        {
            var proveedor = DatosMemoria.Proveedores.FirstOrDefault(p => p.Id == id);
            if (proveedor == null) return NotFound();

            DatosMemoria.Proveedores.Remove(proveedor);
            return NoContent();
        }
    }
}