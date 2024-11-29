using CloseOut.Estructuras;
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
using static CloseOut.Estructuras.Productos;

namespace Proyecto_Final_CloseOut.Formularios
{
    public partial class Form5 : Form
    {
        public static List<Productos> productos = new List<Productos>() { };

        private Form2 form2;
        internal static object historialMovimientos;

        public Form5(Form2 form2)
        {
            InitializeComponent();
            this.form2 = form2;
            CargarDatosEnGrafico();
            ActualizarMensajeEstado();
        }
        public Form5()
        {
            InitializeComponent();
            CargarDatosEnGrafico();
            ActualizarMensajeEstado();
        }

        private bool ValidarProducto(string nombreProducto)
        {
            if (string.IsNullOrWhiteSpace(nombreProducto))
            {
                MessageBox.Show("El nombre del producto no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
        }

        private void groupBox1_Enter(object sender, EventArgs e){}

        private void tstAgregar_Click(object sender, EventArgs e)
        {
            AgregarProducto();
            ActualizarMensajeEstado();
            CuentaProductosCategoria();

            string nombreProducto = txtNombre.Text;

            if (ValidarProducto(nombreProducto)==true)
            {
                MessageBox.Show("El producto fue agregado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }

        }

        private void AgregarProducto()
        {
            int nuevoCódigo = int.Parse(txtCodigo.Text);
            string nuevoProducto = txtNombre.Text;
            string nuevaCategoria = cmbCategoría.SelectedItem.ToString();
            decimal nuevoPrecio = decimal.Parse(txtPrecio.Text);
            int nuevaCantidad = int.Parse(txtStock.Text);

            productos.Add(new Productos(nuevoCódigo, nuevoProducto, nuevaCategoria, nuevoPrecio, nuevaCantidad));
            ActualizarDataGridView();
            LimpiarCampos();
            CuentaProductosCategoria();
            CargarDatosEnGrafico();

            // Registro del movimiento en el historial
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
            dgvZapatos.DataSource = null;
            dgvZapatos.DataSource = Form5.productos;
        }

        private void tstEliminar_Click(object sender, EventArgs e)
        {
            int i = dgvZapatos.CurrentCell.RowIndex;
            var productoEliminado = productos[i];
            productos.RemoveAt(i);
            ActualizarDataGridView();
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
            CargarDatosEnGrafico();

            // Registro del movimiento en el historial
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
            int Cantidad = int.Parse(txtStock.Text);

            Productos producto = new Productos(Codigo, Producto, Categoria, Precio, Cantidad);

            int index = productos.FindIndex(p => p.Codigo == producto.Codigo);
            if (index >= 0)
            {
                productos[index] = producto;
            }

            ActualizarDataGridView();
            MessageBox.Show("Producto actualizado con éxito.");

            // Registro del movimiento en el historial
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

        private void ActualizarMensajeEstado()
        {
            int totalStock = 0;
            foreach (var producto in productos)
            {
                totalStock += producto.Cantidad;

                // Verificar si el stock está por debajo del nivel requerido
                if (producto.Cantidad < 10)
                {
                    MessageBox.Show($"El producto {producto.Producto} está bajo en stock. Quedan solo {producto.Cantidad} unidades.", "Advertencia de Stock Bajo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            toolStripStatusLabel.Text = $"Número de productos en inventario de zapatos: {totalStock}";
        }


        private void toolStripButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ActualizarDataGridView();
        }

        private void dgvZapatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
