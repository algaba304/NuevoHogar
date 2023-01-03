using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.ModeloDTO;
using NuevoHogar.ModeloAuxiliar;
using Newtonsoft.Json.Serialization;
using NuevoHogar.Utils.Convertidores;
using NuevoHogar.Utils;

namespace NuevoHogar.Controllers;

public class AdministracionController : Controller{

    private readonly IHttpClientFactory? _IhttpClientFactory;
    public AdministracionController(IHttpClientFactory httpClientFactory){

        _IhttpClientFactory = httpClientFactory;

    }

    public async System.Threading.Tasks.Task<IActionResult> ConsultarListaSolicitudes(){

        try{

            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage mensaje = await cliente!.GetAsync("api/administradores/" + Cliente.Usuario!.IdUsuario + "/refugios");

            if(mensaje.IsSuccessStatusCode){

                string listaSolicitudes = mensaje.Content.ReadAsStringAsync().Result;

                if(listaSolicitudes != null){
                    
                    List<UsuarioDTO>? lista = JsonConvert.DeserializeObject<List<UsuarioDTO>>(listaSolicitudes);
                    return View(lista);

                }

            }

            return RedirectToAction("ErrorPagina", "Home");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");
        }
    }

    public async System.Threading.Tasks.Task<IActionResult> AceptarSolicitud(){

        try{

            string idUsuario = "";

            if(!String.IsNullOrEmpty(HttpContext.Request.Query["idUsuario"])){

                idUsuario = HttpContext!.Request!.Query["idUsuario"]!;

            }else{

                return RedirectToAction("ErrorPagina", "Home");

            }

            UsuarioDTO usuario = new UsuarioDTO();
            usuario.EstadoUsuario = "Aceptado";

            DefaultContractResolver contractResolver = new DefaultContractResolver{

                NamingStrategy = new CamelCaseNamingStrategy()

            };

            string json = JsonConvert.SerializeObject(usuario, new JsonSerializerSettings{

                ContractResolver = contractResolver,
                Formatting = Formatting.Indented

            });

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage mensaje = await cliente!.PutAsync("api/usuarios/" + idUsuario + "/solicitudes", data);

            if(mensaje.IsSuccessStatusCode){

                string exito = mensaje.Content.ReadAsStringAsync().Result;

                if(exito != null){

                    MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(exito);
                    return RedirectToAction("ConsultarListaSolicitudes", "Administracion");

                }

            }else{

                string noExito = mensaje.Content.ReadAsStringAsync().Result;

                if(noExito != null){

                    MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(noExito);

                }

            }

            return RedirectToAction("ErrorPagina", "Home");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public async System.Threading.Tasks.Task<IActionResult> CancelarSolicitud(){

        try{

            string idUsuario = "";

            if(!String.IsNullOrEmpty(HttpContext.Request.Query["idUsuario"])){

                idUsuario = HttpContext!.Request!.Query["idUsuario"]!;

            }else{

                return RedirectToAction("ErrorPagina", "Home");

            }

            UsuarioDTO usuario = new UsuarioDTO();
            usuario.EstadoUsuario = "Eliminado";

            DefaultContractResolver contractResolver = new DefaultContractResolver{

                NamingStrategy = new CamelCaseNamingStrategy()

            };

            string json = JsonConvert.SerializeObject(usuario, new JsonSerializerSettings{

                ContractResolver = contractResolver,
                Formatting = Formatting.Indented

            });

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage mensaje = await cliente!.PutAsync("api/usuarios/" + idUsuario + "/solicitudes", data);

            if(mensaje.IsSuccessStatusCode){

                string exito = mensaje.Content.ReadAsStringAsync().Result;

                if(exito != null){

                    MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(exito);
                    Console.WriteLine(mensajeResultante!.Mensaje);
                    return RedirectToAction("ConsultarListaSolicitudes", "Administracion");

                }

            }else{

                string noExito = mensaje.Content.ReadAsStringAsync().Result;

                if(noExito != null){

                    MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(noExito);

                }

            }

            return RedirectToAction("ErrorPagina", "Home");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }
}