using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.Models;
using NuevoHogar.ModeloAuxiliar;
using Newtonsoft.Json.Serialization;
using NuevoHogar.Utils;

namespace NuevoHogar.Controllers;

public class AccesoController : Controller{

    private readonly IHttpClientFactory? _IhttpClientFactory;
    public AccesoController(IHttpClientFactory httpClientFactory){

        _IhttpClientFactory = httpClientFactory;

    }
    private String idRefugio = "RF_123_R";
    private String idAnimalista = "AN_123_R";

    public async System.Threading.Tasks.Task<IActionResult> RegistrarCuenta(){

        try{
            
            UsuarioAuxiliar usuario = new UsuarioAuxiliar();
            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage mensaje = await cliente?.GetAsync("api/roles/")!;

            if(mensaje.IsSuccessStatusCode){

                string listaRoles = mensaje.Content.ReadAsStringAsync().Result;
                
                if(listaRoles != null){

                    List<Rol>? roles = JsonConvert.DeserializeObject<List<Rol>>(listaRoles);
                    usuario.Roles = roles;

                }else{

                    return RedirectToAction("Index", "Home");
                }

            }else if(((int)mensaje.StatusCode) == 500){

                return RedirectToAction("ErrorPagina", "Home");

            }

            return View(usuario);

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }
    
    public async System.Threading.Tasks.Task<IActionResult> RegistrarCuentaBtn(UsuarioAuxiliar usuario){

        try{

            Usuario usuarioFinal = new Usuario();
            usuarioFinal.IdUsuario = Guid.NewGuid().ToString();
            usuarioFinal.Nombre = usuario.Nombre;
            usuarioFinal.NombreUsuario = usuario.NombreUsuario;
            usuarioFinal.CorreoElectronico = usuario.CorreoElectronico;
            usuarioFinal.ContadorReportes = usuario.ContadorReportes;
            usuarioFinal.NumeroTelefono = usuario.NumeroTelefono;
            usuarioFinal.Direccion = usuario.Direccion;
            usuarioFinal.Contrasenia = usuario.Contrasenia;
            usuarioFinal.FechaNacimiento = usuario.FechaNacimiento.ToString("yyyy-MM-dd");
            Rol rol = new Rol();
            usuarioFinal.Rol!.IdRol = usuario.RolSeleccionado;

            if(usuarioFinal.Rol.IdRol == idRefugio){

                usuarioFinal.EstadoUsuario = "En espera";

            }else{

                usuarioFinal.EstadoUsuario = "Aceptado";

            }
            
            DefaultContractResolver contractResolver = new DefaultContractResolver{

                NamingStrategy = new CamelCaseNamingStrategy()

            };

            string json = JsonConvert.SerializeObject(usuarioFinal, new JsonSerializerSettings{

                ContractResolver = contractResolver,
                Formatting = Formatting.Indented

            });

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage mensaje = await cliente?.PostAsync("api/usuarios/", data)!;

            if(mensaje.IsSuccessStatusCode){

                string exito = mensaje.Content.ReadAsStringAsync().Result;

                if(exito != null){

                    MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(exito);

                }else{

                    return RedirectToAction("ErrorPagina", "Home");

                }

            }else{

                string noExito = mensaje.Content.ReadAsStringAsync().Result;

                if(noExito != null){

                    MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(noExito);
                    return RedirectToAction("RegistrarCuenta", "Acceso");

                }else{

                    return RedirectToAction("ErrorPagina", "Home");

                }

            }

            if(usuarioFinal.Rol.IdRol == idRefugio){

                return RedirectToAction("Index", "Home");

            }else{

                Cliente.Usuario = usuarioFinal;
                return RedirectToAction("MostrarListaReportes", "Homepage");

            }
            
        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public IActionResult ConsultarCuenta(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idAnimalista || Cliente.Usuario!.Rol!.IdRol == idRefugio){

                return View(Cliente.Usuario);

            }else{

                return RedirectToAction("ErrorPagina", "Home");

            }

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public IActionResult CambiarContrasenia(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idAnimalista || Cliente.Usuario!.Rol!.IdRol == idRefugio){

                return View();

            }else{

                return RedirectToAction("ErrorPagina", "Home");

            }

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public async System.Threading.Tasks.Task<IActionResult> CambiarContraseniaBtn(String contrasenia, String contraseniaRepetida){

        try{

            Usuario usuario = Cliente.Usuario!;
            usuario.Contrasenia = contrasenia;
            var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
            cliente?.DefaultRequestHeaders.Clear();
            cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            DefaultContractResolver contractResolver = new DefaultContractResolver{

                NamingStrategy = new CamelCaseNamingStrategy()

            };

            string json = JsonConvert.SerializeObject(usuario, new JsonSerializerSettings{

                ContractResolver = contractResolver,
                Formatting = Formatting.Indented

            });

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage mensaje = await cliente?.PutAsync("api/usuarios/" + usuario.IdUsuario, data)!;

            if(mensaje.IsSuccessStatusCode){

                string exito = mensaje.Content.ReadAsStringAsync().Result;

                if(exito != null){

                    MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(exito);

                }else{

                    return RedirectToAction("ErrorPagina", "Home");

                }

            }else{

                string noExito = mensaje.Content.ReadAsStringAsync().Result;

                if(noExito != null){

                    MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(noExito);
                    return RedirectToAction("CambiarContrasenia", "Acceso");

                }else{

                    return RedirectToAction("ErrorPagina", "Home");

                }

            }

            return RedirectToAction("ConsultarCuenta", "Acceso");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

}