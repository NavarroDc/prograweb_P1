using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Carniceria.Web.Controllers

{
    public class ProveedoresController : Controller
    {
        private static List<Proveedor> listaProveedores = new List<Proveedor>();

        //CONSULTAR PROVEEDORES GET
        [HttpGet]
        public IActionResult Index()
        {
            return View(listaProveedores);
        }

        //REGISTRAR PROVEEDORES GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //REGISTRAR PROVEEDORES POST
        [HttpPost]
        public IActionResult Create(Proveedor proveedor)
        {
            proveedor.Id = Guid.NewGuid();
            listaProveedores.Add(proveedor);
            TempData["MensajeExito"] = "Proveedor registrado correctamente";

            return RedirectToAction("Index");
        }

        //ACTUALIZAR PROVEEDORES GET
        [HttpGet]
        public IActionResult Actualizar(Guid id)
        {
            var proveedor = listaProveedores.FirstOrDefault(p => p.Id == id);

            if(proveedor == null)
            {
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        //ACTUALIZAR PROVEEDORES POST
        [HttpPost]
        public IActionResult Actualizar(Proveedor proveedor)
        {
            var index = listaProveedores.FindIndex(p => p.Id == proveedor.Id);

            if(index != -1)
            {
                listaProveedores[index] = proveedor;
                TempData["MensajeExito"] = "El proveedor se actualizó correctamente";
            }

            return RedirectToAction("Index");
        }

        //ELIMINAR PROVEEDORES GET
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
        public IActionResult Buscar(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                ViewBag.Mensaje = "Ingrese el código para buscar un proveedor";
                return View();
            }

            var proveedor = listaProveedores.FirstOrDefault(p => p.Identificacion == identificacion); 
            
            if(proveedor == null)
            {
                ViewBag.Mensaje = "No se encontraron proveedores con la identificación ingresada";
                return View();
            }
            return View(proveedor);
        }
    }
}
