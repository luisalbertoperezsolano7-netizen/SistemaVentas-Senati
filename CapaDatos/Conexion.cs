using System.Data.SqlClient;

namespace CapaDatos
{
    public sealed class Conexion
    {
        private static Conexion _instancia;

        // Cadena de conexión apuntando a tu servidor SQL Server Express
        private readonly string _cadena = "Server=.\\SQLEXPRESS; Database=BDVentas; Integrated Security=true; TrustServerCertificate=true";

        // Constructor PRIVADO: nadie puede hacer "new Conexion()" desde afuera[cite: 4]
        private Conexion() { }

        // Única puerta de entrada (Patrón Singleton)[cite: 4]
        public static Conexion Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new Conexion();
                return _instancia;
            }
        }

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_cadena);
        }
    }
}
