using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class CE_DocumentoXcliente
    {
        [Key]
        public Int64 id { get; set;}
        public Int64 id_cliente { get; set; }
        public Int64 id_documento_padre { get; set; }
        public string? nombre_documento_padre { get; set; }
        public Int64 id_documento_hijo { get; set; }
        public string? nombre_documento_hijo { get; set; }
        public Int64 id_tipo_documento { get; set; }
        public byte[]? DATA { get; set; }
        public bool? isPrincipal { get; set; }

    } 
}
