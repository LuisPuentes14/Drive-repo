using CapaEntidad;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Cliente
    {

        private readonly Conexion _contextData;

        public CD_Cliente(Conexion contextData)
        {
            _contextData = contextData;
        }

        public List<CE_Cliente> list()
        {
            List<CE_Cliente> oCE_Cliente = new List<CE_Cliente>();

            oCE_Cliente = _contextData.cliente.ToList();

            return oCE_Cliente;
        }


        //public bool delete(Int64 id, out string message)
        //{
           



        //    return true;
        //}


        //public bool create(CE_Cliente oCE_Cliente)
        //{
           

        //    return true;
        //}


        //public bool update(CE_Cliente oCE_Cliente)
        //{


        //    return true;
        //}



    }
}
