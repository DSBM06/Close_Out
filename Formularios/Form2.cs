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
using static CloseOut.Estructuras.Productos;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Documents.Serialization;

namespace Proyecto_Final_CloseOut.Formularios
{
    public partial class Form2 : Form
    {
        private List<RegistroProductos> productos = new List<RegistroProductos>();


        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;

        public static List<MovimientoInventario> historialMovimientos = new List<MovimientoInventario>();
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

        private void MostrarHistorial()
        {
            dgvHistorial.DataSource = null;
            dgvHistorial.DataSource = historialMovimientos;
        }

        private void btnMostrarHistorial_Click(object sender, EventArgs e)
        {
            MostrarHistorial();
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
                saveFileDialog1.Filter = "Archivo binario (*.bin)|*.bin";
                saveFileDialog1.Title = "Guardar archivo de productos";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;

                    var productos = (List<RegistroProductos>)dataGridView1.DataSource;

                    if (productos == null || productos.Count == 0)
                    {
                        MessageBox.Show("No hay productos para guardar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        
                        writer.Write(productos.Count);

                        
                        foreach (var producto in productos)
                        {
                            writer.Write(producto.ID);               
                            writer.Write(producto.Producto);         
                            writer.Write(producto.Precio);            
                            writer.Write(producto.Categoria);         
                            writer.Write(producto.Cantidad);          
                        }
                    }

                    MessageBox.Show("Archivo binario guardado exitosamente.", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void cargarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "Archivo binario (*.bin)|*.bin";
                openFileDialog1.Title = "Abrir archivo de productos";

                
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog1.FileName;

                    
                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("El archivo seleccionado no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    
                    List<RegistroProductos> productos = new List<RegistroProductos>();

                 
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        
                        int count = reader.ReadInt32();

                  
                        if (count == 0)
                        {
                            MessageBox.Show("El archivo está vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        
                        for (int i = 0; i < count; i++)
                        {
                            
                            if (reader.BaseStream.Position + sizeof(int) + sizeof(decimal) > reader.BaseStream.Length)
                            {
                                MessageBox.Show("El archivo tiene un formato incorrecto o está dañado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            
                            int id = reader.ReadInt32();
                            string nombre = reader.ReadString();
                            decimal precio = reader.ReadDecimal();
                            string categoria = reader.ReadString();
                            int cantidad = reader.ReadInt32();

                            
                            productos.Add(new RegistroProductos
                            {
                                ID = id,
                                Producto = nombre,
                                Precio = precio,
                                Categoria = categoria,
                                Cantidad = cantidad
                            });
                        }
                    }

                    
                    dataGridView1.DataSource = productos;

                    
                    MessageBox.Show("Archivo binario cargado exitosamente.", "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var pantalones = Form4.productos.Where(p => p.Categoria.Contains("Pantalón")).ToList();
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
                saveFileDialog1.Filter = "Archivo binario (*.bin)|*.bin";
                saveFileDialog1.Title = "Guardar archivo de productos";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;

                    var productos = (List<RegistroProductos>)dataGridView1.DataSource;

                    if (productos == null || productos.Count == 0)
                    {
                        MessageBox.Show("No hay productos para guardar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        
                        writer.Write(productos.Count);

                        
                        foreach (var producto in productos)
                        {
                            writer.Write(producto.ID);                
                            writer.Write(producto.Producto);          
                            writer.Write(producto.Precio);            
                            writer.Write(producto.Categoria);         
                            writer.Write(producto.Cantidad);         
                        }
                    }

                    MessageBox.Show("Archivo binario guardado exitosamente.", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            try
            {
                
                openFileDialog1.Filter = "Archivo binario (*.bin)|*.bin";
                openFileDialog1.Title = "Abrir archivo de productos";

          
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog1.FileName;

                  
                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("El archivo seleccionado no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                   
                 

                    
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        
                        int count = reader.ReadInt32();

                        
                        if (count == 0)
                        {
                            MessageBox.Show("El archivo está vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        
                        for (int i = 0; i < count; i++)
                        {
                            
                            if (reader.BaseStream.Position + sizeof(int) + sizeof(decimal) > reader.BaseStream.Length)
                            {
                                MessageBox.Show("El archivo tiene un formato incorrecto o está dañado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            
                            int id = reader.ReadInt32();
                            string nombre = reader.ReadString();
                            decimal precio = reader.ReadDecimal();
                            string categoria = reader.ReadString();
                            int cantidad = reader.ReadInt32();

                            
                            productos.Add(new RegistroProductos
                            {
                                ID = id,
                                Producto = nombre,
                                Precio = precio,
                                Categoria = categoria,
                                Cantidad = cantidad
                            });
                        }
                    }

                    
                    dataGridView1.DataSource = productos;

                    
                    MessageBox.Show("Archivo binario cargado exitosamente.", "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
               
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void manualDeUsoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnMostrarHistorial_Click_1(object sender, EventArgs e)
        {
            MostrarHistorial();
        }

        private void historialDeIngresoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pantalonesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void camisasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


    }
}