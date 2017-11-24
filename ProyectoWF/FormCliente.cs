using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto {
    public partial class FormClientes : Form {
        private int modo;
        private int pk;

        public FormClientes(int modo, int pk)
        {
            InitializeComponent();
            this.modo = modo;
            this.pk = pk;

            string cargaDatos;

            if (modo == 0)
            {
                this.Text = "Alta de Cliente.";
            }
            else if (modo == 1)
            {
                this.Text = "Modificación de Cliente.";
                cargaDatos = "Select * from dbo.Clientes where ClienteID=" + pk;
                SqlDataAdapter ejecutaConsulta = new SqlDataAdapter(cargaDatos, Conexion.getConexion());
                DataSet resultado = new DataSet();
                ejecutaConsulta.Fill(resultado);
                tbNombre.Text = resultado.Tables[0].Rows[0]["ContactoNombre"].ToString();
                mtbFax.Text = resultado.Tables[0].Rows[0]["Fax"].ToString();
                mtbTelefono.Text = resultado.Tables[0].Rows[0]["Telefono"].ToString();
                tbDirec.Text = resultado.Tables[0].Rows[0]["Direccion"].ToString();
                tbCiudad.Text = resultado.Tables[0].Rows[0]["Ciudad"].ToString();
                tbReg.Text = resultado.Tables[0].Rows[0]["Region"].ToString();
                tbCP.Text = resultado.Tables[0].Rows[0]["CodigoPostal"].ToString();
                tbPais.Text = resultado.Tables[0].Rows[0]["Pais"].ToString();
                tbNombreComp.Text = resultado.Tables[0].Rows[0]["NombreCompania"].ToString();
                //EL LOGO ES UN PROBLEMA
                tbLogo.Text = resultado.Tables[0].Rows[0]["Logo"].ToString();
            }
            else if (modo == 2)
            {
                this.Text = "Vista de datos de Cliente.";
                cargaDatos = "Select * from dbo.Clientes where ClienteID=" + pk;
                SqlDataAdapter ejecutaConsulta = new SqlDataAdapter(cargaDatos, Conexion.getConexion());
                DataSet resultado = new DataSet();
                ejecutaConsulta.Fill(resultado);
                tbNombre.Text = resultado.Tables[0].Rows[0]["ContactoNombre"].ToString();
                tbNombre.Enabled = false;
                mtbFax.Text = resultado.Tables[0].Rows[0]["Fax"].ToString();
                mtbFax.Enabled = false;
                mtbTelefono.Text = resultado.Tables[0].Rows[0]["Telefono"].ToString();
                mtbTelefono.Enabled = false;
                tbDirec.Text = resultado.Tables[0].Rows[0]["Direccion"].ToString();
                tbDirec.Enabled = false;
                tbCiudad.Text = resultado.Tables[0].Rows[0]["Ciudad"].ToString();
                tbCiudad.Enabled = false;
                tbReg.Text = resultado.Tables[0].Rows[0]["Region"].ToString();
                tbReg.Enabled = false;
                tbCP.Text = resultado.Tables[0].Rows[0]["CodigoPostal"].ToString();
                tbCP.Enabled = false;
                tbPais.Text = resultado.Tables[0].Rows[0]["Pais"].ToString();
                tbPais.Enabled = false;
                tbNombreComp.Text = resultado.Tables[0].Rows[0]["NombreCompania"].ToString();
                tbNombreComp.Enabled = false;
                //EL LOGO ES UN PROBLEMA
                tbLogo.Text = resultado.Tables[0].Rows[0]["Logo"].ToString();
                tbLogo.Enabled = false;
            }
        }

        public FormClientes()
        {
            InitializeComponent();
        }

        private void btLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofg = new OpenFileDialog();

            ofg.InitialDirectory = ".";
            ofg.Filter = "Imágenes de mapa de bits (*)|*.bmp;*.gif;*.jpg;";
            ofg.RestoreDirectory = true;

            if (ofg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    tbLogo.Text = ofg.FileName;
                    pbLogo.ImageLocation = tbLogo.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se ha podido leer el archivo.\n" + ex.Message);
                }
            }
        }

        private void btAceptar_Click(object sender, EventArgs e)
        {
            switch (modo)
            {
                case 0:
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Clientes " +
                            "(NombreCompania,ContactoNombre,Fax,Telefono,Direccion,Ciudad,Region,CodigoPostal,Pais,Logo) " +
                            "VALUES(@nomCom,@contactoNombre,@fax,@telefono,@dir,@ciudad,@reg,@cp,@pais,@logo)", Conexion.getConexion());
                        cmd.Parameters.Add("@contactoNombre", SqlDbType.NVarChar).Value = this.tbNombre.Text;
                        cmd.Parameters.Add("@fax", SqlDbType.NVarChar).Value = this.mtbFax.Text;
                        cmd.Parameters.Add("@telefono", SqlDbType.NVarChar).Value = this.mtbTelefono.Text;
                        cmd.Parameters.Add("@dir", SqlDbType.NVarChar).Value = this.tbDirec.Text;
                        cmd.Parameters.Add("@ciudad", SqlDbType.NVarChar).Value = this.tbCiudad.Text;
                        cmd.Parameters.Add("@reg", SqlDbType.NVarChar).Value = this.tbReg.Text;
                        cmd.Parameters.Add("@cp", SqlDbType.NVarChar).Value = this.tbCP.Text;
                        cmd.Parameters.Add("@pais", SqlDbType.NVarChar).Value = this.tbPais.Text;
                        cmd.Parameters.Add("@nomCom", SqlDbType.NVarChar).Value = this.tbNombreComp.Text;

                        byte[] imagen;

                        if (tbLogo.Text == "")
                        {
                            imagen = new byte[1];
                        }
                        else
                        {
                            imagen = File.ReadAllBytes(@tbLogo.Text);
                        }

                        cmd.Parameters.Add("@logo", SqlDbType.Image).Value = imagen;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Alta realizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                        break;
                    }
                case 1:
                    {
                        string modificar = "UPDATE Clientes SET (ContactoNombre = '@contactoNombre'," +
                        "Fax = '@fax'," +
                        "Telefono = '@telefono', " +
                        "Direccion = '@dir'," +
                        "Ciudad = '@ciudad'," +
                        "Region = '@reg'," +
                        "CodigoPostal = '@cp'," +
                        "Pais = '@pais'," +
                        "NombreCompania = '@nomCom'," +
                        "Logo = '@logo') WHERE ClienteID = @id";
                        SqlCommand cmd = new SqlCommand(modificar, Conexion.getConexion());
                        cmd.Parameters.Add("@contactoNombre", SqlDbType.NVarChar, 30).Value = this.tbNombre.Text;
                        cmd.Parameters.Add("@fax", SqlDbType.NVarChar, 15).Value = this.mtbFax.Text;
                        cmd.Parameters.Add("@telefono", SqlDbType.NVarChar, 24).Value = this.mtbTelefono.Text;
                        cmd.Parameters.Add("@dir", SqlDbType.NVarChar, 60).Value = this.tbDirec.Text;
                        cmd.Parameters.Add("@ciudad", SqlDbType.NVarChar, 15).Value = this.tbCiudad.Text;
                        cmd.Parameters.Add("@reg", SqlDbType.NVarChar, 15).Value = this.tbReg.Text;
                        cmd.Parameters.Add("@cp", SqlDbType.NVarChar, 10).Value = this.tbCP.Text;
                        cmd.Parameters.Add("@pais", SqlDbType.NVarChar, 15).Value = this.tbPais.Text;
                        cmd.Parameters.Add("@nomCom", SqlDbType.NVarChar, 40).Value = this.tbNombreComp.Text;
                        //cmd.Parameters.AddWithValue("@logo", tbLogo.Text);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = pk;
                        cmd.ExecuteNonQuery();
                        //hola
                        break;
                    }
            }
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            Conexion.cerrarConexion();
        }

    }
}
