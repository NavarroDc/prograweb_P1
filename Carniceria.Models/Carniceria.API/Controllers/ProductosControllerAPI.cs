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
    }
}
