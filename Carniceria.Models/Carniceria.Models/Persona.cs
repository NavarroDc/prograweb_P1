using System.ComponentModel.DataAnnotations;

namespace Carniceria.Models
{
    public class Persona
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el tipo de identificación...")]
        public string TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "Debe escribir la identificación...")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Debe escribir el nombre...")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres...")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe escribir el primer apellido...")]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "El primer apellido debe tener entre 3 y 75 caracteres...")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "Debe escribir el segundo apellido...")]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "El segundo apellido debe tener entre 3 y 75 caracteres...")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "Debe escribir la fecha de nacimiento...")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }
    }
}



