using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;

namespace WebApplication
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            gvArticulos.DataSource = negocio.listar();
            gvArticulos.DataBind();
        }
    }
}