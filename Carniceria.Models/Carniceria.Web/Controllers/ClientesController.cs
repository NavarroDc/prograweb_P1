using Carniceria.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;

namespace Carniceria.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //CONSULTAR CLIENTE
        public async Task<IActionResult> Index()
        {
            var clienteHttp = _httpClientFactory.CreateClient("CarniceriaApi");
            try
            {
                var listaClientes = await clienteHttp.GetFromJsonAsync<List<Cliente>>("api/clientes");
                return View(listaClientes ?? new List<Cliente>());
            }
            catch
            {
                return View(new List<Cliente>());
            }
        }

        //REGISTRAR CLIENTE
        //Muestra formulario en blanco
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Captura los datos y los guarda
        [HttpPost]
        public async Task<IActionResult> Create(Cliente clienteNuevo)
        {
            ModelState.Clear();

            var clienteHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            var respuesta = await clienteHttp.PostAsJsonAsync("api/clientes",clienteNuevo);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                ModelState.AddModelError("Identificacion", mensajeError);
                return View(clienteNuevo);
            }

            
        }

        //BUSCAR CLIENTE
        //Muestra pantalla de búsqueda
        [HttpGet]
        public IActionResult Buscar()
        {
            return View();
        }

        //Procesa la búsqueda
        [HttpPost]
        public async Task<IActionResult> Buscar(string identificacion)
        {
            //Si el usuario no escribe nada y presiona buscar
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                ViewBag.Mensaje = "Debe ingresar una identificación para buscar...";
                return View();
            }
            var clienteHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            try
            {
                //Llamada al endpoint de la API que busca por identificación
                var respuesta = await clienteHttp.GetAsync($"api/clientes/buscar/{identificacion}");

                if (respuesta.IsSuccessStatusCode)
                {
                    var clienteEncontrado = await respuesta.Content.ReadFromJsonAsync<Cliente>();
                    return View(clienteEncontrado);
                }
                else if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.Mensaje = "No se encontraron clientes con la identificación ingresada";
                    return View();
                }
                else
                {
                    ViewBag.Mensaje = "Ocurrió un error al consultar los datos";
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Mensaje = "Error al conectar con la API";
                return View();
            }
        }

        //ACTUALIZAR CLIENTE
        //Busca cliente y muestra el formulario lleno
        [HttpGet]
        public async Task<IActionResult> Actualizar(Guid id)
        {
            var clienteHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            //Consulta de la API por el ID
            var respuesta = await clienteHttp.GetAsync($"api/clientes/{id}");

            if (respuesta.IsSuccessStatusCode)
            {
                var cliente = await respuesta.Content.ReadFromJsonAsync<Cliente>();
                return View(cliente);
            }
            return NotFound();//Error en caso de que no exista
        }

        //Captura lo datos actualizados, valida y guarda
        [HttpPost]
        public async Task<IActionResult> Actualizar(Guid id, Cliente clienteActualizado)
        {
            clienteActualizado.Id = id;

            ModelState.Clear();

            var clienteHttp = _httpClientFactory.CreateClient("CarniceriaApi");

            //Se envían los datos a la API con PUT
            var respuesta = await clienteHttp.PutAsJsonAsync($"api/clientes/{clienteActualizado.Id}", clienteActualizado);
            if (respuesta.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "El cliente fue actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                ModelState.AddModelError("Identificacion", mensajeError);
                return View(clienteActualizado);
            }
        }

       /* //ELIMINAR CLIENTE
        //Busca al cliente y muestra la pantalla de confirmación
        [HttpGet]
        public IActionResult Eliminar(Guid id)
        {
            var cliente = listaClientes.FirstOrDefault(c => c.Id == id);

            if( cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        //Procesa el borrado final
        [HttpPost, ActionName("Eliminar")]
        public IActionResult EliminarConfirmado(Guid id)
        {
            var cliente = listaClientes.FirstOrDefault(c => c.Id == id);

            if(cliente != null)
            {
                listaClientes.Remove(cliente);//Se elimina definitivamente
                TempData["MensajeExito"] = "¡El cliente ha sido eliminado correctamente!";
            }

            return RedirectToAction(nameof(Index));
        }*/
    }
}