using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
using Microsoft.Extensions.Logging;

namespace CapaNegocio
{
    public  class CN_Cliente
    {

        
        private readonly Conexion _contextData;
        public CN_Cliente( Conexion contextData)
        {          
            _contextData = contextData;
        }


        public List<CE_Cliente> list()
        {
            List<CE_Cliente> oCE_Cliente = new CD_Cliente(_contextData).list();          

            return oCE_Cliente;
        }







    }
}
