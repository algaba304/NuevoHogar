using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.Utils;
using NuevoHogar.ModeloDTO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NuevoHogar.Controllers;

public class HomepageController : Controller{

    private String idAnimalista = "AN_123_R";
    private String idRefugio = "RF_123_R";
    private String idAdministrador = "AD_123_R";

    private readonly IHttpClientFactory? _IhttpClientFactory;
    public HomepageController(IHttpClientFactory httpClientFactory){
        _IhttpClientFactory = httpClientFactory;
    }

    public IActionResult Homepage(){

        if(Cliente.Usuario!.Rol!.IdRol == idAnimalista){
            
            return RedirectToAction("HomepageAnimalista", "Homepage");

        }else if(Cliente.Usuario!.Rol!.IdRol == idRefugio){

            return RedirectToAction("HomepageRefugio", "Homepage");
            
        }else if(Cliente.Usuario!.Rol!.IdRol == idAdministrador){

            return RedirectToAction("HomepageAdministrador", "Homepage");

        }else{

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }

    }

    public IActionResult HomepageAnimalista(){

        if(Cliente.Usuario!.Rol!.IdRol == idAnimalista){
            
            return View();

        }else{

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }
        
    }

    public async System.Threading.Tasks.Task<IActionResult> HomepageRefugio(){

        if(Cliente.Usuario!.Rol!.IdRol == idRefugio){
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
            //return View();

        }else{

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }

    }

    public IActionResult HomepageAdministrador(){

        if(Cliente.Usuario!.Rol!.IdRol == idAdministrador){

            return View();

        }else{

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }

    }
}