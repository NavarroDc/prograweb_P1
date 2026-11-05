using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carniceria.Models
{
    public class Factura
    {
        [Key] //Generado automáticamente
        public string NumeroFactura { get; set; }

        public DateTime FechaHoraEmision { get; set; } = DateTime.Now;

        [Required]
        public string ClienteId { get; set; }

        [Required]
        public string EmpleadoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el método de pago...")]
        public string MetodoPago { get; set; }

        [Required(ErrorMessage = "Debe escribir el porcentaje de descuento...")]
        [Range(0, 25, ErrorMessage = "El porcentaje debe ser un valor entre 0 y 25")]
        public decimal PorcentajeDescuento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el estado de la factura...")]
        public string Estado {  get; set; }

        [MinLength(1, ErrorMessage = "La factura debe contener al menos 1 línea de detalle...")]
        public List<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();

        //Cálculos autumáticos
        public decimal Subtotal => Detalles.Sum(d => d.SubtotalLinea);//Suma subtotales todas las líneas de detalle
        public decimal MontoDescuento => (Subtotal * PorcentajeDescuento) / 100;
        public decimal BaseGravable => Subtotal - MontoDescuento;
        public decimal IVA => BaseGravable * 0.13m; //Se le aplica el IVA de 13%
        public decimal Total => BaseGravable + IVA;
    }
}
