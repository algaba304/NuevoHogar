using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.Models;

namespace NuevoHogar.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private String baseURL = "localhost:8080/";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async System.Threading.Tasks.Task<IActionResult> IniciarSesion(String usuario){

        Usuario usuario1 = new Usuario();
        using(var cliente = new HttpClient()){
            cliente.BaseAddress = new System.Uri(baseURL);
            cliente.DefaultRequestHeaders.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage mensaje = await cliente.GetAsync("api/usuarios/" + usuario);

            if(mensaje.IsSuccessStatusCode){
                string usuarioObtenido = mensaje.Content.ReadAsStringAsync().Result;
                if(usuarioObtenido != null){
                    usuario1 = JsonSerializer.Deserialize<Usuario>(usuarioObtenido);
                }else{
                    return RedirectToAction("ErrorNoExiste", "Home");
                }
            }else{
                return RedirectToAction("ErrorPagina", "Home");
            }
        }
        return View(usuario1);
    }

}
