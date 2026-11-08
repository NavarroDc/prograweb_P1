using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carniceria.Models
{
    public class Factura
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Generado automáticamente 
        public string NumeroFactura { get; set; }

        public DateTime FechaHoraEmision { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debe seleccionar un cliente...")]
        public Guid ClienteId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un empleado...")]
        public Guid EmpleadoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el método de pago...")]
        public string MetodoPago { get; set; }

        [Required(ErrorMessage = "Debe escribir el porcentaje de descuento...")]
        [Range(0, 25, ErrorMessage = "El porcentaje debe ser un valor entre 0 y 25")]
        public decimal PorcentajeDescuento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el estado de la factura...")]
        public string Estado { get; set; } = "Pendiente"; // Valor por defecto

        public List<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();

        // Cálculos automáticos
        public decimal Subtotal => Detalles.Sum(d => d.SubtotalLinea);

        public decimal MontoDescuento => (Subtotal * PorcentajeDescuento) / 100;

        public decimal BaseGravable => Subtotal - MontoDescuento;

        public decimal IVA => BaseGravable * 0.13m; // IVA del 13%

        public decimal Total => BaseGravable + IVA;
    }
}