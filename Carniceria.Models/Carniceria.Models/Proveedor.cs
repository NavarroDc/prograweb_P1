using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carniceria.Models
{
    public class Proveedor
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe escribir el tipo de identificación...")]
        public string TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "Debe escribir la identificación...")]

    }
}
