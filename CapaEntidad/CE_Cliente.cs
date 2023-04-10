using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public  class CE_Cliente
    {
        [Key]
        public Int64 id_cliente { get; set; }
        public string? Nombre { get; set; }


    }
}
