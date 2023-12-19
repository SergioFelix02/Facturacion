using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturacion
{

    public partial class Main : Form
    {
        public string ciclo;
        public string Str_Conexion;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.menuStrip1.BackColor = Color.FromArgb(26,26,26);
            this.menuStrip1.ForeColor = Color.White;
            cboCiclo.Text = ciclo;
            cboCiclo.DropDownStyle = ComboBoxStyle.DropDownList;
            //cboCiclo.SelectedIndex = 0;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Login frm = new Login();
            frm.ShowDialog();
            Close();
        }

        private void envioDeRecibosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Recibos frm = new Frm_Recibos();
            frm.Str_Conexion = Str_Conexion;
            frm.ciclo = ciclo;
            frm.ShowDialog();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void facturacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Frm_Correos frm = new Frm_Correos();
            //frm.Str_Conexion = Str_Conexion;
            //frm.ShowDialog();
        }
    }
}
