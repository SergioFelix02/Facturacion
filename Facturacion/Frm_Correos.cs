using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturacion
{
    public partial class Frm_Correos : Form
    {


        public string ruta, destinatario;
        private MailMessage correos = new MailMessage();
        private SmtpClient envios = new SmtpClient();


        public string Str_Conexion;
        public Frm_Correos()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                correos.To.Clear();
                correos.Body = txtAsunto.Text;
                correos.Subject = "Colegiatura";
                correos.IsBodyHtml = true;
                correos.To.Add(destinatario);

                if (!string.IsNullOrEmpty(ruta))
                {
                    var archivo = new System.Net.Mail.Attachment(ruta);
                    correos.Attachments.Add(archivo);
                }

                correos.From = new MailAddress(txtCorreo.Text);
                envios.Credentials = new NetworkCredential(txtCorreo.Text, txtPassword.Text);

                envios.Host = "smtp.gmail.com";
                envios.Port = 587;
                envios.EnableSsl = true;

                envios.Send(correos);
                MessageBox.Show("El correo fue enviado correctamente");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensajeria 1.0 vb.net ®", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
