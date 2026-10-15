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
    }
}
