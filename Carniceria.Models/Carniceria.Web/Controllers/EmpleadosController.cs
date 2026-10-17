using Carniceria.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


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

        //CREAR (GET)
        //Muestra el formulario en blanco
        public IActionResult Create()
        {
            return View();
        }

        //CREAR POST
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
    }   
}
