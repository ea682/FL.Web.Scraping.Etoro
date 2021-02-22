using Automatizacion.Selenium.Generales;
using FL.Automatizacion.Store.Generales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatizacion.Selenium.Negocio
{
    public class IngresoEtoro
    {
        private Sele sele;
        public IngresoEtoro(Sele selenium)
        {
            sele = selenium;
        }
        
        private Boolean IngresoPlataforma()
        {
            sele.OpenChorme();
            sele.CambiarUrl("https://www.etoro.com/es/discover/markets");

            if (sele.BuscarElemento("xpath", "//*[@id='discover - cards - wrapp - discovermarketshome - moversandshakers']/h2/a", 30))
            {
                return true;
            }
            return false;
        }

        public async Task<Boolean> DescargarInfomracionMotoresMercado()
        {
            string JsonData = "";
            //Valadimas ingreso de la pagina de etoro o la carga de la misma.
            if (IngresoPlataforma())
            {
                sele.ClickSelenium("xpath", "//*[@id='discover - cards - wrapp - discovermarketshome - moversandshakers']/a");

                if (sele.BuscarElemento("xpath", "/html/body/ui-layout/div/div/div[2]/et-discovery-markets-results/div/div/et-discovery-markets-results-grid/div/et-instrument-card[1]", 30))
                {
                    //Obtenemos la cantidad de elementos
                    JsonData = sele.EjecutarJS(ObtenerInformacionJson());
                    List<Empresas> ListaEmpresa = new List<Empresas>();
                    try
                    {
                        ListaEmpresa =JsonConvert.DeserializeObject<List<Empresas>>(JsonData);
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    ListaEmpresa = ObtenerInformacionEstadistica(ListaEmpresa);

                    foreach (Empresas Empresa in ListaEmpresa)
                    {
                        EmpresaDal.IngresarUsuario(Empresa);
                        //EmpresaDal.Agregar(Empresa);
                    }
                }
            }
            return false;
        }

        public Boolean DescargarInfomracionMateriasPrimas()
        {
            //Valadimas ingreso de la pagina de etoro o la carga de la misma.
            if (IngresoPlataforma())
            {

            }
            return false;
        }

        public Boolean DescargarInfomracionCriptos()
        {
            //Valadimas ingreso de la pagina de etoro o la carga de la misma.
            if (IngresoPlataforma())
            {

            }
            return false;
        }

        public Boolean DescargarInfomracionTecnologia()
        {
            //Valadimas ingreso de la pagina de etoro o la carga de la misma.
            if (IngresoPlataforma())
            {

            }
            return false;
        }

        private string ObtenerInformacionJson()
        {
            StringBuilder CodigoExtracionInformacion = new StringBuilder();

            CodigoExtracionInformacion.Append("let CantidadElementos = document.querySelector('body > ui - layout > div > div > div.main - app - view.ng - scope > et - discovery - markets - results > div > div > et - discovery - markets - results - grid > div').getElementsByTagName('et-instrument-card').length;");
            CodigoExtracionInformacion.Append("let ArrayEmpresas = [];");
            CodigoExtracionInformacion.Append("for(i = 1; i < CantidadElementos+1; i++){");
            CodigoExtracionInformacion.Append("let ArrayEmpresa  = new Object();");
            CodigoExtracionInformacion.Append("let SiglaEmpresa = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child('+i+')').getElementsByClassName('symbol')[0].innerText;");
            CodigoExtracionInformacion.Append("let NombreEmpresa = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child('+i+') > et-instrument-trading-card > div > header > et-card-avatar > a > div.avatar-info > div.name').innerText;");
            CodigoExtracionInformacion.Append("let Rendimiento;");
            CodigoExtracionInformacion.Append("if(i == 1){");
            CodigoExtracionInformacion.Append("Rendimiento = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child(1) > et-instrument-trading-card > div > section.instrument-card-chart-wrap').getElementsByTagName('H6')[1].innerText;");
            CodigoExtracionInformacion.Append("}else{");
            CodigoExtracionInformacion.Append("Rendimiento = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child('+i+') > et-instrument-trading-card > div > section.instrument-card-chart-wrap').getElementsByTagName('H6')[1].innerText;");
            CodigoExtracionInformacion.Append("}");
            CodigoExtracionInformacion.Append("let PrecioCompra = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child('+i+') > et-instrument-trading-card > div > et-buy-sell-buttons > et-buy-sell-button:nth-child(3) > div > div.price').innerText;");
            CodigoExtracionInformacion.Append("let PrecioVenta = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child('+i+') > et-instrument-trading-card > div > et-buy-sell-buttons > et-buy-sell-button:nth-child(1) > div > div.price').innerText;");
            CodigoExtracionInformacion.Append("ArrayEmpresa.SiglaEmpresa = SiglaEmpresa;");
            CodigoExtracionInformacion.Append("ArrayEmpresa.NombreEmpresa = NombreEmpresa;");
            CodigoExtracionInformacion.Append("ArrayEmpresa.Rendimiento = Rendimiento.replace('%','');");
            CodigoExtracionInformacion.Append("ArrayEmpresa.PrecioCompra = PrecioCompra.replace(',','.');");
            CodigoExtracionInformacion.Append("ArrayEmpresa.PrecioVenta = PrecioVenta.replace(',','.');");
            CodigoExtracionInformacion.Append("ArrayEmpresas.push(ArrayEmpresa);");
            CodigoExtracionInformacion.Append("}");
            CodigoExtracionInformacion.Append("var myJsonString = JSON.stringify(ArrayEmpresas);");
            //CodigoExtracionInformacion.Append("console.log(myJsonString);");
            CodigoExtracionInformacion.Append("return myJsonString;");
            return CodigoExtracionInformacion.ToString();
        }

        private List<Empresas> ObtenerInformacionEstadistica(List<Empresas> ListaEmpresas)
        {
            foreach (Empresas Empresa in ListaEmpresas)
            {
                //Ingresamos a las estadisticas directas de las empresas.
                string UrlEmpresa = string.Format("https://www.etoro.com/es/markets/{0}", Empresa.SiglaEmpresa);
                sele.CambiarUrl(UrlEmpresa+ "/stats");
                //Esperamos a que cargen las estadisticas.
                if (sele.BuscarElemento("xpath", "/html/body/ui-layout/div/div/div[2]/et-market/div/div/div/div[3]/et-market-stats/et-market-stats-overview/et-card/section/et-card-content/div[4]/div[1]/div", 30))
                {
                    string SectorEmpresa = "No se encontraron datos";
                    string IndustriaTecnologia = "No se encontraron datos";
                    string NombreCompletoMercado = "No se encontraron datos";
                    try
                    {
                        NombreCompletoMercado = sele.ObtenerTexto("xpath", "/html/body/ui-layout/div/div/div[2]/et-market/div/div/et-market-header/div/div[1]/div[2]/div[3]");
                        SectorEmpresa = sele.BuscarElementoYObtenerTexto("a", "class", "sector-link ng-star-inserted");
                        IndustriaTecnologia = sele.ObtenerTexto("xpath", "/html/body/ui-layout/div/div/div[2]/et-market/div/div/div/div[3]/et-market-stats/et-market-stats-overview/et-card/section/et-card-content/div[4]/div[2]/div/strong");
                    }
                    catch (Exception ex)
                    {

                    }
                    Empresa.Sector = SectorEmpresa;
                    Empresa.Industria = IndustriaTecnologia;
                    Empresa.NombreCompletoMercado = NombreCompletoMercado;
                    Empresa.TipoMercado = TipoMercado(NombreCompletoMercado);
                    Empresa.UrlEmpresa = UrlEmpresa;
                }
            }
            
            return ListaEmpresas;
        }
        private string TipoMercado(string NombreCompletoMercado)
        {
            string TipoMercado = "No se encontraron datos";

            if (NombreCompletoMercado.ToLower().Contains("nasdaq"))
            {
                return TipoMercado = "GRINGA";
            }
            if (NombreCompletoMercado.ToLower().Contains("xignite"))
            {
                return TipoMercado = "EUROPA?";
            }
            return TipoMercado;
        }
    }
}

