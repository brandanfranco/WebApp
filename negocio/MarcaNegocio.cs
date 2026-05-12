using datos;
using dominio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class MarcaNegocio
    {
        List<Marca> lista = new List<Marca>();
        AccesoDatos datos = new AccesoDatos();
        public List<Marca> listar()
        {
            try
            {
                datos.setQuery("select id, descripcion from MARCAS");
                datos.lecturaDatos();

                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)datos.Lector["id"];
                    aux.Nombre = (string)datos.Lector["descripcion"];

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
                datos.cerrarConexion();
            }
        }
        public void agregar(Marca nuevo)
        {
            string insert = "INSERT INTO MARCAS (Descripcion) VALUES (@nombre)";
            try
            {
                datos.setQuery(insert);
                datos.setarParametro("@nombre", nuevo.Nombre);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void eliminar(int id)
        {
            string consulta = "DELETE FROM MARCAS WHERE Id = @ID";
            try
            {
                datos.setQuery(consulta);
                datos.setarParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void modificar(Marca marcaModificar)
        {
            string consutla = "update MARCAS set Descripcion = @DESCRIPCION where Id = @ID";
            try
            {
                datos.setQuery(consutla);
                datos.setarParametro("@ID", marcaModificar.Id);
                datos.setarParametro("@DESCRIPCION", marcaModificar.Nombre);

                datos.ejecutarAccion();
            }
            catch (Exception EX)
            {

                throw EX;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
