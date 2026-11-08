using Carniceria.Models;
using System.Security.Cryptography.X509Certificates;

namespace Carniceria.API.Datos
{
    public static class DatosMemoria
    {
        // ID fijo que se comparte entre las listas
        public static readonly Guid _idProveedorPorDefecto = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid _idClientePorDefecto = Guid.Parse("22222222-2222-2222-2222-222222222222");
        public static readonly Guid _idEmpleadoPorDefecto = Guid.Parse("33333333-3333-3333-3333-333333333333");
        public static readonly Guid _idProductoPorDefecto = Guid.Parse("44444444-4444-4444-4444-444444444444");

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
                ProveedorId = _idProveedorPorDefecto,
                CodigoProducto = "123abc",
                NombreProducto = "Salchichón",
                Categoria = "Embutidos",
                UnidadMedida = "Unidad",
                PrecioUnidadMedida = 15000,
                ExistenciaActual = 30,
                ExistenciaMinima = 10
            }
        };
        public static List<Proveedor> Proveedores { get; set; } = new List<Proveedor>
        {
            new Proveedor
            {
                Id = _idProveedorPorDefecto,
                TipoIdentificacion = "Cédula nacional",
                Identificacion = "10101010",
                RazonSocial = "Walmart",
                PersonaContacto = "Manuel",
                Telefono = "88888888",
                CorreoElectronico = "datoInicial321@gmail.com",
                TipoSuministro = "Empaques",
                FrecuenciaEntrega = "Quincenal"
            }
        };
        public static List<Factura> Facturas { get; set; } = new List<Factura>
        {
            new Factura
            {
                Id = Guid.NewGuid(),
                NumeroFactura = "FAC-0001",
                FechaHoraEmision = DateTime.Now,
                ClienteId = _idClientePorDefecto,
                EmpleadoId = _idEmpleadoPorDefecto,
                MetodoPago = "Efectivo",
                PorcentajeDescuento = 0,
                Estado = "Pendiente",
                Detalles = new List<DetalleFactura>
                {
                    new DetalleFactura
                    {
                        IdentificadorDetalle = Guid.NewGuid(),
                        ProductoId = _idProductoPorDefecto,
                        UnidadMedida = "Unidad",
                        Cantidad = 2,
                        PrecioUnitario = 15000
                    },
                    new DetalleFactura
                    {
                        IdentificadorDetalle = Guid.NewGuid(),
                        ProductoId = _idProductoPorDefecto,
                        UnidadMedida = "Unidad",
                        Cantidad = 1,
                        PrecioUnitario = 15000
                    }
                }
            }
        };
        public static List<DetalleFactura> DetallesFactura {  get; set; } = new List<DetalleFactura>();
    }
}