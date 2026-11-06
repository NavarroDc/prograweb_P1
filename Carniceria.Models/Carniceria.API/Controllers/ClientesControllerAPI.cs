using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;
using Carniceria.API.Datos;

namespace Carniceria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesControllerAPI : ControllerBase
    {
        [HttpGet]//1. Consulta todos los clientes
        public ActionResult<IEnumerable<Cliente>> GetClientes()
        {
            return Ok(DatosMemoria.Clientes);//Código 200 = Éxito. Se adjunta lista a la memoria
        }

        [HttpGet("buscar/{identificacion}")]//2. Buscar por identificación - RUTA: GET api/Cliente/Buscar/Cédula
        public ActionResult<Cliente> BuscarCliente(string identificacion)
        {
            var cliente = DatosMemoria.Clientes.FirstOrDefault(c => c.Identificacion == identificacion);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpGet("{id:guid}")]//3. Consultar por guid - RUTA: GET api/Clientes/identificador interno
        public ActionResult<Cliente> GetClientePorId(Guid id)
        {
            var cliente = DatosMemoria.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]//4. Registrar cliente nuevo
        public ActionResult<Cliente> RegistrarCliente(Cliente clienteNuevo)
        {
            bool existe = DatosMemoria.Clientes.Any(c => c.Identificacion == clienteNuevo.Identificacion);

            if (existe)
            {
                return BadRequest("Ya existe un cliene con esa identificación...");
            }

            clienteNuevo.Id = Guid.NewGuid();
            DatosMemoria.Clientes.Add(clienteNuevo);

            return CreatedAtAction(nameof(GetClientePorId), new { id = clienteNuevo.Id }, clienteNuevo);
        }

        [HttpPut("{id:guid}")]//5. Actualizar cliente
        public IActionResult ActualizarCliente(Guid id, Cliente clienteActualizado)
        {
            if (id != clienteActualizado.Id) return BadRequest("Los IDs no coinciden...");
            bool existeOtro = DatosMemoria.Clientes.Any(c => c.Identificacion == clienteActualizado.Identificacion && c.Id != clienteActualizado.Id);

            if(existeOtro)
            {
                return BadRequest("Ya existe otro cliente con esa identificación...");
            }
        
            var index = DatosMemoria.Clientes.FindIndex(c => c.Id == id);
            if (index == -1) return NotFound();

            DatosMemoria.Clientes[index] = clienteActualizado;
            return NoContent();
        }
    }
}
