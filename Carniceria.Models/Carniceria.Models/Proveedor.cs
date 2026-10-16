using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carniceria.Models
{
    public class Proveedor
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el tipo de identificación...")]
        public string TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "Debe escribir la identificación...")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Debe escribir la razón social o nombre comercial...")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La razón social debe tener entre 3 y 100 caracteres...")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "Debe escribir el nombre de la persona de contacto...")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del contacto debe tener entre 3 y 100 caracteres...")]
        public string PersonaContacto { get; set; }

        [Required(ErrorMessage = "Debe escribir el número de teléfono...")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El número de teléfono debe tener 8 dígitos...")]
        public string Telefono {  get; set; }

        [Required(ErrorMessage = "Debe escribir el correo electrónico...")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido...")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el tipo de suministro...")]
        public string TipoSuministro { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la frecuencia de entrega...")]
        public string FrecuenciaEntrega { get; set; }
    }
}
