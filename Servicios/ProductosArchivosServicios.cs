using CloseOut.Estructuras;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloseOut.Estructuras;
using Proyecto_Final_CloseOut.Formularios;

namespace Proyecto_Final_CloseOut.Servicios
{
    internal class ProductosArchivosServicios
    {
        public void GuardarArchivo(List<RegistroProductos> productos, string rutaArchivo)
        {
            using (FileStream archivo = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter escritor = new BinaryWriter(archivo))
                {
                    foreach (RegistroProductos producto in productos)
                    {
                        escritor.Write(producto.ID);
                        escritor.Write(producto.Producto.Length);
                        escritor.Write(producto.Producto.ToCharArray());
                        escritor.Write(producto.Categoria.Length);
                        escritor.Write(producto.Categoria.ToCharArray());
                        escritor.Write(producto.Precio);
                        escritor.Write(producto.Cantidad);
                    }
                }
            }
        }

        public List<RegistroProductos> CargarProductos(string rutaArchivo)
        {
            List<RegistroProductos> productos = new List<RegistroProductos>();
            if (!File.Exists(rutaArchivo))
            {
                return productos;
            }

            using (FileStream archivo = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader lector = new BinaryReader(archivo))
                {
                    while (archivo.Position != archivo.Length)
                    {
                        int id = lector.ReadInt32();
                        int tamañoProducto = lector.ReadInt32();
                        char[] productoArray = lector.ReadChars(tamañoProducto);
                        string producto = new string(productoArray);

                        int tamañoCategoria = lector.ReadInt32();
                        char[] categoriaArray = lector.ReadChars(tamañoCategoria);
                        string categoria = new string(categoriaArray);

                        decimal precio = lector.ReadDecimal();
                        int cantidad = lector.ReadInt32();

                        RegistroProductos registroProducto = new RegistroProductos
                        {
                            ID = id,
                            Producto = producto,
                            Categoria = categoria,
                            Precio = precio,
                            Cantidad = cantidad
                        };

                        productos.Add(registroProducto);
                    }
                }
            }
            return productos;
        }
    }
}
