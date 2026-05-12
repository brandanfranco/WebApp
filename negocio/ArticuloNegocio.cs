using datos;
using dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace negocio
{
    public class ArticuloNegocio
    {

        List<Articulo> lista = new List<Articulo>();
        AccesoDatos data = new AccesoDatos();

        public List<Articulo> listar()
        {
            //string select = "SELECT A.Id, Codigo, Nombre, A.Descripcion, M.Id marcID, M.Descripcion marcDesc, C.Id catID, C.Descripcion catDesc, Precio, I.Id imgID, I.ImagenUrl imgUrl";
            //string from = " FROM ARTICULOS A INNER JOIN MARCAS M ON A.IdMarca = M.Id INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id LEFT JOIN IMAGENES AS I ON I.IdArticulo = A.Id";

            //string select = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Id AS marcID, M.Descripcion AS marcDesc, C.Id AS catID, C.Descripcion AS catDesc, A.Precio, I.ImagenUrl";
            //string from = " FROM ARTICULOS A INNER JOIN MARCAS M ON A.IdMarca = M.Id INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id LEFT JOIN IMAGENES I ON I.Id = (SELECT TOP 1 Id FROM IMAGENES WHERE IdArticulo = A.Id ORDER BY Id)";


            //string consulta = select + from;

            try
            {
                //data.setQuery(consulta);

                data.setProcedure("storedListar");
                data.lecturaDatos();

                while (data.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)data.Lector["Id"];
                    aux.Codigo = (string)data.Lector["Codigo"];
                    aux.Nombre = (string)data.Lector["Nombre"];
                    aux.Descripcion = (string)data.Lector["Descripcion"];
                    aux.Precio = (decimal)data.Lector["Precio"];

                    aux.Marca = new Marca
                    {
                        Id = (int)data.Lector["marcID"],
                        Nombre = (string)data.Lector["marcDesc"]
                    };

                    aux.Categoria = new Categoria
                    {
                        Id = (int)data.Lector["catID"],
                        Nombre = (string)data.Lector["catDesc"]
                    };

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
                data.cerrarConexion();
            }
        }

        public void agregarArticulo(Articulo nuevo)
        {

            string insert = "INSERT INTO Articulos (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria ) " +
                            "VALUES (@Codigo, @Nombre, @Descripcion, @Precio, @IdMarca, @IdCategoria)";


            try
            {
                data.setQuery(insert);

                data.setarParametro("@Codigo", nuevo.Codigo);
                data.setarParametro("@Nombre", nuevo.Nombre);
                data.setarParametro("@Descripcion", nuevo.Descripcion);
                data.setarParametro("@Precio", nuevo.Precio);

                data.setarParametro("@IdMarca", nuevo.Marca.Id);
                data.setarParametro("@IdCategoria", nuevo.Categoria.Id);

                data.ejecutarAccion();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }

        }



        public void modificar(Articulo art)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                data.setQuery("update Articulos set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio, IdMarca=@IdMarca, IdCategoria=@IdCategoria Where Id = @id");
                data.setarParametro("@Codigo", art.Codigo);
                data.setarParametro("@Nombre", art.Nombre);
                data.setarParametro("@Descripcion", art.Descripcion);
                data.setarParametro("@Precio", art.Precio);

                data.setarParametro("@IdMarca", art.Marca.Id);
                data.setarParametro("@IdCategoria", art.Categoria.Id);

                data.setarParametro("@id", art.Id);

                data.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        }

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setQuery("delete from Articulos where id = @id");
                datos.setarParametro("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ultimoId()
        {
            try
            {
                data.setQuery("SELECT MAX(Id) FROM Articulos");
                data.lecturaDatos();

                if (data.Lector.Read())
                {
                    return Convert.ToInt32(data.Lector[0]);
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo> ();
            AccesoDatos data = new AccesoDatos();
            try
            {
                string select = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Id AS marcID, M.Descripcion AS marcDesc, C.Id AS catID, C.Descripcion AS catDesc, A.Precio, I.ImagenUrl";
                string from = " FROM ARTICULOS A INNER JOIN MARCAS M ON A.IdMarca = M.Id INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id LEFT JOIN IMAGENES I ON I.Id = (SELECT TOP 1 Id FROM IMAGENES WHERE IdArticulo = A.Id ORDER BY Id) where ";

                string consulta = select + from;

                if (campo == "Código")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Codigo LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "A.Codigo LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Codigo LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Nombre LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "A.Nombre LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Nombre LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Descripción")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Descripcion LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "A.Descripcion LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Descripcion LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "A.Precio > '" + filtro + "'";
                            break;
                        case "Menor a":
                            consulta += "A.Precio < '" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Precio = '" + filtro + "'";
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "M.Descripcion LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Categoría")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "C.Descripcion LIKE '%" + filtro + "%'";
                            break;
                    }
                }

                data.setQuery(consulta);
                data.lecturaDatos();

                while (data.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)data.Lector["Id"];
                    aux.Codigo = (string)data.Lector["Codigo"];
                    aux.Nombre = (string)data.Lector["Nombre"];
                    aux.Descripcion = (string)data.Lector["Descripcion"];
                    aux.Precio = (decimal)data.Lector["Precio"];

                    aux.Marca = new Marca
                    {
                        Id = (int)data.Lector["marcID"],
                        Nombre = (string)data.Lector["marcDesc"]
                    };

                    aux.Categoria = new Categoria
                    {
                        Id = (int)data.Lector["catID"],
                        Nombre = (string)data.Lector["catDesc"]
                    };

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





