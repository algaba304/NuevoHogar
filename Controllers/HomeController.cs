using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.Models;

namespace NuevoHogar.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory? _IhttpClientFactory;
    public HomeController(IHttpClientFactory httpClientFactory){
        _IhttpClientFactory = httpClientFactory;
    }

    public IActionResult Index(){
        Cliente.Usuario = null;
        return View();
    }

    public IActionResult Privacy(){
        return View();
    }

    public IActionResult ErrorPagina(){
        return View();
    }


    [HttpPost]
    public async System.Threading.Tasks.Task<IActionResult> IniciarSesion(Usuario usuario){

        try{
            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var mensaje = await cliente?.GetAsync("api/sesion/?nombreUsuario=" + usuario.NombreUsuario + "&contrasenia=" + usuario.Contrasenia)!;

            if(mensaje.IsSuccessStatusCode){
                string usuarioObtenido = mensaje.Content.ReadAsStringAsync().Result;
                if(usuarioObtenido != null){
                    Usuario? usuarioEncontrado = JsonConvert.DeserializeObject<Usuario>(usuarioObtenido);
                    Cliente.Usuario = usuarioEncontrado;
                }else{
                    return RedirectToAction("ErrorNoExiste", "Home");
                }
            }else{
                return RedirectToAction("ErrorPagina", "Home");
            }
            return RedirectToAction("MostrarListaReportes", "Homepage");
        }catch(Exception ex){
            return RedirectToAction("ErrorPagina", "Home");
        }
    }
}
