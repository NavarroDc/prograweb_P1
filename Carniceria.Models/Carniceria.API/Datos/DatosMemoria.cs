using Carniceria.Models;
using System.Security.Cryptography.X509Certificates;

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

        public static List<Empleado> Empleados { get; set; } = new List<Empleado>
        {
            new Empleado
            {
                Id = Guid.NewGuid(),
                TipoIdentificacion = "Cédula nacional",
                Identificacion = "10101010",
                Nombre = "Alejandro",
                PrimerApellido = "Vega",
                SegundoApellido = "Maroto",
                FechaNacimiento = new DateTime(2003, 8, 13),
                SalarioMensual = 10000,
                FechaIngreso = new DateTime(2020, 8, 13),
                AreaTrabajo = "Administración",
                Puesto = "Asistente administrativo",
                Turno = "Tarde"
            }
        };
        public static List<Producto> Productos { get; set; } = new List<Producto>
        {
            new Producto
            {
                Id = Guid.NewGuid(),
                CodigoProducto = "123abc",
                NombreProducto = "Salchichón",
                Categoria = "Embutidos",
                UnidadMedida = "Unidad",
                PrecioUnidadMedida = 15000,
                ExistenciaActual = 30,
                ExistenciaMinima = 10
            }
        };
        public static List<Proveedor> Proveedores { get; set; } = new List<Proveedor>();
        public static List<Factura> Facturas { get; set; } = new List<Factura>();
        public static List<DetalleFactura> DetallesFactura {  get; set; } = new List<DetalleFactura>();
    }
}