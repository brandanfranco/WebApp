using dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datos;


namespace negocio
{
    public class ImagenNegocio
    {
        List<Imagen> ListaImagenes = new List<Imagen>();
        AccesoDatos datos = new AccesoDatos();
        public List<Imagen> listar()
        {
            try
            {
                datos.setQuery("select * from Imagenes");
                datos.lecturaDatos();

                while (datos.Lector.Read())
                {
                    Imagen aux = new Imagen();
                    aux.Id = (int)datos.Lector["id"];
                    aux.IdArticulo = (int)datos.Lector["IdArticulo"];
                    aux.Url = (string)datos.Lector["Imagenurl"];
                   

                    ListaImagenes.Add(aux);
                }

                return ListaImagenes;
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


        public void agregarImagen(Imagen nueva)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setQuery("INSERT INTO Imagenes (IdArticulo, ImagenUrl) VALUES (@IdArticulo, @Url)");

                datos.setarParametro("@IdArticulo", nueva.IdArticulo);
                datos.setarParametro("@Url", nueva.Url);

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
    }




}
