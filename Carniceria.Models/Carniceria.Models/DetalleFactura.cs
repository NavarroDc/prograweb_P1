using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carniceria.Models
{
    public class DetalleFactura
    {
        [Key]
        public Guid IdentificadorDetalle { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Debe seleccionar el producto del inventario...")]
        public Guid ProductoId { get; set; }

        public string UnidadMedida { get; set; }

        [Required(ErrorMessage = "Debe escribir la cantidad...")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0...")]
        public decimal Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        // Cálculo automático
        public decimal SubtotalLinea => Cantidad * PrecioUnitario;

        public Producto ProductoAsociado { get; set; }
    }
}