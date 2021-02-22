using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automatizacion.Selenium.Generales
{
    public class Sele
    {
        private IWebDriver driver;
        public ChromeOptions options;
        private bool isChromeOpen = false;
        public Sele(ChromeOptions options)
        {
            this.options = options;
        }

        //Se necesita agregar las configuraciones
        public void OpenChorme()
        {
            try
            {
                driver = new ChromeDriver(options);
                isChromeOpen = true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        public void CambiarUrl(string url)
        {
            try
            {
                //Validamos que el chrome esta abierto, en caso contrario lo ejecutaos de forma default
                if (isChromeOpen)
                {
                    driver.Url = url;
                }
                else
                {
                    driver = new ChromeDriver();
                    isChromeOpen = true;
                    driver.Url = url;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        public void BajarFinalPagina()
        {
            IJavaScriptExecutor jsEjecutor = (IJavaScriptExecutor)driver;
            jsEjecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }
        public bool ClickSelenium(string TipoDato, string DatoDarClick)
        {
            //Esta validacion es por que c# o visual studio le agrega unos espacio a los -;
            DatoDarClick = DatoDarClick.Replace(" - ", "-");
            TipoDato = TipoDato.ToUpper();
            try
            {
                switch (TipoDato)
                {
                    case "ID":
                        System.Console.WriteLine("Click en ID: " + DatoDarClick);
                        driver.FindElement(By.Id(DatoDarClick)).Click();
                        break;
                    case "XPATH":
                        System.Console.WriteLine("Click en XPath: " + DatoDarClick);
                        driver.FindElement(By.XPath(DatoDarClick)).Click();
                        break;
                    case "NAME":
                        System.Console.WriteLine("Click en NAME: " + DatoDarClick);
                        driver.FindElement(By.Name(DatoDarClick)).Click();
                        break;
                    default:
                        System.Console.WriteLine("Datos mal ingresado  TipoDato : {0}, DatoDarClick: {1}", TipoDato, DatoDarClick);
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine(ex);
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine("");
                return false;
            }
        }

        public string ObtenerTexto(string TipoDato, string DatoObtener)
        {
            //Esta validacion es por que c# o visual studio le agrega unos espacio a los -;
            DatoObtener = DatoObtener.Replace(" - ", "-");
            TipoDato = TipoDato.ToUpper();
            string Texto = "";
            try
            {
                switch (TipoDato)
                {
                    case "ID":
                        Texto = driver.FindElement(By.Id(DatoObtener)).Text;
                        System.Console.WriteLine("Obtener Texto ID: " + DatoObtener);
                        return Texto;
                    case "XPATH":
                        Texto = driver.FindElement(By.XPath(DatoObtener)).Text;
                        System.Console.WriteLine("Obtener Texto XPATH: " + DatoObtener);
                        return Texto;
                    case "NAME":
                        Texto = driver.FindElement(By.Name(DatoObtener)).Text;
                        System.Console.WriteLine("Obtener Texto NAME: " + DatoObtener);
                        return Texto;
                    default:
                        System.Console.WriteLine("Datos mal ingresado");
                        return "";

                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine(ex);
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine("");
                return "";
            }
        }

        public bool BuscarElemento(string TipoDato, string DatoBuscar, int tiempo = 30)
        {
            //Esta validacion es por que c# o visual studio le agrega unos espacio a los -;
            DatoBuscar = DatoBuscar.Replace(" - ", "-");
            TipoDato = TipoDato.ToUpper();
            IWebElement Elemento;
            bool Encontrado = false;
            int contador = 0;
            System.Console.WriteLine("Buscar Elemento : " + DatoBuscar);
            try
            {
                while (contador <= tiempo && Encontrado != true)
                {
                    try
                    {
                        switch (TipoDato)
                        {
                            case "ID":
                                Elemento = driver.FindElement(By.Id(DatoBuscar));

                                Encontrado = true;
                                break;
                            case "XPATH":
                                Elemento = driver.FindElement(By.XPath(DatoBuscar));
                                Encontrado = true;
                                break;
                            case "NAME":
                                Elemento = driver.FindElement(By.Name(DatoBuscar));
                                Encontrado = true;
                                break;
                            default:
                                System.Console.WriteLine("Datos mal ingresado");
                                Encontrado = false;
                                break;
                        }
                        System.Console.WriteLine("Segundos de espera : " + contador);

                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        contador = contador + 1;
                        System.Console.WriteLine("Segundo : " + contador);
                    }
                }
                System.Console.WriteLine("Elemento en contrado " + Encontrado);
                return Encontrado;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine(ex);
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine("");
                return false;
            }
        }

        public List<string> EjecutarJSObjecto(string js)
        {
            try
            {
                //Esta validacion es por que c# o visual studio le agrega unos espacio a los -;
                js = js.Replace(" - ", "-");
                List<String> ListaResultado = new List<String>();
                System.Console.WriteLine("Ejecutar JS : " + js);
                //Llamamos a la funcion de JS..
                IJavaScriptExecutor jsEjecutor = (IJavaScriptExecutor)driver;


                object ob = jsEjecutor.ExecuteScript(js);
                ReadOnlyCollection<object> list = (ReadOnlyCollection<object>)ob;
                //var obstr = JsonConvert.SerializeObject(list);


                foreach (var tes1 in list)
                {

                    ListaResultado.Add(tes1.ToString());
                }
                //ReadOnlyCollection<IWebElement> list = (ReadOnlyCollection<IWebElement>)ob;
                //string[] arr = Array.ConvertAll((object[])ob, Convert.ToString);
                return ListaResultado;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine(e);
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                return null;
            }

        }
        public string EjecutarJS(string js, bool segundoEsperar = true)
        {
            try
            {
                js = js.Replace(" - ", "-");
                System.Console.WriteLine("Ejecutar JS : " + js);

                //Esta validacion es por que c# o visual studio le agrega unos espacio a los -

                //Llamamos a la funcion de JS..
                IJavaScriptExecutor jsEjecutor = (IJavaScriptExecutor)driver;

                string items = Convert.ToString(jsEjecutor.ExecuteScript(js));
                if (segundoEsperar)
                {
                    Thread.Sleep(1000);
                }

                return items;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine(e);
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                return "false";
            }

        }

        public void EnterEnUnElemento(string xpath)
        {
            driver.FindElement(By.XPath(xpath)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }
        public void CloseChrome()
        {
            try
            {
                //driver.Close();
                driver.Quit();
                System.Console.WriteLine("Cerrando Chrome");
            }
            catch (Exception ex)
            {

            }

        }

        public void DefaultIframe()
        {
            try
            {
                driver.SwitchTo().DefaultContent();
                System.Console.WriteLine("Cambio IFRAME Principal");
            }
            catch (Exception e)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine(e);
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
            }
        }

        public void CambiarIframe(string TipoDato, string DatoBuscar)
        {
            TipoDato = TipoDato.ToUpper();
            try
            {
                switch (TipoDato)
                {

                    case "ID":
                        driver.SwitchTo().Frame(driver.FindElement(By.Id(DatoBuscar)));
                        System.Console.WriteLine("Cambio IFRAME ID : " + DatoBuscar);
                        break;
                    case "XPATH":
                        driver.SwitchTo().Frame(driver.FindElement(By.XPath(DatoBuscar)));
                        System.Console.WriteLine("Cambio IFRAME XPATH : " + DatoBuscar);
                        break;
                    case "NAME":
                        driver.SwitchTo().Frame(driver.FindElement(By.Name(DatoBuscar)));
                        System.Console.WriteLine("Cambio IFRAME NAME : " + DatoBuscar);
                        break;
                    default:
                        System.Console.WriteLine("Datos mal ingresado");
                        break;
                }
                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine(e);
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
            }
        }

        public void SendKeysSelenium(string TipoDato, string DatoBuscar, string Dato)
        {
            TipoDato = TipoDato.ToUpper();
            try
            {
                switch (TipoDato)
                {
                    case "ID":
                        driver.FindElement(By.Id(DatoBuscar)).SendKeys(Dato);
                        break;
                    case "XPATH":
                        driver.FindElement(By.XPath(DatoBuscar)).SendKeys(Dato);
                        break;
                    case "NAME":
                        driver.FindElement(By.Name(DatoBuscar)).SendKeys(Dato);
                        break;
                    default:
                        System.Console.WriteLine("Datos mal ingresado");
                        break;
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine(e);
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
            }

        }

        public List<string> ObtenerVentanasAbiertas()
        {
            return driver.WindowHandles.ToList();
        }

        public void ObtenerVentana(string NombreVentana)
        {
            driver.SwitchTo().Window(NombreVentana);
        }

        public string BuscarElementoYObtenerTexto(string Etiqueta, string TipoDatoBuscar, string NombreDatoBuscar)
        {
            //Esto funciona solo con XPATH

            //Etiqueta = son etiquetas HTML ej = a, input, img, png, etc.
            //TipoDatoBuscar = Se refiere al dato a buscar, en caso de ser ID, Class, name, etc.
            //NombreDatoBuscar= En este caso obtenemos solo el nombre del dato ej = en el ejemplo esta abajo y se muestra que es un hipervinculo y buscamos una clase que se llame "sector-link ng-star-inserted".
            // //a[@class='sector-link ng-star-inserted']

            //Esta validacion es por que c# o visual studio le agrega unos espacio a los -;
            NombreDatoBuscar = NombreDatoBuscar.Replace(" - ", "-");
            string DatoBuscar = string.Format("//{0}[@{1}='{2}']", Etiqueta, TipoDatoBuscar, NombreDatoBuscar);
            string Texto = "";
            try
            {
                Texto = driver.FindElement(By.XPath(DatoBuscar)).Text;
                System.Console.WriteLine("Obtener Texto XPATH: " + DatoBuscar);
                return Texto;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine(ex);
                System.Console.WriteLine("");
                System.Console.WriteLine("------------------------------------------------------------------------------------------------");
                System.Console.WriteLine("");
                return "";
            }
        }
    }
}
