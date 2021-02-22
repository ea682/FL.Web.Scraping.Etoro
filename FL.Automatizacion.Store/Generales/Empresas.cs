using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL.Automatizacion.Store.Generales
{
    public class Empresas
    {
        public string SiglaEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public double Rendimiento { get; set; }
        public double PrecioVenta { get; set; }
        public double PrecioCompra { get; set; }
        public string Sector { get; set; }
        public string Industria { get; set; }
        public string NombreCompletoMercado { get; set; }
        public string TipoMercado { get; set; }
        public string UrlEmpresa { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
