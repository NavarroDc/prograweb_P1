using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carniceria.Models
{
    public class Producto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe escribir el código del producto...")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "El código debe tener entre 1 y 20 caracteres")]
        public string CodigoProducto { get; set; }

        [Required(ErrorMessage = "Debe escribir el nombre del producto...")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre del producto debe tener entre 3 y 80 caracteres")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la categoría del producto...")]
        public string Categoria {  get; set; }

        [Required(ErrorMessage = "Debe seleccionar la unidad de medida del producto...")]
        public string UnidadMedidad { get; set; }

        [Required(ErrorMessage = "Debe escribir el precio de la unidad de medida...")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0 o un valor válido...")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = false)]
        public decimal PrecioUnidadMedida { get; set; }

        [Required(ErrorMessage = "Debe escribir la existencia actual del producto...")]
        [Range(0, double.MaxValue, ErrorMessage = "La existencia actual debe ser igual o mayor a cero...")]
        public decimal ExistenciaActual {  get; set; }

        [Required(ErrorMessage = "Debe escribir la existencia mínima del producto...")]
        [Range(0, double.MaxValue, ErrorMessage = "La existencia mínima debe ser igual o mayor a cero...")]
        public decimal ExistenciaMinima { get; set; }






    }
}
