using Carniceria.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Net.Http.Json;


namespace Carniceria.Web.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public EmpleadosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }
        //CONSULTAR EMPLEADO
        //Muestra la tabla principal con todos los empleados

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var empleadoHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            try
            {
                var listaEmpleados = await empleadoHttp.GetFromJsonAsync<List<Empleado>>("api/empleados");
                return View(listaEmpleados ?? new List<Empleado>());
            }
            catch
            {
                return View(new List<Empleado>());
            }
        }

        //CREAR EMPLEADO (GET)
        //Muestra el formulario en blanco
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //CREAR EMPLEADO POST
        //Recibe los datos, valida y guarda
        [HttpPost]
        public async Task <IActionResult> Create(Empleado nuevoEmpleado)
        {
            ModelState.Clear();

            var empleadoHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            //Se envía a la API
            var respuesta = await empleadoHttp.PostAsJsonAsync("api/empleados", nuevoEmpleado);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Empleado registrado correctamente";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //En caso de que la API lo rechace
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                ModelState.AddModelError("Identificacion", mensajeError);
                return View(nuevoEmpleado);
            }
        }

        //ACTUALIZAR EMPLEADO(GET)
        //Busca al empleado por ID y muestra el formulario lleno
        [HttpGet]
        public async Task<IActionResult> Actualizar(Guid id)
        {
            var empleadoHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            //Consulta API
            var respuesta = await empleadoHttp.GetAsync($"api/empleados/{id}");
            if (respuesta.IsSuccessStatusCode)
            {
                var empleado = await respuesta.Content.ReadFromJsonAsync<Empleado>();
                return View(empleado);
            }

            return RedirectToAction("Index");
        }

        //[POST]Remplazo los datos del empleado en la lista de empleados
        [HttpPost]
        public async Task<IActionResult> Actualizar(Guid id, Empleado empleadoActualizado)
        {
            empleadoActualizado.Id = id;
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return View(empleadoActualizado);
            }

            var empleadoHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            var respuesta = await empleadoHttp.PutAsJsonAsync($"api/empleados/{id}", empleadoActualizado);
            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Datos del empleado actualizados correctamente";
                return RedirectToAction("Index");
            }
            else
            {
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                ModelState.AddModelError("Identificacion", mensajeError);
                return View(empleadoActualizado);
            }
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

       /* //BUSCAR(GET)
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
                ViewBag.Mensaje = "No se encontraron empleados con la identificación ingresada";
                return View();
            }

            return View(empleado);
        }*/
    }   
}