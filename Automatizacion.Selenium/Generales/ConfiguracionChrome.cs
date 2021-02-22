using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatizacion.Selenium.Generales
{
    public class ConfiguracionChrome
    {
        private ChromeOptions options = new ChromeOptions();

        public ChromeOptions getOptions()
        {
            return options;
        }

        public void maximaChorme()
        {
            options.AddArgument("start-maximized");
        }

        public void rutaDescrga(string ruta)
        {
            options.AddUserProfilePreference("download.default_directory", ruta);
        }

        public void EliminarVisualizadorPDF()
        {
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);

        }

        public void EliminarInterfaz()
        {
            options.AddArguments("headless");
            options.AddArguments("disable-gpu");
            options.AddArguments("no-sandbox");

        }
    }
}
