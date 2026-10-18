using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carniceria.Web.Controllers
{
    public class ProductosController : Controller 
    {
        private static List<Producto> listaProductos = new List<Producto>();

        //COSULTAR PRODUCTOS
        public IActionResult Index()
        {
            return View(listaProductos);
        }

        //REGISTRAR PRODUCTO(GET)
        public  IActionResult Create()
        {
            return View();
        }

        //REGISTRAR PRODUCTO (POST)
        [HttpPost]
        public IActionResult Create(Producto nuevoProducto)
        {
            bool existe = listaProductos.Any(p => p.CodigoProducto.Equals(nuevoProducto.CodigoProducto, StringComparison.OrdinalIgnoreCase));
            if (existe)
            {
                ModelState.AddModelError("Codigo", "Ya existe un producto registrado con este código");
            }

            if (ModelState.IsValid)
            {
                nuevoProducto.Id = Guid.NewGuid();
                listaProductos.Add(nuevoProducto);
                TempData["MensajeExito"] = "Producto registrado correctamente en el inventario";
                return RedirectToAction("Index");
            }
            return View(nuevoProducto);
        }

        //ACTUALIZAR PRODUCTOR (POST)
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
            if(producto == null)
            {
                listaProductos.Remove(producto);
                TempData["MensajeExito"] = "Producto eliminado correctamente del inventario";
            }
            return RedirectToAction("Index");
        }
   
    }
}
