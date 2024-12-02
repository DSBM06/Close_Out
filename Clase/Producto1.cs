using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final_CloseOut.Clase
{
    public class Producto1
    {
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; }
        public string Producto { get; set; }
        public Int32 Cantidad { get; set; }
        public string Detalles { get; set; }

        public Producto1(DateTime fecha, string tipoMovimiento, string producto, int cantidad, string detalles)
        {
            Fecha = fecha;
            TipoMovimiento = tipoMovimiento;
            Producto = producto;
            Cantidad = cantidad;
            Detalles = detalles;
        }
    }
}
