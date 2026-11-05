using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carniceria.Models
{
    public class DetalleFactura
    {
        [Key]
        public string IdetificadorDetalle { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el producto del inventario...")]
        public string ProductoId { get; set; }

        public string UnidadMedida { get; set; }

        [Required(ErrorMessage = "Debe escribir la cantidad...")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0...")]
        public decimal Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal SubtotalLinea => Cantidad * PrecioUnitario;


    }
}
