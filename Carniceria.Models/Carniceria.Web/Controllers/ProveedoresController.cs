using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Carniceria.Web.Controllers

{
    public class ProveedoresController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProveedoresController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //CONSULTAR PROVEEDORES GET
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var proveedorHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            try
            {
                var listaProveedores = await proveedorHttp.GetFromJsonAsync<List<Proveedor>>("api/proveedores");
                return View(listaProveedores ?? new List<Proveedor>());
            }
            catch
            {
                return View(new List<Proveedor>());
            }
        }

        //REGISTRAR PROVEEDORES GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //REGISTRAR PROVEEDORES POST
        [HttpPost]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
            ModelState.Remove("Id");

            if (!ModelState.IsValid)
            {
                return View(proveedor);
            }

            var proveedorHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            var respuesta = await proveedorHttp.PostAsJsonAsync("api/proveedores", proveedor);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Proveedor registrado correctamente";
                return RedirectToAction("Index");
            }
            else
            {
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                ModelState.AddModelError("Identificacion", mensajeError);
                return View(proveedor);
            }
        }

        //ACTUALIZAR PROVEEDORES GET
        [HttpGet]
        public async Task<IActionResult> Actualizar(Guid id)
        {
            var proveedorHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            var respuesta = await proveedorHttp.GetAsync($"api/proveedores/{id}");

            if (respuesta.IsSuccessStatusCode)
            {
                var proveedor = await respuesta.Content.ReadFromJsonAsync<Proveedor>();
                return View(proveedor);
            }
            return RedirectToAction("Index");
        }

        //ACTUALIZAR PROVEEDORES POST
        [HttpPost]
        public async Task <IActionResult> Actualizar(Guid id, Proveedor proveedor)
        {
            proveedor.Id = id;
            ModelState.Remove("Id");

            if (!ModelState.IsValid)
            {
                return View(proveedor);
            }

            var proveedorHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            var respuesta = await proveedorHttp.PutAsJsonAsync($"api/proveedores/{id}", proveedor);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "El proveedor se actualizó correctamente";
                return RedirectToAction("Index");
            }
            else
            {
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                ModelState.AddModelError("Identificacion", mensajeError);
                return View(proveedor);
            }
        }

        /* //ELIMINAR PROVEEDORES GET
        [HttpGet]
        public IActionResult Eliminar(Guid id)
        {
            var proveedor = listaProveedores.FirstOrDefault(p => p.Id == id);

            if(proveedor == null)
            {
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        //ELIMINAR PROVEEDORES POST
        [HttpPost]
        public IActionResult EliminarConfirmado(Guid id)
        {
            var proveedor = listaProveedores.FirstOrDefault(p => p.Id == id);

            if(proveedor != null)
            {
                listaProveedores.Remove(proveedor);
                TempData["MensajeExito"] = "El proveedor se eliminó correctamente";
            }
            return RedirectToAction("Index");
        }

        //BUSCAR PROVEEDORES GET
        [HttpGet]
        public IActionResult Buscar()
        {
            return View();
        }

        //BUSCAR PROVEEDORES POST
        [HttpPost]
        public IActionResult Buscar(string terminoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(terminoBusqueda))
            {
                ViewBag.Mensaje = "Ingrese la identificación o razón social para buscar un proveedor";
                return View();
            }

            var proveedor = listaProveedores.FirstOrDefault(p => p.Identificacion == terminoBusqueda || p.RazonSocial == terminoBusqueda); 
            
            if(proveedor == null)
            {
                ViewBag.Mensaje = "No se encontraron proveedores con los datos ingresados";
                return View();
            }
            return View(proveedor);
        }*/
    }
}
