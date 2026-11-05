using Carniceria.Models;

namespace Carniceria.API.Datos
{
    public static class DatosMemoria
    {
        public static List<Cliente> Clientes { get; set; } = new List<Cliente>
        {
            new Cliente
            {
                Id = Guid.NewGuid(),
                TipoIdentificacion = "Cédula nacional",
                Identificacion = "10101010",
                Nombre = "María",
                PrimerApellido = "Martínez",
                SegundoApellido = "Guevara",
                FechaNacimiento = new DateTime(2003, 7, 12)
            }
        };
    }
}
