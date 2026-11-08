using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;
using Carniceria.API.Datos;

namespace Carniceria.API.Controllers
{
    [Route("api/facturas")]
    [ApiController]
    public class FacturasControllerAPI : ControllerBase
    {
        // 1. Consultar todas las facturas
        [HttpGet]
        public ActionResult<IEnumerable<Factura>> GetFacturas()
        {
            return Ok(DatosMemoria.Facturas);
        }

        //Buscar factura mediante ID interno
        [HttpGet("{id:guid}")]
        public ActionResult<Factura> GetFacturaPorId(Guid id)
        {
            var factura = DatosMemoria.Facturas.FirstOrDefault(f => f.Id == id);
            if (factura == null) return NotFound();
            return Ok(factura);
        }

        // 2. Buscar factura por Número de Factura o Identificación del Cliente
        [HttpGet("buscar/{termino}")]
        public ActionResult<IEnumerable<Factura>> BuscarFactura(string termino)
        {
            // Se buscan coincidencias por num de factura o identificación del cliente
            var facturasEncontradas = DatosMemoria.Facturas.Where(f =>
                f.NumeroFactura.Equals(termino, StringComparison.OrdinalIgnoreCase) ||
                DatosMemoria.Clientes.Any(c => c.Id == f.ClienteId && c.Identificacion == termino)
            ).ToList();

            if (!facturasEncontradas.Any()) return NotFound();

            return Ok(facturasEncontradas);
        }

        // 3. Registrar nueva factura---------------------------------------
        [HttpPost]
        public ActionResult<Factura> RegistrarFactura(Factura nuevaFactura)
        {
            // Validar que venga al menos una línea de detalle
            if (nuevaFactura.Detalles == null || !nuevaFactura.Detalles.Any())
            {
                return BadRequest("La factura debe contener como mínimo una línea de detalle");
            }

            // Recorrer cada línea para validar inventario y asignar precios oficiales
            foreach (var detalle in nuevaFactura.Detalles)
            {
                var productoDb = DatosMemoria.Productos.FirstOrDefault(p => p.Id == detalle.ProductoId);
                if (productoDb == null)
                {
                    return BadRequest("Uno de los productos seleccionados no existe en el inventario");
                }

                // No permitir registrar cantidad superior a la existencia
                if (detalle.Cantidad > productoDb.ExistenciaActual)
                {
                    return BadRequest($"La cantidad solicitada para '{productoDb.NombreProducto}' supera la existencia actual ({productoDb.ExistenciaActual} disponibles).");
                }

                // Datos automáticos de la línea para evitar manipulación manual
                detalle.IdentificadorDetalle = Guid.NewGuid();
                detalle.PrecioUnitario = productoDb.PrecioUnidadMedida;
                detalle.UnidadMedida = productoDb.UnidadMedida;
            }

            // Genera datos automáticos 
            nuevaFactura.Id = Guid.NewGuid();
            // Genera un consecutivo automático
            nuevaFactura.NumeroFactura = $"FAC-{(DatosMemoria.Facturas.Count + 1).ToString("D4")}";
            nuevaFactura.FechaHoraEmision = DateTime.Now;
            nuevaFactura.Estado = "Pendiente";

            // Guardar en la memoria
            DatosMemoria.Facturas.Add(nuevaFactura);

            return CreatedAtAction(nameof(GetFacturaPorId), new { id = nuevaFactura.Id }, nuevaFactura);
        }

        // 4. Actualizar factura (y manejo de inventario)----------------------------------------
        [HttpPut("{id:guid}")]
        public IActionResult ActualizarFactura(Guid id, Factura facturaActualizada)
        {
            if (id != facturaActualizada.Id) return BadRequest("Los IDs de la factura no coinciden");

            var index = DatosMemoria.Facturas.FindIndex(f => f.Id == id);
            if (index == -1) return NotFound();

            var facturaOriginal = DatosMemoria.Facturas[index];

            // Una factura anulada no se puede modificar
            if (facturaOriginal.Estado == "Anulada")
            {
                return BadRequest("Una factura en estado Anulada no puede ser modificada");
            }

            // --- LÓGICA DE INVENTARIO SEGÚN ESTADOS ---

            // Caso A: Se paga la factura por primera vez
            if (facturaOriginal.Estado == "Pendiente" && facturaActualizada.Estado == "Pagada")
            {
                foreach (var detalle in facturaActualizada.Detalles)
                {
                    var producto = DatosMemoria.Productos.FirstOrDefault(p => p.Id == detalle.ProductoId);
                    if (producto != null) producto.ExistenciaActual -= detalle.Cantidad; // Se descuenta del stock
                }
            }
            // Caso B: Se edita una factura que ya estaba pagada
            else if (facturaOriginal.Estado == "Pagada" && facturaActualizada.Estado == "Pagada")
            {
                // 1. Se reintegran las cantidades originales al inventario
                foreach (var detOriginal in facturaOriginal.Detalles)
                {
                    var prod = DatosMemoria.Productos.FirstOrDefault(p => p.Id == detOriginal.ProductoId);
                    if (prod != null) prod.ExistenciaActual += detOriginal.Cantidad;
                }

                // 2. Se descuentan las nuevas cantidades
                foreach (var detNuevo in facturaActualizada.Detalles)
                {
                    var prod = DatosMemoria.Productos.FirstOrDefault(p => p.Id == detNuevo.ProductoId);
                    if (prod != null) prod.ExistenciaActual -= detNuevo.Cantidad;
                }
            }
            // Caso C: Se anula una factura que ya estaba pagada
            else if (facturaOriginal.Estado == "Pagada" && facturaActualizada.Estado == "Anulada")
            {
                foreach (var detalle in facturaOriginal.Detalles)
                {
                    var producto = DatosMemoria.Productos.FirstOrDefault(p => p.Id == detalle.ProductoId);
                    if (producto != null) producto.ExistenciaActual += detalle.Cantidad; // Se devuelve al stock
                }
            }

            // Se protegen campos autogenerados para que no los alteren desde el cliente Web
            facturaActualizada.NumeroFactura = facturaOriginal.NumeroFactura;
            facturaActualizada.FechaHoraEmision = facturaOriginal.FechaHoraEmision;

            // Actualiza la memoria
            DatosMemoria.Facturas[index] = facturaActualizada;
            return NoContent();
        }

        // 5. Eliminar factura físicamente
        [HttpDelete("{id:guid}")]
        public IActionResult EliminarFactura(Guid id)
        {
            var factura = DatosMemoria.Facturas.FirstOrDefault(f => f.Id == id);
            if (factura == null) return NotFound();

            // Solo se pueden eliminar facturas en estado Pendiente
            if (factura.Estado != "Pendiente")
            {
                return BadRequest("Únicamente las facturas en estado Pendiente pueden ser eliminadas físicamente. Las demás deben ser Anuladas");
            }

            DatosMemoria.Facturas.Remove(factura);
            return NoContent();
        }
    }
}