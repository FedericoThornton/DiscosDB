using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace Proyecto_Discos
{
    public partial class frmAltaDisco : Form
    {
        private Disco disco = null;
        private OpenFileDialog archivo = null;
        public frmAltaDisco()
        {
            InitializeComponent();
        }
        public frmAltaDisco( Disco disco)
        {
            InitializeComponent();
            this.disco = disco;
            Text = "Modificar Disco";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
         
            DiscoNegocio negocio = new DiscoNegocio();
            AccesoDatos datos = new AccesoDatos();
            try
             {
                if (disco != null)
                {
                    disco = new Disco();
                }

                disco.Titulo =txtTitulo.Text;
                disco.Canciones = int.Parse(txtCanciones.Text);
                disco.UrlImagen = txtImagen.Text;
                disco.Genero = (Estilo)cbxGenero.SelectedItem;
                disco.Edicion = (Formato)cbxEdicion.SelectedItem;

                if (disco.Id != 0)
                {
                    negocio.agregar(disco, datos);
                    MessageBox.Show("Agregado exitosamente");
                }
                else
                {
                    negocio.modificar(disco);
                    MessageBox.Show("Modificado exitosamente");
                }

                // guardo imagen si la levanto localmente

                if (archivo != null && !(txtImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images - folder"] + archivo.SafeFileName);
                }
              
                Close(); ;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaDisco_Load(object sender, EventArgs e)
        {
            EstiloNegocio estiloNegocio = new EstiloNegocio();
            FormatoNegocio formatoNegocio= new FormatoNegocio();
            try
            {
                cbxGenero.DataSource = estiloNegocio.listar();
                cbxGenero.ValueMember = "Id";
                cbxGenero.DisplayMember = "Descripcion"; 
                cbxEdicion.DataSource = formatoNegocio.listar();
                cbxEdicion.ValueMember = "Id";
                cbxEdicion.DisplayMember= "Descripcion"; 


                if(disco !=null)
                {
                    txtTitulo.Text= disco.Titulo;
                    txtCanciones.Text = disco.Canciones.ToString();
                    cargarImagen(disco.UrlImagen);
                    txtImagen.Text = disco.UrlImagen;
                    cbxGenero.SelectedValue = disco.Genero.Id;
                    cbxEdicion.SelectedValue = disco.Edicion.Id;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagen.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception)
            {
                pbxImagen.Load("https://sajarutyoga.com/wp-content/uploads/2020/06/Playlist-icon-768x432.png");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png" ;
            if (archivo.ShowDialog() == DialogResult.OK) 
            {
                txtImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                // guardo la imagen
               // File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images - folder"] + archivo.SafeFileName);
            }

        }
    }
}
