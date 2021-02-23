using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Automatizacion.Selenium.Negocio;
using Automatizacion.Selenium.Generales;
using System.IO;

namespace Proceso
{
    class Program
    {
        private static string RutaOrigen = AppDomain.CurrentDomain.BaseDirectory;
        private static string EmailBuscar = System.Configuration.ConfigurationManager.AppSettings["Email"].ToString();
        private static string PasswordEmail = System.Configuration.ConfigurationManager.AppSettings["PasswordEmail"].ToString();

        static void Main(string[] args)
        {
            ConfiguracionChrome config = new ConfiguracionChrome();
            config.rutaDescrga(ConfigurationManager.AppSettings["RutaDescargaLicencias"]);
            config.maximaChorme();
            Sele sele = new Sele(config.getOptions());
            IngresoEtoro ingreso = new IngresoEtoro(sele);

            try
            {
                ingreso.DescargarInfomracionMotoresMercado().Wait();
                ingreso.DescargarInfomracionAccionesAsync().Wait();
                
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
           

            Console.WriteLine("Proceso terminado");
        }
    }
}
