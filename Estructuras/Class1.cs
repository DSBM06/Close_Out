using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloseOut.Estructuras;


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
        public DateTime Fecha { get; set; }

        public string Detalles { get; set; }
        public string TipoMovimiento { get; set; }


    }

    public class Productos
    {
        public int Codigo { get; set; }
        public string Producto { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }


        public Productos(int codigo, string producto1, string categoria, decimal precio, int cantidad)
        {
            Codigo = codigo;
            Producto = producto1;
            Categoria = categoria;
            Precio = precio;
            Cantidad = cantidad;
        }


    }

    public class MovimientoInventario
    {

        public DateTime Fecha{ get; set; }
        public string TipoMovimiento { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public string Detalles { get; set; }

        public MovimientoInventario(DateTime fecha, string tipoMovimiento, string producto, int cantidad, string detalles)
        {
            Fecha = fecha;
            TipoMovimiento = tipoMovimiento;
            Producto = producto;
            Cantidad = cantidad;
            Detalles = detalles;
        }



    }

        
}
