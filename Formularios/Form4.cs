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

namespace Proyecto_Final_CloseOut.Formularios
{
    public partial class Form4 : Form
    {
        public static List<Productos> productos = new List<Productos>() { };

        private Form2 form2;

        public Form4(Form2 form2)
        {
            InitializeComponent();
            this.form2 = form2;
            CargarDatosEnGrafico();
            ActualizarMensajeEstado();
        }
        public Form4()
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

        private void Form4_Load(object sender, EventArgs e)
        {}

        private void tstAgregar_Click(object sender, EventArgs e)
        {
            AgregarProducto();
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
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
        }

        private void ActualizarDataGridView()
        {
            dgvPantalones.DataSource = null;
            dgvPantalones.DataSource = Form4.productos;
        }

        private void tstEliminar_Click(object sender, EventArgs e)
        {
            int i = dgvPantalones.CurrentCell.RowIndex;
            productos.RemoveAt(i);
            ActualizarDataGridView();
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
            CargarDatosEnGrafico();
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
            }

            toolStripStatusLabel.Text = $"Número de productos en inventario de pantalones: {totalStock}";
        }

        private void toolStripButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ActualizarDataGridView();
        }

        private void toolStripButton_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripSalir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void txtProductosPorCategoria_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
