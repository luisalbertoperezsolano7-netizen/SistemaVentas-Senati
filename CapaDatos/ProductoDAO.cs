using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class ProductoDAO
    {
        // Singleton (versión corta)[cite: 5]
        private static readonly ProductoDAO _instancia = new ProductoDAO();
        public static ProductoDAO Instancia => _instancia;

        private ProductoDAO() { }

        // Listar productos (si el nombre está vacío, devuelve todos)
        public List<Producto> Listar(string nombre)
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection cn = Conexion.Instancia.ObtenerConexion())
            {
                string sql = "SELECT IdProducto, Nombre, Categoria, Precio, Stock, Activo " +
                             "FROM Producto " +
                             "WHERE Nombre LIKE @nombre " +
                             "ORDER BY IdProducto DESC";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@nombre", "%" + nombre + "%");

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Producto p = new Producto();
                    p.IdProducto = Convert.ToInt32(dr["IdProducto"]);
                    p.Nombre = dr["Nombre"].ToString();
                    p.Categoria = dr["Categoria"].ToString();
                    p.Precio = Convert.ToDecimal(dr["Precio"]);
                    p.Stock = Convert.ToInt32(dr["Stock"]);
                    p.Activo = Convert.ToBoolean(dr["Activo"]);
                    lista.Add(p);
                }
            }
            return lista;
        }

        // Insertar un nuevo producto[cite: 6]
        public void Insertar(Producto p)
        {
            using (SqlConnection cn = Conexion.Instancia.ObtenerConexion())
            {
                string sql = "INSERT INTO Producto (Nombre, Categoria, Precio, Stock, Activo) " +
                             "VALUES (@nombre, @categoria, @precio, @stock, @activo)";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@categoria", p.Categoria);
                cmd.Parameters.AddWithValue("@precio", p.Precio);
                cmd.Parameters.AddWithValue("@stock", p.Stock);
                cmd.Parameters.AddWithValue("@activo", p.Activo);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}