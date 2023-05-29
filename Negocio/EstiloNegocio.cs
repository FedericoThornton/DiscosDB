using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;


namespace Negocio
{
    public class EstiloNegocio
    {
        public List<Estilo> listar()
        {
			List<Estilo>lista = new List<Estilo>();
			AccesoDatos datos = new AccesoDatos();
			try
			{
				datos.SetearConsulta("select  Id, Descripcion from ESTILOS");
				datos.EjecutarLectura();
				while (datos.lector.Read())
				{
					Estilo aux = new Estilo();
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
