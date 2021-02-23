using Automatizacion.Selenium.Generales;
using FL.Automatizacion.Store.Generales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                    sele.CloseChrome();
                    foreach (Empresas Empresa in ListaEmpresa)
                    {
                        Empresa.MotorMercado = true;
                        EmpresaDal.IngresarUsuario(Empresa);
                        //EmpresaDal.Agregar(Empresa);
                    }
                }
            }
            sele.CloseChrome();
            return false;
        }

        public async Task<bool> DescargarInfomracionAccionesAsync()
        {
            //Valadimas ingreso de la pagina de etoro o la carga de la misma.
            if (IngresoPlataforma())
            {
                //Ingresamos a las acciones
                sele.CambiarUrl("https://www.etoro.com/discover/markets/stocks/exchange/nasdaq");
                if (sele.BuscarElemento("xpath", "//div[@class='discovery-tag-wrapper']"))
                {
                    Thread.Sleep(1);
                    //Abrimis el CBO de busqueda de acciones
                    sele.ClickSelenium("xpath", "/html/body/ui-layout/div/div/div[2]/et-discovery-markets-results/div/et-discovery-markets-results-header/div/div[1]/div/div[2]/div[2]/et-select[2]/div");
                    
                    try
                    {
                        Thread.Sleep(2);
                        Thread.Sleep(1);
                        //Obtenemos la cantidad a buscar
                        int CantidadElementos = Convert.ToInt32(sele.EjecutarJS("let CantidadMercadosAcciones = 0;try{CantidadMercadosAcciones = document.querySelector('div.dlg-body')[0].getElementsByTagName('a').length;}catch (error){CantidadMercadosAcciones = document.querySelector('div.dlg-body').getElementsByTagName('a').length;}                        return CantidadMercadosAcciones;"));
                        List<Empresas> ListaEmpresa = new List<Empresas>();
                        for (int i = 0; i < CantidadElementos; i++)
                        {
                            if (i != 0)
                            {
                                //Abrimis el CBO de busqueda de acciones
                                sele.ClickSelenium("xpath", "/html/body/ui-layout/div/div/div[2]/et-discovery-markets-results/div/et-discovery-markets-results-header/div/div[1]/div/div[2]/div[2]/et-select[2]/div");
                                Thread.Sleep(1);
                                Thread.Sleep(2);
                                //Recorremos las acciones y cambiamos de empresa
                                sele.EjecutarJS("try{document.querySelector('div.dlg-body')[0].getElementsByTagName('a')["+i+ "].click(); }catch (error) {document.querySelector('div.dlg-body').getElementsByTagName('a')["+i+"].click(); } ");
                            }
                            
                            try
                            {
                                Thread.Sleep(2);
                                //La cantidad de acciones es menor 50 ocurrira este error
                                int CantidadAcciones = 0;
                                try
                                {
                                    CantidadAcciones = Convert.ToInt32(sele.EjecutarJS("let CantidadAcciones =document.getElementsByClassName('inner-menu')[0].getElementsByClassName('paging-bold')[1].innerText; CantidadAcciones = Math.round(CantidadAcciones / 50); return CantidadAcciones;"));

                                }
                                catch (Exception ex)
                                {
                                    CantidadAcciones = 1;
                                }
                                
                                string JsonData = "";
                                //Recorremos las acciones
                                for (int Acciones = 0; Acciones < CantidadAcciones; Acciones++)
                                {
                                    //Esperamos a que carge el primer elemento
                                    if (sele.BuscarElemento("xpath", "/html/body/ui-layout/div/div/div[2]/et-discovery-markets-results/div/div/et-discovery-markets-results-grid/div/et-instrument-card[1]"))
                                    {
                                        JsonData = sele.EjecutarJS(ObtenerInformacionJson());

                                        try
                                        {
                                            List<Empresas> ListaEmpresa2 = new List<Empresas>();
                                            ListaEmpresa2 = JsonConvert.DeserializeObject<List<Empresas>>(JsonData);
                                            foreach (Empresas empresa2 in ListaEmpresa2)
                                            {
                                                try
                                                {
                                                    ListaEmpresa.Add(empresa2);
                                                }
                                                catch (Exception ExEmpresa)
                                                {
                                                    Console.WriteLine(empresa2);
                                                    Console.WriteLine(ExEmpresa);
                                                }

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(JsonData);
                                            Console.WriteLine(ex);
                                        }
                                    }
                                    sele.ClickSelenium("xpath", "//a[@class='menu-item-button ng-star-inserted']//span[@class='nav-button-right sprite']");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        sele.CloseChrome();
                        ListaEmpresa = await ObtenerEstadisticasAsync(ListaEmpresa);
                        foreach (Empresas Empresa in ListaEmpresa)
                        {
                            Empresa.MotorMercado = false;
                            EmpresaDal.IngresarUsuario(Empresa);
                            //EmpresaDal.Agregar(Empresa);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    

                }
            }
            return false;
        }
        private async Task<List<Empresas>> ObtenerEstadisticasAsync(List<Empresas> ListEmpresas)
        {
            try
            {
                int CantidadAccionesEmpresas = ListEmpresas.Count() / 7;
                List<Empresas> Largo1 = ListEmpresas.GetRange(0, CantidadAccionesEmpresas);
                List<Empresas> Largo2 = ListEmpresas.GetRange(Largo1.Count(), CantidadAccionesEmpresas);
                List<Empresas> Largo3 = ListEmpresas.GetRange(Largo2.Count() * 2, CantidadAccionesEmpresas);
                List<Empresas> Largo4 = ListEmpresas.GetRange(Largo3.Count() * 3, CantidadAccionesEmpresas);
                List<Empresas> Largo5 = ListEmpresas.GetRange(Largo4.Count() * 4, CantidadAccionesEmpresas);
                List<Empresas> Largo6 = ListEmpresas.GetRange(Largo4.Count() * 5, CantidadAccionesEmpresas);
                List<Empresas> Largo7 = ListEmpresas.GetRange(Largo4.Count() * 6, CantidadAccionesEmpresas);

                List<Empresas> ListEmpresas1 = new List<Empresas>();
                List<Empresas> ListEmpresas2 = new List<Empresas>();
                List<Empresas> ListEmpresas3 = new List<Empresas>();
                List<Empresas> ListEmpresas4 = new List<Empresas>();
                List<Empresas> ListEmpresas5 = new List<Empresas>();
                List<Empresas> ListEmpresas6 = new List<Empresas>();
                List<Empresas> ListEmpresas7 = new List<Empresas>();
                Parallel.Invoke(() =>
                {
                    ListEmpresas1 = ObtenerInformacionEstadistica(Largo1);
                    foreach (Empresas Empresa in ListEmpresas1)
                    {
                        ListEmpresas.Add(Empresa);
                    }
                },
                () =>
                {
                    ListEmpresas2 = ObtenerInformacionEstadistica(Largo2);
                    foreach (Empresas Empresa in ListEmpresas2)
                    {
                        ListEmpresas.Add(Empresa);
                    }
                },
                () =>
                {
                    ListEmpresas3 = ObtenerInformacionEstadistica(Largo3);
                    foreach (Empresas Empresa in ListEmpresas3)
                    {
                        ListEmpresas.Add(Empresa);
                    }
                },
                () =>
                {
                    ListEmpresas4 = ObtenerInformacionEstadistica(Largo4);
                    foreach (Empresas Empresa in ListEmpresas4)
                    {
                        ListEmpresas.Add(Empresa);
                    }
                },
                () =>
                {
                    ListEmpresas5 = ObtenerInformacionEstadistica(Largo5);
                    foreach (Empresas Empresa in ListEmpresas5)
                    {
                        ListEmpresas.Add(Empresa);
                    }
                },
                () =>
                {
                    ListEmpresas6 = ObtenerInformacionEstadistica(Largo6);
                    foreach (Empresas Empresa in ListEmpresas6)
                    {
                        ListEmpresas.Add(Empresa);
                    }
                },
                () =>
                {
                    ListEmpresas7 = ObtenerInformacionEstadistica(Largo7);
                    foreach (Empresas Empresa in ListEmpresas7)
                    {
                        ListEmpresas.Add(Empresa);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return ListEmpresas;
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
            CodigoExtracionInformacion.Append("let RendimientoFiltrado = Rendimiento.replace('%', '').replace('>', '').replace('<', '');");
            CodigoExtracionInformacion.Append("if(RendimientoFiltrado == 0){");
            CodigoExtracionInformacion.Append("RendimientoFiltrado = '0.1';");
            CodigoExtracionInformacion.Append("}");
            CodigoExtracionInformacion.Append("ArrayEmpresa.Rendimiento = RendimientoFiltrado;");
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
            ConfiguracionChrome config = new ConfiguracionChrome();
            config.maximaChorme();
            Sele sele = new Sele(config.getOptions());
            sele.OpenChorme();
            int Contador = 0;
            foreach (Empresas Empresa in ListaEmpresas)
            {
                //Ingresamos a las estadisticas directas de las empresas.
                string UrlEmpresa = string.Format("https://www.etoro.com/es/markets/{0}", Empresa.SiglaEmpresa);
                sele.CambiarUrl(UrlEmpresa+ "/stats");
                //Esperamos a que cargen las estadisticas.
                if (sele.BuscarElemento("xpath", "/html/body/ui-layout/div/div/div[2]/et-market/div/div/div/div[3]/et-market-stats/et-market-stats-overview/et-card/section/et-card-content/div[4]/div[1]/div", 5))
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
                    try
                    {
                        if (SectorEmpresa == null)
                        {
                            Console.WriteLine();
                        }
                        Empresa.Sector = SectorEmpresa;
                        Empresa.Industria = IndustriaTecnologia;
                        Empresa.NombreCompletoMercado = NombreCompletoMercado;
                        Empresa.TipoMercado = TipoMercado(NombreCompletoMercado);
                        Empresa.UrlEmpresa = UrlEmpresa;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                Contador = Contador + 1;
                Console.WriteLine("Total de Acciones: "+ ListaEmpresas.Count()+"  Accion actual: "+ Contador);
            }
            sele.CloseChrome();
            Console.WriteLine("Proceso Chrome Cerrado");
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

