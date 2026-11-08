using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;
using Carniceria.API.Datos;


namespace Carniceria.API.Controllers
{
    [Route("api/productos")]
    [ApiController]
    public class ProductosControllerAPI : ControllerBase
    {
        [HttpGet]//1. Consultar productos
        public ActionResult<IEnumerable<Producto>> GetProductos()
        {
            return Ok(DatosMemoria.Productos);
        }

        [HttpGet("buscar/{termino}")]//2. Buscar producto por código o nombre
        public ActionResult<IEnumerable<Producto>> BuscarProducto(string termino)
        {
            var productosEncontrados = DatosMemoria.Productos
                .Where(p => p.CodigoProducto == termino || p.NombreProducto.Contains(
                    termino, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!productosEncontrados.Any())
            {
                return NotFound();
            }
            return Ok(productosEncontrados);
        }

        [HttpGet("{id:guid}")]//2. Buscar por Guid
        public ActionResult<Producto> GetProductoPorId(Guid id)
        {
            var producto = DatosMemoria.Productos.FirstOrDefault(p => p.Id == id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPost]//3. Registrar producto
        public ActionResult<Producto> RegistrarProducto(Producto productoNuevo)
        {
            //Validar que no se asocie a un proveedor que no exista
            bool existeProveedor = DatosMemoria.Proveedores.Any(p => p.Id == productoNuevo.ProveedorId);
            if (!existeProveedor)
            {
                return BadRequest("El proveedor seleccionado no existe...");
            }
            //Validar que no se repita el código del producto
            bool existeCodigo = DatosMemoria.Productos.Any(p => p.CodigoProducto == productoNuevo.CodigoProducto);
            if (existeCodigo)
            {
                return BadRequest("Ya existe un producto registrado con ese código...");
            }
            productoNuevo.Id = Guid.NewGuid();
            DatosMemoria.Productos.Add(productoNuevo);

            return CreatedAtAction(nameof(GetProductoPorId), new { id = productoNuevo.Id }, productoNuevo);
        }

        [HttpPut("{id:guid}")]//4. Actualizar producto
        public IActionResult ActualizarProducto(Guid id, Producto productoActualizado)
        {
            if (id != productoActualizado.Id) return BadRequest("Los IDs no coinciden");

            //Validar que el proveedor existe
            bool existeProveedor = DatosMemoria.Proveedores.Any(p => p.Id == productoActualizado.ProveedorId);
            if (!existeProveedor)
            {
                return BadRequest("El proveedor seleccionado no existe...");
            }
            //Validar que el nuevo código no lo tenga otro producto
            bool existeOtroCodigo = DatosMemoria.Productos.Any(p => p.CodigoProducto == productoActualizado.CodigoProducto && p.Id != id);
            if (existeOtroCodigo)
            {
                return BadRequest("Ya existe otro producto con el código ingresado");
            }
            var index = DatosMemoria.Productos.FindIndex(p => p.Id == id);
            if (index == -1) return NotFound();

            DatosMemoria.Productos[index] = productoActualizado;
            return NoContent();
        }

        [HttpDelete("{id:guid}")]//5. Eliminar producto
        public IActionResult EliminarProducto(Guid id)
        {
            var producto = DatosMemoria.Productos.FirstOrDefault(p => p.Id == id);
            if (producto == null) return NotFound();

            DatosMemoria.Productos.Remove(producto);
            return NoContent();
        }
    }
}