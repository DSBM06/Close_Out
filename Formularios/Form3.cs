using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CloseOut.Estructuras;
using Proyecto_Final_CloseOut.Formularios;
using static CloseOut.Estructuras.Productos;
using CloseOut.Estructuras;

namespace Proyecto_Final_CloseOut.Formularios
{
    public partial class Form3 : Form
    {
        public static List<Productos> productos = new List<Productos>() { };

        private Form2 form2;
        internal static object historialMovimientos;

        public Form3()
        {
            InitializeComponent();
            CargarDatosEnGrafico();
            ActualizarMensajeEstado();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
        }

        private void Form3_Load(object sender, EventArgs e) { }

        private void tstAgregar_Click(object sender, EventArgs e)
        {
            AgregarProducto();
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
        }

        private void AgregarProducto()
        {
            int nuevoCodigo = int.Parse(txtCodigo.Text);
            string nuevoProducto = txtNombre.Text;
            string nuevaCategoria = cmbCategoría.SelectedItem.ToString();
            decimal nuevoPrecio = decimal.Parse(txtPrecio.Text);
            int nuevaCantidad = int.Parse(txtStock.Text);

            productos.Add(new Productos(nuevoCodigo, nuevoProducto, nuevaCategoria, nuevoPrecio, nuevaCantidad));
            ActualizarDataGridView();
            LimpiarCampos();
            CuentaProductosCategoria();
            CargarDatosEnGrafico();

            Form2.historialMovimientos.Add(new MovimientoInventario
            {
                Fecha = DateTime.Now,
                TipoMovimiento = "Ingreso",
                Producto = nuevoProducto,
                Cantidad = nuevaCantidad,
                Detalles = $"Se agregó el producto {nuevoProducto} con cantidad {nuevaCantidad}."
            });
        }

        private void ActualizarDataGridView()
        {
            dgvCamisetas.DataSource = null;
            dgvCamisetas.DataSource = productos;
        }

        private void tstEliminar_Click(object sender, EventArgs e)
        { 
            int i = dgvCamisetas.CurrentCell.RowIndex;
            var productoEliminado = productos[i];

            productos.RemoveAt(i);
            ActualizarDataGridView();
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
            CargarDatosEnGrafico();

            Form2.historialMovimientos.Add(new MovimientoInventario
            {
                Fecha = DateTime.Now,
                TipoMovimiento = "Salida",
                Producto = productoEliminado.Producto,
                Cantidad = productoEliminado.Cantidad,
                Detalles = $"Se eliminó el producto {productoEliminado.Producto} con cantidad {productoEliminado.Cantidad}."
            });
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {

            int Codigo = int.Parse(txtCodigo.Text);
             string Producto = txtNombre.Text;
             string Categoria = cmbCategoría.SelectedItem.ToString();
               decimal Precio = decimal.Parse(txtPrecio.Text);
              int   Cantidad = int.Parse(txtStock.Text);

            Productos producto = new Productos(Codigo, Producto, Categoria, Precio, Cantidad);

            int index = productos.FindIndex(p => p.Codigo == producto.Codigo);
            if (index >= 0)
            {
                productos[index] = producto;
            }

            ActualizarDataGridView();
            MessageBox.Show("Producto actualizado con éxito.");

            
            Form2.historialMovimientos.Add(new MovimientoInventario
            {
                Fecha = DateTime.Now,
                TipoMovimiento = "Actualización",
                Producto = producto.Producto,
                Cantidad = producto.Cantidad,
                Detalles = $"Se actualizó el producto {producto.Producto} con cantidad {producto.Cantidad}."
            });
        }

        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            cmbCategoría.SelectedIndex = -1;
            txtPrecio.Clear();
            txtStock.Clear();
        }

        private void CuentaProductosCategoria()
        {
            var productosPorCategoria = productos
                  .GroupBy(p => p.Categoria)
                  .Select(g => new { Categoria = g.Key, Cantidad = g.Count() })
                  .ToList();

            string mensaje = "";
            foreach (var categoria in productosPorCategoria)
            {
                mensaje += $"{categoria.Categoria}: {categoria.Cantidad}" + Environment.NewLine;
            }

            txtProductosPorCategoria.Text = mensaje;
        }

        private void CargarDatosEnGrafico()
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Titles.Add("Cantidad de Productos por Categoría");
            chart1.ChartAreas[0].AxisX.Title = "Categorías";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Title = "Cantidad";
            chart1.ChartAreas[0].AxisY.Maximum = 5;
            chart1.ChartAreas[0].AxisY.Interval = 1;

            Series serie = new Series("Productos")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            var productosPorCategoria = productos
               .GroupBy(p => p.Categoria)
               .Select(g => new { Categoria = g.Key, Cantidad = g.Count() })
               .ToList();

            foreach (var categoria in productosPorCategoria)
            {
                serie.Points.AddXY(categoria.Categoria, categoria.Cantidad);
            }

            chart1.Series.Add(serie);
            chart1.Invalidate();
        }

  

        private void toolStripButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        { }

        private void dgvCamisetas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ActualizarDataGridView();
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {

        }
        private void ActualizarMensajeEstado()
        {
            int totalStock = 0;
            foreach (var producto in productos)
            {
                totalStock += producto.Cantidad;

                if (producto.Cantidad < 5)
                {
                    MessageBox.Show($"El producto {producto.Producto} está bajo en stock. Quedan solo {producto.Cantidad} unidades.", "Advertencia de Stock Bajo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            toolStripStatusLabel.Text = $"Número de productos en inventario de camisetas: {totalStock}";
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            ActualizarDataGridView();
        }
    }
}
