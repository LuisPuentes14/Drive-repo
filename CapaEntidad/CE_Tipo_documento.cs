using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class CE_Tipo_documento
    {
        [Key]
        public Int64 id_tipo_documento { get; set; }
        public string? Nombre { get; set; }

    }
}
