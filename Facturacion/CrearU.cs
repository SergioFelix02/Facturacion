using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Facturacion
{
    public partial class CrearU : Form

    {
        SqlConnection cn;
        public String Str_Conexion;
        public CrearU()
        {
            InitializeComponent();
        }

        private void CrearU_Load(object sender, EventArgs e)
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

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Login frm = new Login();
            frm.ShowDialog();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPassword1.Text.Trim() != txtPassword2.Text.Trim())
            {
                MessageBox.Show("Las Contraseñas NO son Iguales");
            }
            else if (txtPassword1.Text == "" || txtPassword2.Text == "" || txtUsuario.Text == ""){
                MessageBox.Show("Llena todos los campos");
            }
            else
            {
                string encryptar;
                encryptar = codificar(txtPassword1.Text, txtPassword2.Text);
                if (encryptar.Trim() != "")
                {
                    cn = new SqlConnection(Str_Conexion);
                    string sqlstr = "UPDATE PARMS SET PA_NOMUS='" + txtUsuario.Text + "', PA_PASS='" + encryptar + "'";
                    SqlCommand cmd = new SqlCommand(sqlstr, cn);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    MessageBox.Show(" DATOS ACTUALIZADOS");
                }
            }
        }
        public static string codificar(string entrada, string clave)
        {
            TripleDESCryptoServiceProvider triple = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider triplemd5 = new MD5CryptoServiceProvider();
            try
            {
                if (entrada.Trim() != "")
                {
                    triple.Key = triplemd5.ComputeHash(Encoding.Default.GetBytes(clave));
                    triple.Mode = CipherMode.ECB;
                    ICryptoTransform desencrypar = triple.CreateEncryptor();
                    byte[] buff = Encoding.Default.GetBytes(entrada);
                    return Convert.ToBase64String(desencrypar.TransformFinalBlock(buff, 0, buff.Length));
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


    }
}
