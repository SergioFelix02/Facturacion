using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Facturacion
{
    public partial class Login : Form
    {
        SqlConnection cn;
        private SqlDataAdapter dbControl;
        private DataSet tbControl;
        private DataRow regControl;
        protected string Str_Conexion = "SERVER=.;DATABASE=PREESCOLAR;USER=SA;PASSWORD=Safp270602;";

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUsuario.Select();
            cn = new SqlConnection();
            cn.ConnectionString = Str_Conexion;
            try
            {
                cn.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Problemas de Conexión");
            }
        }

        public static string decodificar(string entrada, string clave)

        {
            TripleDESCryptoServiceProvider triple = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider triplemd5 = new MD5CryptoServiceProvider();
            try
            {
                if (entrada.Trim() != "")
                {
                    triple.Key = triplemd5.ComputeHash(Encoding.Default.GetBytes(clave));
                    triple.Mode = CipherMode.ECB;
                    ICryptoTransform desencrypar = triple.CreateDecryptor();
                    byte[] buff = Convert.FromBase64String(entrada);
                    return Encoding.Default.GetString(desencrypar.TransformFinalBlock(buff, 0, buff.Length));
                }
                else
                {
                    return "";
                }
            }
            catch (Exception exception)
            {
                return "";
                throw exception;
            }
            finally
            {
                triple = null;
                triplemd5 = null;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            CrearU frm = new CrearU();
            frm.Str_Conexion = Str_Conexion;
            frm.ShowDialog();
            Close();
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            string sqlstr = "SELECT * FROM PARMS";
            SqlCommand cmd = new SqlCommand(sqlstr, cn);
            dbControl = new SqlDataAdapter(cmd);
            tbControl = new DataSet();
            dbControl.Fill(tbControl, "PARMS");
            regControl = tbControl.Tables["PARMS"].Rows[0];
            string desencyptar;
            desencyptar = decodificar(Convert.ToString(regControl["PA_PASS"]), txtPassword.Text);

            if ((desencyptar.Trim() != txtPassword.Text.Trim()) || (txtUsuario.Text != Convert.ToString(regControl["PA_NOMUS"])))
            {
                MessageBox.Show("Usuario o Contraseña Incorrecta");
            }
            else
            {
                this.Visible = false;
                Main frm = new Main();
                frm.Str_Conexion = Str_Conexion;
                frm.ciclo = $"{regControl["PA_CICLO"]}";
                frm.ShowDialog();
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label4_MouseClick(object sender, MouseEventArgs e)
        {
            txtUsuario.Text = "sergio";
            txtPassword.Text = "123";
        }
    }
}
