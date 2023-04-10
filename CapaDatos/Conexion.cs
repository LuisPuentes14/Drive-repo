using Microsoft.EntityFrameworkCore;
using CapaEntidad;


namespace CapaDatos
{
    public class Conexion : DbContext
    {

        public Conexion(DbContextOptions<Conexion> option) : base(option)
        {
            //
        }

        public DbSet<CE_Cliente> cliente { get; set; }
        public DbSet<CE_DocumentoXcliente> documentoXcliente { get; set; }
        public DbSet<CE_Tipo_documento> tipo_documento { get; set; }


    }
}