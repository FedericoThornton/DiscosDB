using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Negocio
{
    public class DiscoNegocio
    {
       public List<Disco> listar()
        {
           List<Disco> lista= new List<Disco>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;


            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=DISCOS_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select D. Id, Titulo, CantidadCanciones as Canciones, UrlImagenTapa, E. Descripcion as Genero, T. Descripcion as Formato, IdEstilo, IdTipoEdicion from DISCOS D, ESTILOS E, TIPOSEDICION T where E.Id = D.IdEstilo and T.Id = D.IdTipoEdicion";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Disco aux = new Disco();
                    if (!(lector.IsDBNull(lector.GetOrdinal("Titulo"))))
                    {
                        aux.Titulo = (string)lector["Titulo"];
                    }
                    if (!(lector.IsDBNull(lector.GetOrdinal("Canciones"))))
                    {
                        aux.Canciones = (int)lector["Canciones"];
                    }
                    
                    if (!(lector.IsDBNull(lector.GetOrdinal("UrlImagenTapa"))))
                    {
                        aux.UrlImagen = (string)lector["UrlImagenTapa"];
                    }

                    aux.Id = (int)lector["Id"];        

                    aux.Genero = new Estilo();
                    aux.Genero.Id = (int)lector["IdEstilo"];
                    aux.Genero.Descripcion = (string)lector["Genero"];
                    aux.Edicion= new Formato();
                    aux.Edicion.Id = (int)lector["IdTipoEdicion"];
                    aux.Edicion.Descripcion = (string)lector["Formato"];
                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            

           
        }

        public void agregar (Disco nuevo, AccesoDatos datos)
        {
                    
          
            try
            {
                datos.SetearConsulta("insert into DISCOS (Titulo, CantidadCanciones, idEstilo, idTipoEdicion, UrlImagenTapa) values (@titulo,@canciones,@idEstilo,@idTipoEdicion, @UrlImagen)");
                datos.setearParametros("@titulo", nuevo.Titulo);
                datos.setearParametros ("@idEstilo", nuevo.Genero.Id);
                datos.setearParametros("@idTipoEdicion", nuevo.Edicion.Id);
                datos.setearParametros("@canciones", nuevo.Canciones);
                datos.setearParametros("UrlImagen", nuevo.UrlImagen);
                datos.EjecutarAccion();
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

        public void modificar(Disco nuevo)
        {

            AccesoDatos datos = new AccesoDatos();
            datos.SetearConsulta("Update DISCOS set Titulo = @Titulo, CantidadCanciones = @Canciones, UrlImagenTapa = @UrlImagenTapa, IdEstilo = @IdEstilo , IdTipoEdicion= @IdTipoEdicion where Id = @Id");
            datos.setearParametros("@Titulo",nuevo.Titulo);
            datos.setearParametros("@Canciones", nuevo.Canciones);
            datos.setearParametros("@UrlImagenTapa", nuevo.UrlImagen);
            datos.setearParametros("@IdEstilo", nuevo.Genero);
            datos.setearParametros("@IdTipoEdicion", nuevo.Edicion);
            datos.setearParametros("@Id", nuevo.Id);
            try
            {

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

        public void Eliminar( int id)
        {
           
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta("delete from DISCOS where id = @Id");
                datos.setearParametros("@Id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Disco> filtrar (string campo, string criterio, string filtro)
        {
            List<Disco> lista = new List<Disco>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
              
                string consulta = "select D. Id, Titulo, CantidadCanciones as Canciones, UrlImagenTapa, E. Descripcion as Genero, T. Descripcion as Formato, IdEstilo, IdTipoEdicion from DISCOS D, ESTILOS E, TIPOSEDICION T where E.Id = D.IdEstilo and T.Id = D.IdTipoEdicion and ";
             

                    switch (campo)
                {

                    case "Título":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "Titulo like '" + filtro + "%'";
                            break;
                            case "termina con":
                                consulta += "Titulo like '%" + filtro + "'";
                                break;

                            default:
                                consulta += "Titulo like '%" + filtro + "%'";

                                break;
                        }
                        break;
                    case "Canciones":
                        switch (criterio)
                        {
                            case "Mayor a":
                                consulta += "CantidadCanciones >" + filtro;
                                break;
                            case "Menor a":
                                consulta += "CantidadCanciones <" + filtro;
                                break;

                            default:
                                consulta += "CantidadCanciones =" + filtro;
                                break;
                        }

                        break;
                    case "Género":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "E. Descripcion like '" + filtro + "%'";
                                break;
                            case "termina con":
                                consulta += "E. Descripcion like '%" + filtro + "'";
                                break;

                            default:
                                consulta += "E. Descripcion like '%" + filtro + "%'";

                                break;
                        }

                        break;
                    case "Edición":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "T. Descripcion like '" + filtro + "%'";
                                break;
                            case "termina con":
                                consulta += "T. Descripcion like '%" + filtro + "'";
                                break;

                            default:
                                consulta += "T. Descripcion like '%" + filtro + "%'";

                                break;
                        }
                        break;
                  
                }

            

                datos.SetearConsulta( consulta );
                datos.EjecutarLectura();


                while (datos.lector.Read())
                {
                    Disco aux = new Disco();
                    if (!(datos.lector.IsDBNull(datos.lector.GetOrdinal("Titulo"))))
                    {
                        aux.Titulo = (string)datos.lector["Titulo"];
                    }
                    if (!(datos.lector.IsDBNull(datos.lector.GetOrdinal("Canciones"))))
                    {
                        aux.Canciones = (int)datos.lector["Canciones"];
                    }

                    if (!(datos.lector.IsDBNull(datos.lector.GetOrdinal("UrlImagenTapa"))))
                    {
                        aux.UrlImagen = (string)datos.lector["UrlImagenTapa"];
                    }

                    aux.Id = (int)datos.lector["Id"];

                    aux.Genero = new Estilo();
                    aux.Genero.Id = (int)datos.lector["IdEstilo"];
                    aux.Genero.Descripcion = (string)datos.lector["Genero"];
                    aux.Edicion = new Formato();
                    aux.Edicion.Id = (int)datos.lector["IdTipoEdicion"];
                    aux.Edicion.Descripcion = (string)datos.lector["Formato"];
                    lista.Add(aux);
                }

                
              
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
