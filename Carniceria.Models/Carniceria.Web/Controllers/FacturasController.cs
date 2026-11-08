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
    }
}
