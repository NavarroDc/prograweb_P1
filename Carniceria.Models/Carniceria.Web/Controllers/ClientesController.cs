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
            bool existe = listaClientes.Any(c => c.Identificacion == clienteNuevo.Identificacion);
            if (existe)
            {
                ModelState.AddModelError("Identificacion", "Ya existe un cliente con esa identificación");
            }
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

        //ACTUALIZAR CLIENTE
        //Busca cliente y muestra el formulario lleno
        [HttpGet]
        public IActionResult Actualizar(Guid id)
        {
            var cliente = listaClientes.FirstOrDefault(c => c.Id == id);

            if(cliente == null)
            {
                return NotFound();//Error en caso de que no exista
            }

            return View(cliente);
        }

        //Captura lo datos actualizados, valida y guarda
        [HttpPost]
        public IActionResult Actualizar (Cliente clienteActualizado)
        {
            bool existe = listaClientes.Any(c => c.Identificacion == clienteActualizado.Identificacion && c.Id != clienteActualizado.Id);
            if (existe)
            {
                ModelState.AddModelError("Identificacion", "Otro cliente ya tiene esta identificación");
            }
            if (ModelState.IsValid)
            {
                var index = listaClientes.FindIndex(c => c.Id == clienteActualizado.Id);

                if (index != -1)
                {
                    listaClientes[index] = clienteActualizado;

                    TempData["MensajeExito"] = "¡El cliente fue actualizado correctamente!";
                }

                return RedirectToAction(nameof(Index));
            }

            return View(clienteActualizado);
        }

        //ELIMINAR CLIENTE
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
        }
    }
}