using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;


namespace Facturacion
{
    public partial class Frm_Recibos : Form
    {
        SqlConnection cn;
        private SqlDataAdapter dbAlumnos, dbMes;
        private DataSet tbAlumnos, tbMes;
        private DataRow regAlumnos, regMes;
        public string Str_Conexion;
        public string ciclo;
        PrintPreviewDialog imprime = new PrintPreviewDialog();
        private int lineas = 0;
        string NumLetras = "";
        string ruta = "";
        string destinatario = "";

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            e.Graphics.DrawImage(Image.FromFile(Path.Combine(Application.StartupPath, "Recibo.png")), 10, 50, 800, 1000);
            Font font = new Font("Arial", 16, FontStyle.Regular);
            for (int i = lineas; i <= tbAlumnos.Tables["ALUMNOS"].Rows.Count - 1; i++)
            {
                regAlumnos = tbAlumnos.Tables["ALUMNOS"].Rows[lineas];
                regMes = tbMes.Tables["MES"].Rows[lineas];

                e.Graphics.DrawString($"{Convert.ToDateTime(regMes["M_FECHA"]).ToString("dd/MM/yyyy")}", font, Brushes.Black, 590, 200);
                e.Graphics.DrawString($"{regMes["M_FAC0"]}", font, Brushes.Black, 600, 130);
                e.Graphics.DrawString($"{regMes["REFERENCIA"]}", font, Brushes.Black, 200, 400);
                e.Graphics.DrawString($"${regMes["M_IMPORTE"]}", font, Brushes.Black, 125, 570);
                e.Graphics.DrawString($"{regAlumnos["AL_PATERNO"]} {regAlumnos["AL_MATERNO"]} {regAlumnos["AL_NOMBRES"]}", font, Brushes.Black, 200, 300);
                NumLetras = Letras(Convert.ToDouble(regMes["M_IMPORTE"]));
                e.Graphics.DrawString($"${NumLetras}", font, Brushes.Black, 200, 570);
                lineas++;
                if (i <= tbAlumnos.Tables["ALUMNOS"].Rows.Count - 2)
                    e.HasMorePages = true;
                break;

            }
        }

