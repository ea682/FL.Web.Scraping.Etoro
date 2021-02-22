using FL.Automatizacion.Store.Generales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL.Automatizacion.Store.Generales
{
    public class EmpresaDal
    {
        public static int Agregar(Empresas empresa)
        {
            int retorno = 0;
            double Venta = Convert.ToDouble(empresa.PrecioVenta);
            double Compra = Convert.ToDouble(empresa.PrecioCompra);
            try
            {
                using (SqlConnection Conn = Conexion.ObtenerConexion())
                {
                    string queryAgregarEmpresa = string.Format("Insert into Empresas (SiglaEmpresa, NombreEmpresa, PrecioVenta, PrecioCompra, Sector, Industria, FechaCreacion) values ('{0}','{1}',{2},{3},'{4}','{5}','{6}',)", empresa.SiglaEmpresa, empresa.NombreEmpresa, Venta, Compra, empresa.Sector, empresa.Industria, DateTime.Now);

                    SqlCommand Comando = new SqlCommand(queryAgregarEmpresa);
                    retorno = Comando.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return retorno;
        }

        public static bool IngresarUsuario(Empresas empresa)
        {
            double Venta = Convert.ToDouble(empresa.PrecioVenta);
            double Compra = Convert.ToDouble(empresa.PrecioCompra);
            double Rendimiento = Convert.ToDouble(empresa.Rendimiento);

            string ruta = "Data Source=DESKTOP-4V4E9VS;Initial Catalog=FL.Web.Scrapping.Etoro;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(ruta))
            {
                string query = "Insert into Empresas (SiglaEmpresa, NombreEmpresa, PrecioVenta, PrecioCompra, Sector, Industria, FechaCreacion, Rendimiento, NombreCompletoMercado, TipoMercado, UrlEmpresa) values (@SiglaEmpresa, @NombreEmpresa, @PrecioVenta, @PrecioCompra, @Sector, @Industria, @FechaCreacion, @Rendimiento, @NombreCompletoMercado, @TipoMercado, @UrlEmpresa);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@SiglaEmpresa", empresa.SiglaEmpresa);
                        command.Parameters.AddWithValue("@NombreEmpresa", empresa.NombreEmpresa);
                        command.Parameters.AddWithValue("@PrecioVenta", Venta);
                        command.Parameters.AddWithValue("@PrecioCompra", Compra);
                        command.Parameters.AddWithValue("@Sector", empresa.Sector);
                        command.Parameters.AddWithValue("@Industria", empresa.Industria);
                        command.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                        command.Parameters.AddWithValue("@Rendimiento", Rendimiento);
                        command.Parameters.AddWithValue("@NombreCompletoMercado", empresa.NombreCompletoMercado);
                        command.Parameters.AddWithValue("@TipoMercado", empresa.TipoMercado);
                        command.Parameters.AddWithValue("@UrlEmpresa", empresa.UrlEmpresa);

                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                        // Log de exepcciones
                    }
                }
            }

            return false;
        }
    }
}
