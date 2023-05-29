using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Policy;

namespace Negocio
{
    public class AccesoDatos 
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader Lector;
        public SqlDataReader lector
        { 
            get { return Lector; }
        }


        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=DISCOS_DB; integrated security=true");
            comando = new SqlCommand();
        }

        public void SetearConsulta( string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText= consulta;
        }

        public void EjecutarLectura ()
        {
            try
            {
                comando.Connection = conexion;
                conexion.Open();
                Lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }
         
        
        }

        public void setearParametros(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);

        }

            public void CerrarConexion()
        {
            if (conexion != null)
            {
                conexion.Close();
            }

            if (Lector != null)
            {
                Lector.Close();
            }
             conexion.Close ();

        }
        public void EjecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
    