        public static string Letras(double numberAsString)
        {
            string dec;

            var entero = Convert.ToInt64(Math.Truncate(numberAsString));
            var decimales = Convert.ToInt32(Math.Round((numberAsString - entero) * 100, 2));
            if (decimales > 0)
            {
                //dec = " PESOS CON " + decimales.ToString() + "/100";
                dec = $" PESOS {decimales:0,0} /100";
            }
            //Código agregado por mí
            else
            {
                //dec = " PESOS CON " + decimales.ToString() + "/100";
                dec = $" PESOS {decimales:0,0} /100";
            }
            var res = NumeroALetras(Convert.ToDouble(entero)) + dec;
            return res;
        }
        // [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static string NumeroALetras(double value)
        {
            string num2Text; value = Math.Truncate(value);
            if (value == 0) num2Text = "CERO";
            else if (value == 1) num2Text = "UNO";
            else if (value == 2) num2Text = "DOS";
            else if (value == 3) num2Text = "TRES";
            else if (value == 4) num2Text = "CUATRO";
            else if (value == 5) num2Text = "CINCO";
            else if (value == 6) num2Text = "SEIS";
            else if (value == 7) num2Text = "SIETE";
            else if (value == 8) num2Text = "OCHO";
            else if (value == 9) num2Text = "NUEVE";
            else if (value == 10) num2Text = "DIEZ";
            else if (value == 11) num2Text = "ONCE";
            else if (value == 12) num2Text = "DOCE";
            else if (value == 13) num2Text = "TRECE";
            else if (value == 14) num2Text = "CATORCE";
            else if (value == 15) num2Text = "QUINCE";
            else if (value < 20) num2Text = "DIECI" + NumeroALetras(value - 10);
            else if (value == 20) num2Text = "VEINTE";
            else if (value < 30) num2Text = "VEINTI" + NumeroALetras(value - 20);
            else if (value == 30) num2Text = "TREINTA";
            else if (value == 40) num2Text = "CUARENTA";
            else if (value == 50) num2Text = "CINCUENTA";
            else if (value == 60) num2Text = "SESENTA";
            else if (value == 70) num2Text = "SETENTA";
            else if (value == 80) num2Text = "OCHENTA";
            else if (value == 90) num2Text = "NOVENTA";
            else if (value < 100) num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10);
            else if (value == 100) num2Text = "CIEN";
            else if (value < 200) num2Text = "CIENTO " + NumeroALetras(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) num2Text = "QUINIENTOS";
            else if (value == 700) num2Text = "SETECIENTOS";
            else if (value == 900) num2Text = "NOVECIENTOS";
            else if (value < 1000) num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
            else if (value == 1000) num2Text = "MIL";
            else if (value < 2000) num2Text = "MIL " + NumeroALetras(value % 1000);
            else if (value < 1000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value % 1000);
                }
            }
            else if (value == 1000000)
            {
                num2Text = "UN MILLON";
            }
            else if (value < 2000000)
            {
                num2Text = "UN MILLON " + NumeroALetras(value % 1000000);
            }
            else if (value < 1000000000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
                }
            }
            else if (value == 1000000000000) num2Text = "UN BILLON";
            else if (value < 2000000000000) num2Text = "UN BILLON " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
            }
            return num2Text;
        }

        public Frm_Recibos()
        {
            InitializeComponent();
        }

        private void Frm_Recibos_Load(object sender, EventArgs e)
        {
            txtFolio1.Select();
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

            //Folios
            string sqlstr = "SELECT * FROM MES";
            SqlCommand cmd = new SqlCommand(sqlstr, cn);
            dbMes = new SqlDataAdapter(cmd);
            tbMes = new DataSet();
            dbMes.Fill(tbMes, "MES");
            regMes = tbMes.Tables["MES"].Rows[0];
            txtFolio1.Text = Convert.ToString(regMes["M_FAC0"]);
            regMes = tbMes.Tables["Mes"].Rows[(tbMes.Tables["Mes"].Rows.Count) - 1];
            txtFolio2.Text = Convert.ToString(regMes["M_FAC0"]);
            //Alumnos
            sqlstr = "SELECT * FROM ALUMNOS WHERE AL_CICLO = '2022-2023' ORDER BY AL_PATERNO, AL_MATERNO, AL_NOMBRES";
            SqlCommand cmd2 = new SqlCommand(sqlstr, cn);
            dbAlumnos = new SqlDataAdapter(cmd2);
            tbAlumnos = new DataSet();
            dbAlumnos.Fill(tbAlumnos, "ALUMNOS");
            regAlumnos = tbAlumnos.Tables["ALUMNOS"].Rows[0];
            cboAlumnos1.Text = Convert.ToString(regAlumnos["AL_PATERNO"]) + " " + (regAlumnos["AL_MATERNO"]) + " " + (regAlumnos["AL_NOMBRES"]);

            for (int i = 0; i < tbAlumnos.Tables["ALUMNOS"].Rows.Count; i++)
            {
                regAlumnos = tbAlumnos.Tables["ALUMNOS"].Rows[i];
                String Alumno = (
                    Convert.ToString(regAlumnos["AL_PATERNO"]) + " " +
                    Convert.ToString(regAlumnos["AL_MATERNO"]) + " " +
                    Convert.ToString(regAlumnos["AL_NOMBRES"]) + " " 
                    );
                cboAlumnos1.Items.Add(Alumno);
                cboAlumnos2.Items.Add(Alumno);

                if ($"{regAlumnos["AL_PATERNO"]}" == "MENDOZA")
                    destinatario = $"{regAlumnos["AL_CORREO"]}";

            }

            cboAlumnos1.SelectedIndex = 0;
            cboAlumnos2.SelectedIndex = 0;

            cboAlumnos1.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAlumnos2.DropDownStyle = ComboBoxStyle.DropDownList;

            /*
            //Ciclo
            sqlstr = "SELECT DISTINCT * FROM ALUMNOS";
            SqlCommand cmd3 = new SqlCommand(sqlstr, cn);
            BdMes = new SqlDataAdapter(cmd);
            TbMes = new DataSet();
            BdMes.Fill(TbMes, "MES");
            RegMes = TbMes.Tables["MES"].Rows[0];
            txtFolio1.Text = Convert.ToString(RegMes["M_FAC0"]);
            RegMes = TbMes.Tables["Mes"].Rows[(TbMes.Tables["Mes"].Rows.Count) - 1];
            txtFolio2.Text = Convert.ToString(RegMes["M_FAC0"]);
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strQuery = "SELECT ALUMNOS.AL_PATERNO, ALUMNOS.AL_MATERNO, ALUMNOS.AL_NOMBRES, MES.*, " +
                "ARTICULO.AR_DESC, ALUMNOS.AL_CVE, ALUMNOS.AL_CICLO, ALUMNOS.AL_CORREO " +
                "FROM ALUMNOS INNER JOIN MES ON ALUMNOS.AL_CVE = MES.M_CVE " +
                "INNER JOIN ARTICULO ON MES.M_ART = ARTICULO.AR_CVE " +
                "WHERE ALUMNOS.AL_PATERNO + ' ' + ALUMNOS.AL_MATERNO + ' ' + ALUMNOS.AL_NOMBRES " +
                ">= '" + cboAlumnos1.Text + "' AND " +
                "ALUMNOS.AL_PATERNO + ' ' + ALUMNOS.AL_MATERNO + ' ' + ALUMNOS.AL_NOMBRES " +
                ">= '" + cboAlumnos2.Text + "' AND " +
                "MES.M_FAC0 >= " + Convert.ToInt32(txtFolio1.Text) + " AND " +
                "MES.M_FAC0 <= " + Convert.ToInt32(txtFolio2.Text);
            SqlCommand cmd = new SqlCommand(strQuery, cn);
            dbAlumnos = new SqlDataAdapter(cmd);
            tbAlumnos = new DataSet();
            dbAlumnos.Fill(tbAlumnos, "ALUMNOS");
            MessageBox.Show($"Facturas Encontradas: {tbAlumnos.Tables["ALUMNOS"].Rows.Count}");
            imprime.Document = printDocument1;
            imprime.WindowState = FormWindowState.Maximized;
            //imprime.ShowDialog();

            //Guardar PDF
            ruta = Path.Combine(Application.StartupPath) + "/Recibos/Recibo" + txtFolio1.Text.Trim() + ".pdf";
            //ruta = @"\Facturacion\Recibos\Recibo" + txtFolio1.Text.Trim() + ".pdf";
            printDocument1.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            printDocument1.PrinterSettings.PrintFileName = ruta;
            printDocument1.PrinterSettings.PrintToFile = true;
            printDocument1.Print();

            //Form Correos
            Frm_Correos frmCorreos = new Frm_Correos();
            frmCorreos.ruta = ruta;
            frmCorreos.destinatario = destinatario;
            frmCorreos.ShowDialog();
        }
    }
}
