using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseOut.Estructuras
{
    public struct InicioSesion
    {
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
    }

    public struct RegistroProductos
    {
        public int ID { get; set; }
        public string Producto { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }

    public class Productos
    { 
        public int Codigo { get; set; }
        public string Producto { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }


        public Productos(int codigo, string producto, string categoria, decimal precio, int cantidad)
        {
            Codigo = codigo;
            Producto = producto;
            Categoria = categoria;
            Precio = precio;
            Cantidad = cantidad;
        }

        public Productos()
        {
        }
    }
}
