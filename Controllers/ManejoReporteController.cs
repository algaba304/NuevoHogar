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