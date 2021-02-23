let CantidadElementos = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div').getElementsByTagName('et-instrument-card').length;
let ArrayEmpresas = [];
for (i = 1; i < CantidadElementos+1; i++) {
    let ArrayEmpresa = new Object();
    let SiglaEmpresa = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child(2)').getElementsByClassName('symbol')[0].innerText;
    let NombreEmpresa = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child(' + i + ') > et-instrument-trading-card > div > header > et-card-avatar > a > div.avatar-info > div.name').innerText;
    let Rendimiento;
    if (i == 1) {
        Rendimiento = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child(1) > et-instrument-trading-card > div > section.instrument-card-chart-wrap').getElementsByTagName('H6')[1].innerText;
    } else {
        Rendimiento = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child('+i+') > et-instrument-trading-card > div > section.instrument-card-chart-wrap').getElementsByTagName('H6')[1].innerText;
    }
    let PrecioCompra = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child(' + i + ') > et-instrument-trading-card > div > et-buy-sell-buttons > et-buy-sell-button:nth-child(3) > div > div.price').innerText;
    let PrecioVenta = document.querySelector('body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > div > et-discovery-markets-results-grid > div > et-instrument-card:nth-child(' + i + ') > et-instrument-trading-card > div > et-buy-sell-buttons > et-buy-sell-button:nth-child(1) > div > div.price').innerText;

    let RendimientoFiltrado = Rendimiento.replace('%', '');
    if(RendimientoFiltrado == 0){
        RendimientoFiltrado = 1.1;
    }
    ArrayEmpresa.SiglaEmpresa = SiglaEmpresa;
    ArrayEmpresa.NombreEmpresa = NombreEmpresa;
    ArrayEmpresa.Rendimiento = RendimientoFiltrado;
    ArrayEmpresa.PrecioCompra = PrecioCompra.replace(',', '.');
    ArrayEmpresa.PrecioVenta = PrecioVenta.replace(',', '.');
    ArrayEmpresas.push(ArrayEmpresa);
}

var myJsonString = JSON.stringify(ArrayEmpresas);
console.log(myJsonString);
//return myJsonString;

//-------------------------------------------------------------------
//Obtener cantidad acciones
let CantidadAcciones =document.querySelector("body > ui-layout > div > div > div.main-app-view.ng-scope > et-discovery-markets-results > div > et-discovery-markets-results-header > div > div.inner-menu").getElementsByClassName('paging-bold')[1].innerText;


return Math.round(CantidadAcciones / 50);