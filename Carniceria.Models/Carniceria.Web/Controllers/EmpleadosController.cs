using Carniceria.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;


namespace Carniceria.Web.Controllers
{
    public class EmpleadosController : Controller
    {
        private static List<Empleado> _empleados = new List<Empleado>();

        //CONSULTAR EMPLEADO
        //Muestra la tabla principal con todos los empleados

        public IActionResult Index()
        {
            return View(_empleados);
        }

        //CREAR EMPLEADO (GET)
        //Muestra el formulario en blanco
        public IActionResult Create()
        {
            return View();
        }

        //CREAR EMPLEADO POST
        //Recibe los datos, valida y guarda
        [HttpPost]
        public IActionResult Create(Empleado nuevoEmpleado)
        {
            bool identificacionExiste = _empleados.Any(e => e.Identificacion == nuevoEmpleado.Identificacion);
            if (identificacionExiste)
            {
                ModelState.AddModelError("Identificacion", "Ya existe un empleado con esta identificación");
            }

            if (ModelState.IsValid)
            {
                nuevoEmpleado.Id = Guid.NewGuid();//ID Generado automáticamente
                _empleados.Add(nuevoEmpleado);
                TempData["MensajeExito"] = "Empleado registrado correctamente";
                return RedirectToAction("Index");
            }
            return View(nuevoEmpleado);//Devuelve al formulario si hay error
        }

        //ACTUALIZAR EMPLEADO(GET)
        //Busca al empleado por ID y muestra el formulario lleno
        public IActionResult Actualizar(Guid id)
        {
            var empleado = _empleados.FirstOrDefault(e => e.Id == id);
            if(empleado == null)
            {
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        //[POST]Remplazo los datos del empleado en la lista de empleados
        [HttpPost]
        public IActionResult Actualizar(Empleado empleadoActualizado)
        {
            bool existe = _empleados.Any(e => e.Identificacion == empleadoActualizado.Identificacion && e.Id != empleadoActualizado.Id);
            if (existe)
            {
                ModelState.AddModelError("Identificacion", "Ya existe otra persona con esta identificación");
            }

            if (ModelState.IsValid)
            {
                var index = _empleados.FindIndex(e => e.Id == empleadoActualizado.Id);
                if (index != -1)
                {
                    _empleados[index] = empleadoActualizado;
                    TempData["MensajeExisto"] = "Datos del empleado actualizados correctamente";
                    return RedirectToAction("Index");
                }
            }
            return View(empleadoActualizado);
        }

        //ELIMINAR EMPLEADO (GET)
        //Muestra la pantalla de confirmación con los datos
        public IActionResult Eliminar(Guid id)
        {
            var empleado = _empleados.FirstOrDefault(e => e.Id == id);
            if(empleado == null)
            {
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        //(POST) Ejecuta el borrado 
        [HttpPost]
        public IActionResult EliminarConfirmado(Guid id)
        {
            var empleado = _empleados.FirstOrDefault(e => e.Id == id);
            if(empleado != null)
            {
                _empleados.Remove(empleado);
                TempData["MensajeExito"] = "Empleado eliminado del sistema";
            }
            return RedirectToAction("Index");
        }

        //BUSCAR(GET)
        //Muestra la pantalla de búsqueda con la caja de texto
        public IActionResult Buscar()
        {
            return View();
        }

        //POST 
        //Procesa la búsqueda por identificación
        [HttpPost]
        public IActionResult Buscar(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                ViewBag.Mensaje = "Debe ingresar una identificación para poder buscar un empleado";
                return View();
            }

            var empleado = _empleados.FirstOrDefault(e => e.Identificacion == identificacion);

            if (empleado == null)
            {
                ViewBag.Mensaje = "No se encontraron empleados con esa identificación";
                return View();
            }

            return View(empleado);
        }
    }   
}