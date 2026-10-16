using Carniceria.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carniceria.Web.Controllers
{
    public class ClientesController : Controller
    {
        private static List<Cliente> listaClientes =  new List<Cliente>();

        //CONSULTAR CLIENTE
        public IActionResult Index()
        {
            return View(listaClientes);
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
        public IActionResult Create(Cliente clienteNuevo)
        {
            //Validaciones clase Cliente
            if (ModelState.IsValid)
            {
                clienteNuevo.Id = Guid.NewGuid();
                listaClientes.Add(clienteNuevo);
                return RedirectToAction(nameof(Index));
            }
            return View(clienteNuevo);
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
        public IActionResult Buscar(string identificacion)
        {
            var clienteEncontrado = listaClientes.FirstOrDefault(c => c.Identificacion == identificacion);

            if(clienteEncontrado == null)
            {
                ViewBag.Mensaje = "No se encontraron clientes con la identificación ingresada";
            }

            return View(clienteEncontrado);
        }
    }
}
