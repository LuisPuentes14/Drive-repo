using Azure;
using CapaDatos;
using CapaEntidad;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Documents
    {

        private readonly Conexion _contextData;
        public CN_Documents(Conexion contextData)
        {
            _contextData = contextData;
        }

        public List<CE_DocumentoXcliente> getPrincipalFilesCliente(Int64 idCliente, out string message)
        {
            List<CE_DocumentoXcliente> cE_DocumentoXclientes = new List<CE_DocumentoXcliente>();
            message = "";            

            cE_DocumentoXclientes = new CD_Documents(_contextData).getPrincipalFilesCliente(idCliente, out message);
            
            return cE_DocumentoXclientes;

        }


        public object openFile(Int64 idDocPadre, Int64 idCliente, out string message)
        {
            message = "";
            object parametersFile = null;

            parametersFile = new CD_Documents(_contextData).openFile(idDocPadre, idCliente, out message);

            return parametersFile;
        }


        public bool addFolder(CE_DocumentoXcliente cE_DocumentoXcliente, out string message)
        {

            bool isSave = false;

            message = "";

            if (cE_DocumentoXcliente.isPrincipal == true)
            {

                if (string.IsNullOrEmpty(cE_DocumentoXcliente.nombre_documento_padre) || string.IsNullOrWhiteSpace(cE_DocumentoXcliente.nombre_documento_padre))
                {

                    message = "El nombre de la carpeta no puede ser vacia";
                    return isSave;
                }

            }

            if (cE_DocumentoXcliente.isPrincipal == false)
            {

                if (string.IsNullOrEmpty(cE_DocumentoXcliente.nombre_documento_hijo) || string.IsNullOrWhiteSpace(cE_DocumentoXcliente.nombre_documento_hijo))
                {
                    message = "El nombre de la carpeta no puede ser vacia";
                    return isSave;
                }

            }


            isSave = new CD_Documents(_contextData).addFolder(cE_DocumentoXcliente, out message);

            return isSave;
        }


        public bool addFile(IFormFile archivo, CE_DocumentoXcliente cE_DocumentoXcliente, out string message)
        {

            message = "";
            bool isSave = false;


            var nombreArchivo = archivo.FileName;

            if (string.IsNullOrEmpty(nombreArchivo) || string.IsNullOrWhiteSpace(cE_DocumentoXcliente.nombre_documento_hijo))
            {
                message = "El archivo no puede estar vacio";
                return isSave;
            }

            isSave = new CD_Documents(_contextData).addFile(archivo, cE_DocumentoXcliente, out message);



            return isSave;
        }


        public bool deleteDocByFolder(CE_DocumentoXcliente cE_DocumentoXcliente, out string message)
        {

            bool isDelete = false;
            message = "";      
                      

            isDelete = new CD_Documents(_contextData).deleteDocByFolder(cE_DocumentoXcliente, out message);

            return isDelete;

        }


        public CE_DocumentoXcliente getFileBytes(CE_DocumentoXcliente cE_DocumentoXcliente, out string message)
        {
            cE_DocumentoXcliente = new CD_Documents(_contextData).getFileBytes(cE_DocumentoXcliente, out message);

            return cE_DocumentoXcliente;

        }
    }

}
