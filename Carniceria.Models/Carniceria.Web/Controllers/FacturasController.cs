using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;

namespace Carniceria.Web.Controllers
{
    public class FacturasController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FacturasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 1. CONSULTAR FACTURAS (GET)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var apiHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            try
            { 
                var facturas = await apiHttp.GetFromJsonAsync<List<Factura>>("api/facturas") ?? new List<Factura>();
                var clientes = await apiHttp.GetFromJsonAsync<List<Cliente>>("api/clientes") ?? new List<Cliente>();
                ViewBag.ListaClientes = clientes;

                return View(facturas);
            }
            catch
            {
                return View(new List<Factura>());
            }
        }

        // 2. REGISTRAR FACTURA (GET)
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var apiHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            // Se cargan los catálogos necesarios para los selects del formulario
            ViewBag.ListaClientes = await apiHttp.GetFromJsonAsync<List<Cliente>>("api/clientes") ?? new List<Cliente>();
            ViewBag.ListaEmpleados = await apiHttp.GetFromJsonAsync<List<Empleado>>("api/empleados") ?? new List<Empleado>();
            ViewBag.ListaProductos = await apiHttp.GetFromJsonAsync<List<Producto>>("api/productos") ?? new List<Producto>();

            return View();
        }

        // 2. REGISTRAR FACTURA (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Factura nuevaFactura)
        {
            ModelState.Remove("NumeroFactura");
            ModelState.Remove("Detalles[0].UnidadMedida");
            ModelState.Remove("Detalles[0].ProductoAsociado");
            var apiHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            if (nuevaFactura.Detalles == null || !nuevaFactura.Detalles.Any())
            {
                ModelState.AddModelError("", "Debe agregar al menos una línea de detalle a la factura");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ListaClientes = await apiHttp.GetFromJsonAsync<List<Cliente>>("api/clientes") ?? new List<Cliente>();
                ViewBag.ListaEmpleados = await apiHttp.GetFromJsonAsync<List<Empleado>>("api/empleados") ?? new List<Empleado>();
                ViewBag.ListaProductos = await apiHttp.GetFromJsonAsync<List<Producto>>("api/productos") ?? new List<Producto>();
                return View(nuevaFactura);
            }

            // Se envía la factura completa a la API
            var respuesta = await apiHttp.PostAsJsonAsync("api/facturas", nuevaFactura);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Factura registrada correctamente";
                return RedirectToAction("Index");
            }
            else
            {
                var errorMensaje = await respuesta.Content.ReadAsStringAsync();
                ModelState.AddModelError("", errorMensaje);

                ViewBag.ListaClientes = await apiHttp.GetFromJsonAsync<List<Cliente>>("api/clientes") ?? new List<Cliente>();
                ViewBag.ListaEmpleados = await apiHttp.GetFromJsonAsync<List<Empleado>>("api/empleados") ?? new List<Empleado>();
                ViewBag.ListaProductos = await apiHttp.GetFromJsonAsync<List<Producto>>("api/productos") ?? new List<Producto>();
                return View(nuevaFactura);
            }


        }

        // GET: 3. Buscar facturas
        [HttpGet]
        public IActionResult Buscar()
        {
            return View(new List<Factura>());
        }

        // POST: 3. Buscar facturas
        [HttpPost]
        public async Task<IActionResult> Buscar(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
            {
                ModelState.AddModelError("", "Debe ingresar un número de factura o la identificación del cliente.");
                return View(new List<Factura>());
            }

            var apiHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            try
            {
                var response = await apiHttp.GetAsync($"api/facturas/buscar/{termino}");

                if (response.IsSuccessStatusCode)
                {
                    var facturasEncontradas = await response.Content.ReadFromJsonAsync<List<Factura>>() ?? new List<Factura>();

                    // Cargamos los clientes para mostrar el nombre y cédula en la tabla de resultados
                    var clientes = await apiHttp.GetFromJsonAsync<List<Cliente>>("api/clientes") ?? new List<Cliente>();
                    ViewBag.ListaClientes = clientes;

                    return View(facturasEncontradas);
                }
                else
                {
                    // Si la API responde con error, informamos a la persona usuaria
                    ModelState.AddModelError("", "No se encontraron coincidencias con la búsqueda ingresada");
                    return View(new List<Factura>());
                }
            }
            catch
            {
                ModelState.AddModelError("", "Ocurrió un error de conexión...");
                return View(new List<Factura>());
            }
        }

        [HttpGet]//4. Actualizar facturas
        public async Task<IActionResult> Actualizar(Guid id)
        {
            var apiHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            // 1. Obtenemos la factura existente por su ID
            var factura = await apiHttp.GetFromJsonAsync<Factura>($"api/facturas/{id}");
            if (factura == null) return NotFound();

            // 2. Cargamos los catálogos para los menús desplegables
            ViewBag.ListaClientes = await apiHttp.GetFromJsonAsync<List<Cliente>>("api/clientes") ?? new List<Cliente>();
            ViewBag.ListaEmpleados = await apiHttp.GetFromJsonAsync<List<Empleado>>("api/empleados") ?? new List<Empleado>();
            ViewBag.ListaProductos = await apiHttp.GetFromJsonAsync<List<Producto>>("api/productos") ?? new List<Producto>();

            return View(factura);
        }

        // 4. ACTUALIZAR FACTURA (POST)
        [HttpPost]
        public async Task<IActionResult> Actualizar(Guid id, Factura facturaActualizada)
        {
            if (id != facturaActualizada.Id) return BadRequest();

            // Ignoramos campos autogenerados para evitar errores de validación 
            ModelState.Remove("NumeroFactura");
            ModelState.Remove("FechaHoraEmision");
            ModelState.Remove("Detalles[0].UnidadMedida");
            ModelState.Remove("Detalles[0].ProductoAsociado");

            var apiHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            if (!ModelState.IsValid)
            {
                ViewBag.ListaClientes = await apiHttp.GetFromJsonAsync<List<Cliente>>("api/clientes") ?? new List<Cliente>();
                ViewBag.ListaEmpleados = await apiHttp.GetFromJsonAsync<List<Empleado>>("api/empleados") ?? new List<Empleado>();
                ViewBag.ListaProductos = await apiHttp.GetFromJsonAsync<List<Producto>>("api/productos") ?? new List<Producto>();
                return View(facturaActualizada);
            }

            // Enviamos la actualización a la API 
            var respuesta = await apiHttp.PutAsJsonAsync($"api/facturas/{id}", facturaActualizada);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Factura actualizada correctamente";
                return RedirectToAction("Index");
            }
            else
            {
                var errorMensaje = await respuesta.Content.ReadAsStringAsync();
                ModelState.AddModelError("", errorMensaje);

                ViewBag.ListaClientes = await apiHttp.GetFromJsonAsync<List<Cliente>>("api/clientes") ?? new List<Cliente>();
                ViewBag.ListaEmpleados = await apiHttp.GetFromJsonAsync<List<Empleado>>("api/empleados") ?? new List<Empleado>();
                ViewBag.ListaProductos = await apiHttp.GetFromJsonAsync<List<Producto>>("api/productos") ?? new List<Producto>();
                return View(facturaActualizada);
            }
        }
    }
}