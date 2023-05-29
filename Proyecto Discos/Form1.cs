using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Proyecto_Discos
{
    public partial class Discos : Form
    {
        private List<Disco> listaDisco;
        public Discos()
        {
            InitializeComponent();
        }

        private void Discos_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Título");
            cboCampo.Items.Add("Canciones");
            cboCampo.Items.Add("Género");
            cboCampo.Items.Add("Edición");
        }

        private void cargar()

        {
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {

                listaDisco = negocio.listar();
                DgbDiscos.DataSource = listaDisco;
                ocultarColumnas();
                cargarImagen(listaDisco[0].UrlImagen);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            DgbDiscos.Columns["UrlImagen"].Visible = false;
            DgbDiscos.Columns["Id"].Visible = false;
        }

        private void DgbDiscos_SelectionChanged(object sender, EventArgs e)
        {
            if (DgbDiscos.CurrentRow != null)
            {
                Disco seleccionado = (Disco)DgbDiscos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }
         
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxDiscos.Load(imagen);
            }
            catch (Exception)
            {
                pbxDiscos.Load("https://sajarutyoga.com/wp-content/uploads/2020/06/Playlist-icon-768x432.png");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaDisco alta = new frmAltaDisco();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Disco seleccionado;
            seleccionado = (Disco)DgbDiscos.CurrentRow.DataBoundItem;
            frmAltaDisco modificar = new frmAltaDisco(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            DiscoNegocio negocio = new DiscoNegocio();
            Disco seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad queres eliminar?", "Eliminando", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if ( respuesta == DialogResult.Yes)
                {
                    seleccionado = (Disco)DgbDiscos.CurrentRow.DataBoundItem;
                    negocio.Eliminar(seleccionado.Id);
                    cargar();
                }
             
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validadFiltro()
        {
            if (cboCampo.SelectedIndex== -1) 
            {
                MessageBox.Show("Por favor seleccione un campo");
                return true;
            }
            if (cboCriterio.SelectedIndex== -1)
            {
                MessageBox.Show("Por favor seleccione un criterio");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Canciones")

            {
                if(string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Por favor completar el filtro");
                    return true;
                }
                if(!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Por favor colocar número");
                        return true;
                }
                
            }


            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }


            return true;

        }
        private void BtnFiltro_Click(object sender, EventArgs e)
        {
            DiscoNegocio negocio = new DiscoNegocio();

            try
            {
                if (validadFiltro())
                {
                    return;
                }
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                DgbDiscos.DataSource = negocio.filtrar(campo, criterio, filtro);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
          

        }

        private void TxtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            

        }

        private void TxtFiltro_TextChanged(object sender, EventArgs e)  //para buscar sin el boton buscar
        {
            List<Disco> listaFiltrada;
            string filtro = TxtFiltro.Text.Trim(); // trim para eliminar espacios en blanco

            if (filtro.Length >= 2 )
            {
                //lambda expression
                listaFiltrada = listaDisco.FindAll(x => x.Titulo.ToUpper().Contains(filtro.ToUpper()) || x.Genero.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaDisco;
            }


            DgbDiscos.DataSource = null;
            DgbDiscos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "Canciones")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");


            } else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }

        }
    }
           
    } 
