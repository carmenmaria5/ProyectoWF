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
                //Carga los datos de la BBDD
                cargaDatos = "Select * from dbo.Clientes where ClienteID='" +pk+"'";
                //Ejecuta la consulta
                SqlDataAdapter ejecutaConsulta = new SqlDataAdapter(cargaDatos, Conexion.getConexion());
                DataSet resultado = new DataSet();
                //Guarda el resultado de la consulta en DataSet resultado
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


                //Sacar la imagen de la BBDD
                Byte[] image = (Byte[])(resultado.Tables[0].Rows[0]["Logo"]);
                //Si existe la imagen
                if (image.Length > 1)
                {
                    pbLogo.Image = Image.FromStream(new MemoryStream(image));
                }


            }
            else if (modo == 2)
            {
                this.Text = "Vista de datos de Cliente.";
                //Carga los datos de la BBDD
                cargaDatos = "Select * from dbo.Clientes where ClienteID=" + pk;
                //Ejecuta la consulta
                SqlDataAdapter ejecutaConsulta = new SqlDataAdapter(cargaDatos, Conexion.getConexion());
                DataSet resultado = new DataSet();
                //Guarda el resultado de la consulta en DataSet resultado
                ejecutaConsulta.Fill(resultado);
                tbNombre.Text = resultado.Tables[0].Rows[0]["ContactoNombre"].ToString();
                //Pone el campo para que no se pueda modificar
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

                //Sacar la imagen de la BBDD
                Byte[] image = (Byte[])(resultado.Tables[0].Rows[0]["Logo"]);
                //Si existe la imagen
                if (image.Length > 1)
                {
                    pbLogo.Image = Image.FromStream(new MemoryStream(image));
                }

                //Deshabilito el botón aceptar
                btAceptar.Enabled = false;
            }
        }

        /// <summary>
        /// Método click para el botón de la imagen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLogo_Click(object sender, EventArgs e)
        {
            //Abre la ventana del explorador para seleccionar la imagen
            OpenFileDialog ofg = new OpenFileDialog();

            ofg.InitialDirectory = ".";
            //ofg.Filter = "Imagenes de mapa de bits (*)|*.bmp;*.gif;*.jpg;";
            //Abre siempre el directorio puesto por defecto
            ofg.RestoreDirectory = true;

            if (ofg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Añade la ruta de la imagen al cuadro de texto
                    tbLogo.Text = ofg.FileName;
                    //Añade la imagen al cuadro seleccionado
                    pbLogo.ImageLocation = tbLogo.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se ha podido leer el archivo.\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Botón aceptar.
        /// Comprueba si existe el campo obligatorio. En caso de ser nulo, muestra un messageBox.
        /// En otro caso, realiza las consultas de inserción y modificación, dependiendo del caso especificado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAceptar_Click(object sender, EventArgs e)
        {
            //Compruebo si el campo obligatorio no está relleno
            if (tbNombreComp.Text.Equals(""))
            {
                MessageBox.Show("Nombre compañía obligatorio.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //Si dicho campo está relleno
            else
            {
                switch (modo)
                {
                    case 0:
                        {
                            //Consulta para hacer el insert
                            SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Clientes " +
                                "(NombreCompania,ContactoNombre,Fax,Telefono,Direccion,Ciudad,Region,CodigoPostal,Pais,Logo) " +
                                "VALUES(@nomCom,@contactoNombre,@fax,@telefono,@dir,@ciudad,@reg,@cp,@pais,@logo)", Conexion.getConexion());
                            //Añado a cada testBox el valor dado
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

                            //Si el textBox del logo está vacío, no añade imagen
                            if (tbLogo.Text == "")
                            {
                                imagen = new byte[1];
                            }
                            //en caso contrario, la añade
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
                            //Consulta para modificar los datos
                            string modificar = "UPDATE Clientes SET ContactoNombre = @contactoNombre," +
                            "Fax = @fax," +
                            "Telefono = @telefono, " +
                            "Direccion = @dir," +
                            "Ciudad = @ciudad," +
                            "Region = @reg," +
                            "CodigoPostal = @cp," +
                            "Pais = @pais," +
                            "NombreCompania = @nomCom," +
                            "Logo = @logo WHERE ClienteID = @id";
                            SqlCommand cmd = new SqlCommand(modificar, Conexion.getConexion());
                            //Vuelve a añadir todos los datos una vez modificados
                            cmd.Parameters.Add("@contactoNombre", SqlDbType.NVarChar, 30).Value = this.tbNombre.Text;
                            cmd.Parameters.Add("@fax", SqlDbType.NVarChar, 15).Value = this.mtbFax.Text;
                            cmd.Parameters.Add("@telefono", SqlDbType.NVarChar, 24).Value = this.mtbTelefono.Text;
                            cmd.Parameters.Add("@dir", SqlDbType.NVarChar, 60).Value = this.tbDirec.Text;
                            cmd.Parameters.Add("@ciudad", SqlDbType.NVarChar, 15).Value = this.tbCiudad.Text;
                            cmd.Parameters.Add("@reg", SqlDbType.NVarChar, 15).Value = this.tbReg.Text;
                            cmd.Parameters.Add("@cp", SqlDbType.NVarChar, 10).Value = this.tbCP.Text;
                            cmd.Parameters.Add("@pais", SqlDbType.NVarChar, 15).Value = this.tbPais.Text;
                            cmd.Parameters.Add("@nomCom", SqlDbType.NVarChar, 40).Value = this.tbNombreComp.Text;

                            byte[] imagen;

                            //Añade imagen si no la hay
                            if (tbLogo.Text != "")
                            {
                                imagen = File.ReadAllBytes(@tbLogo.Text);
                                cmd.Parameters.Add("@logo", SqlDbType.Image).Value = imagen;
                            }
                            
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = pk;
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Modificación realizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Botón cancelar. 
        /// Cierra el formulario y la conexión a la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            Conexion.cerrarConexion();
        }
    }
}
