using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.Utils;
using NuevoHogar.ModeloDTO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NuevoHogar.Controllers;

public class ManejoReporteController : Controller{
    private readonly IHttpClientFactory? _IhttpClientFactory;
    public ManejoReporteController(IHttpClientFactory httpClientFactory){
        _IhttpClientFactory = httpClientFactory;
    }
    
    public IActionResult GenerarReporte(){
        try{
            return View();
        }catch(Exception ex){
            return RedirectToAction("ErrorPagina", "Home");
        }
    }

    public async System.Threading.Tasks.Task<IActionResult> EditReporte(string? s){
        Reporte2DTO reporteAux = new Reporte2DTO();
        ReporteDTO reporte = new ReporteDTO();
        try{
            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var mensaje = await cliente?.GetAsync("api/reportes/" + s)!;

            if(mensaje.IsSuccessStatusCode){
                string cadena = mensaje.Content.ReadAsStringAsync().Result;
                reporteAux = JsonConvert.DeserializeObject<Reporte2DTO>(cadena);
                System.Console.WriteLine(reporteAux.idReporteDeMaltrato);
            }
            reporte = convertirReporte(reporteAux);
            return View(reporte);
        }catch(Exception ex){
            return RedirectToAction("ErrorPagina", "Home");
        }
    }

    private ReporteDTO convertirReporte(Reporte2DTO r){
        ReporteDTO reporte = new ReporteDTO();
        reporte.idReporteDeMaltrato = r.idReporteDeMaltrato;
        reporte.ubicacion = r.ubicacion;
        reporte.descripcion = r.descripcion;
        reporte.fecha = r.fecha;
        reporte.idRefugio = r.idRefugio;
        reporte.idUsuario = r.idUsuario;
        return reporte;
    }

    public async System.Threading.Tasks.Task<IActionResult> ListaReportesUsuario(){
        List<Reporte2DTO> listaReportes = new List<Reporte2DTO>();
        List<Reporte2DTO> listaFiltrada = new List<Reporte2DTO>();
        try{
            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var mensaje = await cliente?.GetAsync("api/reportes/lastReports/20")!;

            System.Console.WriteLine(mensaje.ToString());

            if(mensaje.IsSuccessStatusCode){
                string listaStrings = mensaje.Content.ReadAsStringAsync().Result;
                listaReportes = JsonConvert.DeserializeObject<List<Reporte2DTO>>(listaStrings);
            }

            foreach(Reporte2DTO r in listaReportes){
                if(r.idUsuario == Cliente.Usuario.IdUsuario)
                    listaFiltrada.Add(r);
            }

            return View(listaFiltrada);
        }catch(Exception ex){
            return RedirectToAction("ErrorPagina", "Home");
        }
    }

    public async System.Threading.Tasks.Task<IActionResult> ListaReportes(){
        List<Reporte2DTO> listaReportes = new List<Reporte2DTO>();
        try{
            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var mensaje = await cliente?.GetAsync("api/reportes/lastReports/20")!;

            System.Console.WriteLine(mensaje.ToString());

            if(mensaje.IsSuccessStatusCode){
                string listaStrings = mensaje.Content.ReadAsStringAsync().Result;
                listaReportes = JsonConvert.DeserializeObject<List<Reporte2DTO>>(listaStrings);
            }

            return View(listaReportes);
        }catch(Exception ex){
            return RedirectToAction("ErrorPagina", "Home");
        }
    }

    public async System.Threading.Tasks.Task<IActionResult> GenerarReporteBtn(ReporteDTO reporteAux){
        try{
            System.Console.WriteLine(reporteAux.fecha);
            reporteAux.idUsuario = Cliente.Usuario.IdUsuario;
            reporteAux.idRefugio = null;
            reporteAux.idReporteDeMaltrato = Guid.NewGuid().ToString();
            System.Console.WriteLine(reporteAux.idReporteDeMaltrato);

            DefaultContractResolver contractResolver = new DefaultContractResolver{

                NamingStrategy = new CamelCaseNamingStrategy()

            };

            string json = JsonConvert.SerializeObject(reporteAux, new JsonSerializerSettings{

                ContractResolver = contractResolver,
                Formatting = Formatting.Indented

            });

            System.Console.WriteLine(json);

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
            HttpResponseMessage mensaje = await cliente?.PostAsync("api/reportes/", data)!;
            
            System.Console.WriteLine(mensaje.ToString());

            if(mensaje.IsSuccessStatusCode){
                return RedirectToAction("HomepageAnimalista", "Homepage");
            }else{
                return RedirectToAction("ErrorPagina", "Home");
            }

        }catch(Exception ex){
            return RedirectToAction("ErrorPagina", "Home");
        }

    }

}