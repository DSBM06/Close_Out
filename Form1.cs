using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloseOut.Estructuras;
using Proyecto_Final_CloseOut.Formularios;

namespace Proyecto_Final_CloseOut
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {}

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuarioIngresado = txtUsuario.Text;
            string contrasena = txtContraseña.Text;

            string usuarioCorrecto = "Kevin";
            string contrasenaCorrecta = "1234";

            Form2 nuevoFormulario = new Form2();


            if (usuarioIngresado == usuarioCorrecto && contrasena == contrasenaCorrecta)
            {
                MessageBox.Show("Bienvenido Kevin.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                


                nuevoFormulario.Show();


                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            
        }

        private void gBxInicio_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
