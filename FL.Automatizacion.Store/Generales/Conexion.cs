using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL.Automatizacion.Store.Generales
{
    public class Conexion
    {
        public static SqlConnection ObtenerConexion()
        {
            SqlConnection Conn = new SqlConnection("Data Source=DESKTOP-4V4E9VS;Initial Catalog=FL.Web.Scrapping.Etoro;Integrated Security=True");
            Conn.Open();
            return Conn;
        }
    }
}
