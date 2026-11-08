using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carniceria.Web.Controllers
{
    public class ProductosController : Controller 
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //COSULTAR PRODUCTOS
        public async Task <IActionResult> Index()
        {
            var productoHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            try
            {
                var listaProductos = await productoHttp.GetFromJsonAsync<List<Producto>>("api/productos");
                return View(listaProductos ?? new List<Producto>());
            }
            catch
            {
                return View(new List<Producto>());
            }
        }

        //REGISTRAR PRODUCTO(GET)
        [HttpGet]
        public async Task <IActionResult> Create()
        {
            var apiHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            var proveedores = await apiHttp.GetFromJsonAsync<List<Proveedor>>("api/proveedores") ?? new List<Proveedor>();

            ViewBag.ListaProveedores = proveedores;
            return View();
        }

        //REGISTRAR PRODUCTO (POST)
        [HttpPost]
        public async Task <IActionResult> Create(Producto nuevoProducto)
        {
            ModelState.Remove("Id");
            ModelState.Remove("Proveedor");
            var apiHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            if (!ModelState.IsValid)
            {
                var proveedores = await apiHttp.GetFromJsonAsync<List<Proveedor>>("api/proveedores") ?? new List<Proveedor>();
                ViewBag.ListaProveedores = proveedores;
                return View(nuevoProducto);
            }
            var respuesta = await apiHttp.PostAsJsonAsync("api/productos", nuevoProducto);
            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Producto registrado correctamente";
                return RedirectToAction("Index");
            }
            else
            {
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                ModelState.AddModelError("CodigoProducto", mensajeError);

                var proveedores = await apiHttp.GetFromJsonAsync<List<Proveedor>>("api/proveedores") ?? new List<Proveedor>();
                ViewBag.ListaProveedores = proveedores;

                return View(nuevoProducto);
            }
        }

       /* //ACTUALIZAR PRODUCTOR (POST)
        [HttpGet]
        public IActionResult Actualizar(Guid id)
        {
            var producto = listaProductos.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return RedirectToAction("Index");
            }
            return View(producto);
        }
        //ACTUALIZAR PRODUCTOS (POST)
        [HttpPost]
        public IActionResult Actualizar(Producto productoActualizado)
        {
            bool existe = listaProductos.Any(p => p.CodigoProducto.Equals(productoActualizado.CodigoProducto, StringComparison.OrdinalIgnoreCase) && productoActualizado.Id != productoActualizado.Id);
            if (existe)
            {
                ModelState.AddModelError("CodigoProducto", "Otro producto ya utiliza este código");
            }

            if (ModelState.IsValid)
            {
                var index = listaProductos.FindIndex(p => p.Id == productoActualizado.Id);
                if(index != -1)
                {
                    listaProductos[index] = productoActualizado;
                    TempData["MensajeExito"] = "Información del producto actualizada";
                    return RedirectToAction("Index");
                }
            }
            return View(productoActualizado);
        }

        //ELIMINAR PRODUCTO (GET)
        [HttpGet]
        public IActionResult Eliminar(Guid id)
        {
            var producto = listaProductos.FirstOrDefault(p => p.Id == id);
            if(producto == null)
            {
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        //ELIMINAR PRODUCTO (POST)
        [HttpPost]
        public IActionResult EliminarConfirmado(Guid id) 
        {
            var producto = listaProductos.FirstOrDefault(p => p.Id == id);
            if(producto != null)
            {
                listaProductos.Remove(producto);
                TempData["MensajeExito"] = "Producto eliminado correctamente del inventario";
            }
            return RedirectToAction("Index");
        }

        //BUSCAR PRODUCTO (GET)
        [HttpGet]
        public IActionResult Buscar()
        {
            return View();
        }

        //BUSCAR PRODUCTO (POST)
        [HttpPost]
        public IActionResult Buscar(string codigoProducto)
        {
            if (string.IsNullOrWhiteSpace(codigoProducto))
            {
                ViewBag.Mensaje = "Ingrese el código o el nombre para buscar un producto";
                return View();
            }

            //Búsqueda de productos por coincidencia
            var producto = listaProductos.FirstOrDefault(p => p.CodigoProducto == codigoProducto);

            if(producto == null)
            {
                ViewBag.Mensaje = "No se encontraron productos con el código ingresado";
                return View();
            }

            return View(producto);
        }*/
    }
}
