using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Carniceria.Models;
using Carniceria.API.Datos;

namespace Carniceria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> GetClientes()
        {
            return Ok(DatosMemoria.Clientes);
        }
    }
}
