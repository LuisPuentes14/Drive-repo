using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace archivos.Controllers
{
    public class ClienteController : Controller
    {

        private readonly ILogger<ClienteController> _logger;
        private readonly Conexion _contextData;

        public ClienteController(ILogger<ClienteController> logger, Conexion contextData)
        {
            _logger = logger;
            _contextData = contextData;
        }


        public IActionResult list()
        {
            List<CE_Cliente> cE_Clientes = new CN_Cliente(_contextData).list();
            ViewBag.Clientes = cE_Clientes;

            return View();
        }
    }
}
