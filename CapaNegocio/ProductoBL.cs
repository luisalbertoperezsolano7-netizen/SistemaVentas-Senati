using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class ProductoBL
    {
        public List<Producto> Buscar(string nombre)
        {
            if (nombre == null)
                nombre = "";
            return ProductoDAO.Instancia.Listar(nombre.Trim());
        }

        // Devuelve una cadena vacía si todo salió bien, o el mensaje de error si falla la validación
        public string Registrar(Producto p)
        {
            if (string.IsNullOrWhiteSpace(p.Nombre))
                return "Ingrese el nombre del producto.";

            if (string.IsNullOrWhiteSpace(p.Categoria))
                return "Seleccione una categoría.";

            if (p.Precio <= 0)
                return "El precio debe ser mayor a 0.";

            // Si pasa las validaciones, manda a guardar a la capa de datos usando el Singleton
            ProductoDAO.Instancia.Insertar(p);
            return "";
        }
    }
}