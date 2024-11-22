using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using CloseOut.Estructuras;
using Proyecto_Final_CloseOut.Formularios;
using Newtonsoft.Json;
using System.IO;
using Proyecto_Final_CloseOut.Servicios;
using System.Collections.Concurrent;

namespace Proyecto_Final_CloseOut.Formularios
{
    public partial class Form2 : Form
    {
        private List<RegistroProductos> productos;
        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;
        public Form2()
        {
            InitializeComponent();
            saveFileDialog1 = new SaveFileDialog();
            openFileDialog1 = new OpenFileDialog();
        }

        public void ActualizarDataGridView()
        {
            productos = new List<RegistroProductos>();

            if (Form3.productos != null)
            {
                productos.AddRange(Form3.productos.Select(p => new RegistroProductos
                {
                    ID = p.Codigo,
                    Producto = p.Producto,
                    Categoria = p.Categoria,
                    Precio = p.Precio,
                    Cantidad = p.Cantidad
                }));
            }

            if (Form4.productos != null)
            {
                productos.AddRange(Form4.productos.Select(p => new RegistroProductos
                {
                    ID = p.Codigo,
                    Producto = p.Producto,
                    Categoria = p.Categoria,
                    Precio = p.Precio,
                    Cantidad = p.Cantidad
                }));
            }

            if (Form5.productos != null)
            {
                productos.AddRange(Form5.productos.Select(p => new RegistroProductos
                {
                    ID = p.Codigo,
                    Producto = p.Producto,
                    Categoria = p.Categoria,
                    Precio = p.Precio,
                    Cantidad = p.Cantidad
                }));
            }

            if (Form6.productos != null)
            {
                productos.AddRange(Form6.productos.Select(p => new RegistroProductos
                {
                    ID = p.Codigo,
                    Producto = p.Producto,
                    Categoria = p.Categoria,
                    Precio = p.Precio,
                    Cantidad = p.Cantidad
                }));
            }

            MostrarDatos();
        }

        private void Form2_Load(object sender, EventArgs e)
        { }

        private void groupBox1_Enter(object sender, EventArgs e)
        { }

        private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { }

        private void actualizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActualizarDataGridView();
        }

        private void informeCamisetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder informe = new StringBuilder();
            informe.AppendLine("Informe de Camisetas:");
            informe.AppendLine("----------------------------");

            foreach (var producto in Form3.productos)
            {
                informe.AppendLine($"Nombre: {producto.Producto}, Precio: {producto.Precio}, Cantidad: {producto.Cantidad}");
            }

            MessageBox.Show(informe.ToString(), "Informe de Camisetas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "Archivo JSON (*.json)|*.json";
                saveFileDialog1.Title = "Guardar archivo de productos";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;
                    var productos = (List<RegistroProductos>)dataGridView1.DataSource;
                    string json = JsonConvert.SerializeObject(productos, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(filePath, json);
                    MessageBox.Show("Archivo guardado exitosamente.", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cargarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "Archivo JSON (*.json)|*.json";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string ruta = openFileDialog1.FileName;
                    string json = File.ReadAllText(ruta);
                    productos = JsonConvert.DeserializeObject<List<RegistroProductos>>(json);

                    if (productos != null && productos.Count > 0)
                    {
                        ActualizarDataGridView();
                        MessageBox.Show("Archivo cargado exitosamente.", "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("El archivo no contiene datos o está vacío.", "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No se seleccionó ningún archivo.", "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void MostrarDatos()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = productos;
        }

        private void inventarioPantalonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 frm = new Form4();
            frm.Show();
        }

        private void informePantalonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder informe = new StringBuilder();
            informe.AppendLine("Informe de Pantalones:");
            informe.AppendLine("----------------------------");

            foreach (var producto in Form4.productos)
            {
                informe.AppendLine($"Nombre: {producto.Producto}, Precio: {producto.Precio}, Cantidad: {producto.Cantidad}");
            }

            MessageBox.Show(informe.ToString(), "Informe de Pantalones", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void informeZapatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder informe = new StringBuilder();
            informe.AppendLine("Informe de Zapatos:");
            informe.AppendLine("----------------------------");

            foreach (var producto in Form5.productos)
            {
                informe.AppendLine($"Nombre: {producto.Producto}, Precio: {producto.Precio}, Cantidad: {producto.Cantidad}");
            }

            MessageBox.Show(informe.ToString(), "Informe de Zapatos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void informeSuetersChaquetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder informe = new StringBuilder();
            informe.AppendLine("Informe de Sueters/Chaquetas:");
            informe.AppendLine("----------------------------");

            foreach (var producto in Form6.productos)
            {
                informe.AppendLine($"Nombre: {producto.Producto}, Precio: {producto.Precio}, Cantidad: {producto.Cantidad}");
            }

            MessageBox.Show(informe.ToString(), "Informe de Sueters/Chaquetas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void inventarioZapatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 frm = new Form5();
            frm.Show();
        }

        private void inventarioSuetersChaquetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 frm = new Form6();
            frm.Show();
        }

        private void informeGeneralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder informe = new StringBuilder();
            informe.AppendLine("Informe General del dia:");
            informe.AppendLine("----------------------------");

            foreach (var producto in productos)
            {
                informe.AppendLine($"Nombre: {producto.Producto}, Precio: {producto.Precio}, Cantidad: {producto.Cantidad}");
            }

            MessageBox.Show(informe.ToString(), "Informe de General del dia", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void opcionesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            ActualizarDataGridView();

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "Archivo JSON (*.json)|*.json";
                saveFileDialog1.Title = "Guardar archivo de productos";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;
                    var productos = (List<RegistroProductos>)dataGridView1.DataSource;
                    string json = JsonConvert.SerializeObject(productos, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(filePath, json);
                    MessageBox.Show("Archivo guardado exitosamente.", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "Archivo JSON (*.json)|*.json";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string ruta = openFileDialog1.FileName;
                    string json = File.ReadAllText(ruta);
                    productos = JsonConvert.DeserializeObject<List<RegistroProductos>>(json);

                    if (productos != null && productos.Count > 0)
                    {
                        ActualizarDataGridView();
                        MessageBox.Show("Archivo cargado exitosamente.", "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("El archivo no contiene datos o está vacío.", "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No se seleccionó ningún archivo.", "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void manualDeUsoToolStripMenuItem_Click(object sender, EventArgs e)
        {}
    }
}