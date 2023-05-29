using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class FormatoNegocio
    {
        public List<Formato> listar()
        {
            List<Formato> lista = new List<Formato>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("select  Id, Descripcion from TIPOSEDICION");
                datos.EjecutarLectura();
                while (datos.lector.Read())
                {
                    Formato aux = new Formato();
                    aux.Id = (int)datos.lector["Id"];
                    aux.Descripcion = (string)datos.lector["Descripcion"];
                    lista.Add(aux);
                }


                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }

        }
    }
}
