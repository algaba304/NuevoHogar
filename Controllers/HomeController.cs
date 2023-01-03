using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.ModeloDTO;
using NuevoHogar.Utils;

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

        Cliente.Usuario = null;
        return View();

    }

    public IActionResult PaginaNoEncontrada(){

        return View();

    }

    public IActionResult ErrorNoExiste(){

        return View();

    }

    public async System.Threading.Tasks.Task<IActionResult> IniciarSesionBtn(UsuarioDTO usuario){

        try{

            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var mensaje = await cliente?.GetAsync("api/sesion/?nombreUsuario=" + usuario.NombreUsuario + "&contrasenia=" + usuario.Contrasenia)!;

            if(mensaje.StatusCode == System.Net.HttpStatusCode.NotFound){

                return RedirectToAction("ErrorNoExiste", "Home");

            }
            
            if(mensaje.IsSuccessStatusCode){

                string usuarioObtenido = mensaje.Content.ReadAsStringAsync().Result;

                if(usuarioObtenido != null){

                    UsuarioDTO? usuarioEncontrado = JsonConvert.DeserializeObject<UsuarioDTO>(usuarioObtenido);
                    Cliente.Usuario = usuarioEncontrado;
                    
                }else{

                    return RedirectToAction("ErrorNoExiste", "Home");

                }

            }else{

                return RedirectToAction("ErrorPagina", "Home");

            }

            if(Cliente.Usuario!.Rol!.IdRol != null){

                return RedirectToAction("Homepage", "Homepage");

            }

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }catch(Exception ex){
            Console.WriteLine(ex);
            return RedirectToAction("ErrorPagina", "Home");

        }

    }
    
}
