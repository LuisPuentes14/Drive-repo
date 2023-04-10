using CapaDatos;
using CapaEntidad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapaNegocio;
using Azure;
using System.Reflection;
using Newtonsoft.Json;
using System.Web.Helpers;
using archivos.Utilidades;

namespace archivos.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly Conexion _contextData;
        public DocumentsController(ILogger<DocumentsController> logger, Conexion contextData)
        {
            _logger = logger;
            _contextData = contextData;
        }

        [HttpGet]
        public async Task<IActionResult> getPrincipalFilesCliente(Int64 idCliente)
        {
            string message = string.Empty;

            List<CE_DocumentoXcliente> cE_DocumentoXclientes = new List<CE_DocumentoXcliente>();

            cE_DocumentoXclientes = new CN_Documents(_contextData).getPrincipalFilesCliente(idCliente, out message);

            ViewBag.idCliente = idCliente;

            ViewBag.files = cE_DocumentoXclientes;

            return View();

        }

        [HttpGet]
        public async Task<IActionResult> openFile(Int64 idDocPadre, Int64 idCliente)
        {
            string message = string.Empty;


            object parametersFile = new CN_Documents(_contextData).openFile(idDocPadre, idCliente, out message);

            // Obtener el tipo del objeto anónimo
            Type tipo = parametersFile.GetType();

            // Obtener las propiedades del objeto anónimo
            PropertyInfo[] propiedades = tipo.GetProperties();


            foreach (PropertyInfo propiedad in propiedades)
            {
                object valor = propiedad.GetValue(parametersFile);

                if (propiedad.Name == "idVolver")
                {
                    ViewBag.idVolver = valor;
                }

                if (propiedad.Name == "cliente")
                {
                    ViewBag.cliente = valor;
                }

                if (propiedad.Name == "idDocPadre")
                {
                    ViewBag.idDocPadre = valor;
                }

                if (propiedad.Name == "files")
                {                
                    string json  = JsonConvert.SerializeObject(valor);

                    List<CE_DocumentoXcliente> listaDeObjetos = JsonConvert.DeserializeObject<List<CE_DocumentoXcliente>>(json);

                    ViewBag.files = listaDeObjetos; 
                }
            }
            return View();
        }


        public async Task<IActionResult> addFolder([FromBody] CE_DocumentoXcliente cE_DocumentoXcliente)
        {

            bool isSave = false;
            string message = string.Empty;

            isSave = new CN_Documents(_contextData).addFolder(cE_DocumentoXcliente, out message);

            if (!isSave)
            {
                return Ok("{\"status\":\"400\", \"message\":\"" + message + "\"}");
            }

            return Ok("{\"status\":\"200\", \"message\":\"" + message + "\"}");
        }


        public async Task<IActionResult> addFile([FromForm] IFormFile archivo, CE_DocumentoXcliente cE_DocumentoXcliente)
        {

            bool isSave = false;
            string message = string.Empty;

            isSave = new CD_Documents(_contextData).addFile(archivo, cE_DocumentoXcliente, out message);

            if (!isSave)
            {
                return Ok("{\"status\":\"400\", \"message\":\"" + message + "\"}");
            }

            return Ok("{\"status\":\"200\", \"message\":\"" + message + "\"}");
        }


        public async Task<IActionResult> deleteDocByFolder([FromBody] CE_DocumentoXcliente cE_DocumentoXcliente)
        {

            bool isDelete = false;
            string message = string.Empty;          

            isDelete = new CD_Documents(_contextData).deleteDocByFolder(cE_DocumentoXcliente, out message);


            if (!isDelete)
            {
                return Ok("{\"status\":\"400\", \"message\":\"" + message + "\"}");
            }

            return Ok("{\"status\":\"200\", \"message\":\"" + message + "\"}");

        }


        public async Task<FileStreamResult> getFileBytes([FromQuery] Int64 idCliente, Int64 idPadre, Int64 idHijo)
        {
            string message = string.Empty;

            CE_DocumentoXcliente cE_DocumentoXcliente = new CE_DocumentoXcliente() {

              id_cliente = idCliente,
              id_documento_padre = idPadre,
              id_documento_hijo = idHijo,
              isPrincipal = false,

            };

            cE_DocumentoXcliente = new CN_Documents(_contextData).getFileBytes(cE_DocumentoXcliente, out message);

            byte[] contenidoArchivo = cE_DocumentoXcliente.DATA;
            string nombreArchivo = cE_DocumentoXcliente.nombre_documento_hijo;

            nombreArchivo = FileUtil.biteToFile(contenidoArchivo, nombreArchivo);

            string filePath = "wwwroot/outputFile/" + nombreArchivo;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf");

        }


        public async Task<IActionResult> dowloandFile(Int64 idCliente, Int64 idPadre, Int64 idHijo)
        {

            string message = string.Empty;

            CE_DocumentoXcliente cE_DocumentoXcliente = new CE_DocumentoXcliente()
            {

                id_cliente = idCliente,
                id_documento_padre = idPadre,
                id_documento_hijo = idHijo,
                isPrincipal = false,

            };

            cE_DocumentoXcliente = new CN_Documents(_contextData).getFileBytes(cE_DocumentoXcliente, out message);

            byte[] contenidoArchivo = cE_DocumentoXcliente.DATA;
            string nombreArchivo = cE_DocumentoXcliente.nombre_documento_hijo;          
           
          
            return File(contenidoArchivo, "application/pdf", nombreArchivo);


        }
    }
}
