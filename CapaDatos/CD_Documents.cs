using CapaEntidad;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Documents
    {
        private readonly Conexion _contextData;
        private Int64 cliente;
        private Int64 padre;
        private DataTable Table = new DataTable();



        public CD_Documents(Conexion contextData)
        {
            _contextData = contextData;
        }

        // Lista las carpetas principales por cliente

        public List<CE_DocumentoXcliente> getPrincipalFilesCliente(Int64 idCliente, out string message)
        {
            message = "";
            List<CE_DocumentoXcliente> folders = new List<CE_DocumentoXcliente>();



            try
            {
                var obj = (from documentoXcliente in _contextData.documentoXcliente
                           where documentoXcliente.id_cliente == idCliente
                           where documentoXcliente.isPrincipal == true
                           select documentoXcliente
                           ).ToList();


                // _contextData.documentoXcliente.Where(d => d.id_cliente == idCliente && d.isPrincipal == true).ToList();

                foreach (var item in obj)
                {
                    CE_DocumentoXcliente cE_DocumentoXcliente = new CE_DocumentoXcliente
                    {
                        id = item.id,
                        id_cliente = item.id_cliente,
                        id_documento_padre = item.id_documento_padre,
                        nombre_documento_padre = item.nombre_documento_padre,
                        nombre_documento_hijo = item.nombre_documento_hijo,
                        id_tipo_documento = item.id_tipo_documento,
                        DATA = item.DATA,
                        isPrincipal = item.isPrincipal
                    };

                    folders.Add(cE_DocumentoXcliente);


                }

            }
            catch (Exception ex)
            {

                message = ex.Message;
            }

            return folders;
        }



        public object openFile(Int64 idDocPadre, Int64 idCliente, out string message)
        {
            message = "";
            object parametersFile = null;

            try
            {

                var idPadreXHijo = _contextData.documentoXcliente.Where(d => d.id_documento_hijo == idDocPadre).Select(d => d.id_documento_padre).ToList();

                var files = (from documentoXcliente in _contextData.documentoXcliente
                             where documentoXcliente.id_cliente == idCliente
                             where documentoXcliente.isPrincipal == false
                             where documentoXcliente.id_documento_padre == idDocPadre
                             select new { 
                                 documentoXcliente.id_documento_padre,
                                 documentoXcliente.id_cliente,
                                 documentoXcliente.id_documento_hijo, 
                                 documentoXcliente.nombre_documento_hijo,
                                 documentoXcliente.id_tipo_documento,
                             }
                               ).ToList();




                parametersFile = new
                {
                    idVolver = (Int64)idPadreXHijo.Count() > 0 ? idPadreXHijo[0] : 0,
                    cliente = idCliente,
                    idDocPadre = idDocPadre,
                    files = files
                };

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return parametersFile;
        }

        public bool addFolder(CE_DocumentoXcliente cE_DocumentoXcliente, out string message)
        {

            bool isSave = false;

            message = "";

            try
            {
                Int64 nextId = _contextData.documentoXcliente
                       .Select(dc => (int?)dc.id)
                       .DefaultIfEmpty()
                       .Max() ?? 0 + 1;

                nextId += 1;

                if (cE_DocumentoXcliente.isPrincipal == true)
                {
                    cE_DocumentoXcliente.id_documento_padre = nextId;
                }
                else
                {
                    cE_DocumentoXcliente.id_documento_hijo = nextId;
                }


                _contextData.documentoXcliente.Add(cE_DocumentoXcliente);
                _contextData.SaveChanges();

                isSave = true;
            }
            catch (Exception ex)
            {

                message = ex.Message;
            }


            return isSave;
        }

        public bool addFile(IFormFile archivo, CE_DocumentoXcliente cE_DocumentoXcliente, out string message)
        {

            message = "";
            bool isSave = false;

            try
            {
                var nombreArchivo = archivo.FileName;

                cE_DocumentoXcliente.nombre_documento_hijo = nombreArchivo;


                byte[] contenidoArchivo;
                using (var memoryStream = new MemoryStream())
                {
                    archivo.CopyTo(memoryStream);
                    contenidoArchivo = memoryStream.ToArray();
                }

                Int64 nextId = _contextData.documentoXcliente
                            .Select(dc => (int?)dc.id)
                            .DefaultIfEmpty()
                            .Max() ?? 0 + 1;

                nextId += 1;

                cE_DocumentoXcliente.id_documento_hijo = nextId;
                cE_DocumentoXcliente.id_tipo_documento = 2;
                cE_DocumentoXcliente.DATA = contenidoArchivo;
                cE_DocumentoXcliente.isPrincipal = false;

                _contextData.documentoXcliente.Add(cE_DocumentoXcliente);
                _contextData.SaveChanges();

                isSave = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return isSave;
        }


        public bool deleteDocByFolder(CE_DocumentoXcliente cE_DocumentoXcliente, out string message)
        {

            bool isDelete = false;
            message = "";

            try
            {
                cliente = cE_DocumentoXcliente.id_cliente;
                padre = cE_DocumentoXcliente.id_documento_padre;

                Table.Columns.Add("id", typeof(Int64));
                Table.Columns.Add("id_cliente", typeof(Int64));
                Table.Columns.Add("id_documento_padre", typeof(Int64));
                Table.Columns.Add("nombre_documento_padre", typeof(string));
                Table.Columns.Add("id_documento_hijo", typeof(Int64));
                Table.Columns.Add("nombre_documento_hijo", typeof(string));
                Table.Columns.Add("id_tipo_documento", typeof(Int64));
                Table.Columns.Add("DATA", typeof(byte[]));
                Table.Columns.Add("isPrincipal", typeof(byte[]));

                var validatePadreQury = (from documentoXcliente in _contextData.documentoXcliente
                                         where (documentoXcliente.id_documento_hijo == padre
                                         && documentoXcliente.id_cliente == cliente)
                                         select new
                                         {
                                             documentoXcliente.id_documento_padre,
                                             documentoXcliente.nombre_documento_padre,
                                             documentoXcliente.id_documento_hijo,
                                             documentoXcliente.nombre_documento_hijo

                                         }).ToList();

                foreach (var item in validatePadreQury)
                {

                    DataRow row = Table.NewRow();
                    row["id_documento_padre"] = item.id_documento_padre;
                    row["nombre_documento_padre"] = item.nombre_documento_padre;
                    row["id_documento_hijo"] = item.id_documento_hijo;
                    row["nombre_documento_hijo"] = item.nombre_documento_hijo;
                    Table.Rows.Add(row);

                };

                recorrerHijos(getHijo(padre));


                foreach (DataRow row in Table.Rows)
                {

                    Int64 id_documento_padre = Convert.ToInt64(row["id_documento_padre"]);
                    Int64 id_documento_hijo = Convert.ToInt64(row["id_documento_hijo"]);


                    var registroAEliminar = _contextData.documentoXcliente.FirstOrDefault(x => x.id_documento_padre == id_documento_padre && x.id_documento_hijo == id_documento_hijo);

                    if (registroAEliminar != null)
                    {
                        // Eliminar el registro
                        _contextData.documentoXcliente.Remove(registroAEliminar);
                        _contextData.SaveChanges(); // Guardar los cambios en la base de datos
                    }

                }

                isDelete = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return isDelete;
        }


        public CE_DocumentoXcliente getFileBytes(CE_DocumentoXcliente cE_DocumentoXcliente, out string message)
        {
            message = "";

            byte[] contenidoArchivo;

            CE_DocumentoXcliente cE_DocumentoXcliente1 = new CE_DocumentoXcliente();
            try
            {
                var files = (from documentoXcliente in _contextData.documentoXcliente
                             where documentoXcliente.id_cliente == cE_DocumentoXcliente.id_cliente
                             where documentoXcliente.id_documento_padre == cE_DocumentoXcliente.id_documento_padre
                             where documentoXcliente.id_documento_hijo == cE_DocumentoXcliente.id_documento_hijo
                             where documentoXcliente.isPrincipal == cE_DocumentoXcliente.isPrincipal
                             select documentoXcliente
                     ).ToList();


                cE_DocumentoXcliente1 = new CE_DocumentoXcliente() {

                    id = files[0].id,
                    id_cliente = files[0].id_cliente,
                    id_documento_padre = files[0].id_documento_padre,
                    nombre_documento_padre = files[0].nombre_documento_padre,
                    nombre_documento_hijo = files[0].nombre_documento_hijo,
                    id_tipo_documento = files[0].id_tipo_documento,
                    DATA = files[0].DATA,
                    isPrincipal = files[0].isPrincipal



                };


               

                return cE_DocumentoXcliente1;
            }
            catch (Exception ex)
            {
                message = ex.Message;

                return cE_DocumentoXcliente1;
            }        

           

        }







            //Metodos auxiliares

            #region Metodos Auxiliares

            public List<CE_DocumentoXcliente> getHijo(Int64 x)
        {

            var query = (from documentoXcliente in _contextData.documentoXcliente
                         where (documentoXcliente.id_documento_padre == x
                         && documentoXcliente.id_cliente == cliente)
                         select new
                         {
                             documentoXcliente.id_documento_padre,
                             documentoXcliente.nombre_documento_padre,
                             documentoXcliente.id_documento_hijo,
                             documentoXcliente.nombre_documento_hijo
                         }).ToList();

            List<CE_DocumentoXcliente> cE_DocumentoXcliente = new List<CE_DocumentoXcliente>();

            foreach (var item in query)
            {

                DataRow row = Table.NewRow();
                row["id_documento_padre"] = item.id_documento_padre;
                row["nombre_documento_padre"] = item.nombre_documento_padre;
                row["id_documento_hijo"] = item.id_documento_hijo;
                row["nombre_documento_hijo"] = item.nombre_documento_hijo;
                Table.Rows.Add(row);

                CE_DocumentoXcliente obj = new CE_DocumentoXcliente
                {
                    id_documento_padre = item.id_documento_padre,
                    nombre_documento_padre = item.nombre_documento_padre,
                    id_documento_hijo = item.id_documento_hijo,
                    nombre_documento_hijo = item.nombre_documento_hijo


                };

                cE_DocumentoXcliente.Add(obj);
            };

            return cE_DocumentoXcliente;

        }

        public void recorrerHijos(List<CE_DocumentoXcliente> obj)
        {

            foreach (var objecto in obj)
            {
                var hijos = getHijo(objecto.id_documento_hijo);

                if (hijos.Count > 0)
                {
                    recorrerHijos(hijos);
                }
            }

        }


        #endregion



    }
}
