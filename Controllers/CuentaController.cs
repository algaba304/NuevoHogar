using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.ModeloDTO;
using NuevoHogar.ModeloAuxiliar;
using Newtonsoft.Json.Serialization;
using NuevoHogar.Utils.Convertidores;
using NuevoHogar.Utils;

namespace NuevoHogar.Controllers;

public class CuentaController : Controller{

    private readonly IHttpClientFactory? _IhttpClientFactory;
    public CuentaController(IHttpClientFactory httpClientFactory){

        _IhttpClientFactory = httpClientFactory;

    }
    private String idRefugio = "RF_123_R";
    private String idAnimalista = "AN_123_R";
    private String idAdministrador = "AD_123_R";

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

                    List<RolDTO>? roles = JsonConvert.DeserializeObject<List<RolDTO>>(listaRoles);
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
    
    public async System.Threading.Tasks.Task<IActionResult> RegistrarCuentaBtn(UsuarioAuxiliar usuarioAuxiliar){

        try{

            UsuarioConverter usuarioConverter = new UsuarioConverter();
            UsuarioDTO usuario = usuarioConverter.convertirDesdeUsuario(usuarioAuxiliar);
            usuario.IdUsuario = Guid.NewGuid().ToString();
            RolDTO rol = new RolDTO();
            usuario.Rol = rol;
            usuario.Rol!.Nombre = usuarioAuxiliar.RolSeleccionado;

            if(usuario.Rol.Nombre == "Refugio"){

                usuario.Rol.IdRol = "RF_123_R";
                usuario.EstadoUsuario = "En espera";

            }else if(usuario.Rol.Nombre == "Animalista"){

                usuario.Rol.IdRol = "AN_123_R";
                usuario.EstadoUsuario = "Aceptado";

            }else{

                return RedirectToAction("ErrorPagina", "Home");

            }
            
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
                    return RedirectToAction("RegistrarCuenta", "Cuenta");

                }else{

                    return RedirectToAction("ErrorPagina", "Home");

                }

            }

            if(usuario.Rol.IdRol == idRefugio){

                return RedirectToAction("Index", "Home");

            }else if (usuario.Rol.IdRol == idAnimalista){

                Cliente.Usuario = usuario;
                return RedirectToAction("HomepageAnimalista", "Homepage");

            }else{

                return RedirectToAction("PaginaNoEncontrada", "Homepage");

            }
            
        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public IActionResult ConsultarCuenta(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idAnimalista || Cliente.Usuario!.Rol!.IdRol == idRefugio){
                
                UsuarioConverter usuarioConverter = new UsuarioConverter();
                UsuarioAuxiliar usuarioAuxiliar = usuarioConverter.convertirDesdeUsuarioDTO(Cliente.Usuario);
                usuarioAuxiliar.RolSeleccionado = Cliente.Usuario.Rol.Nombre;
                return View(usuarioAuxiliar);

            }else{

                return RedirectToAction("EditarCuenta", "Home");

            }

        }catch(Exception ex){
            
            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public async System.Threading.Tasks.Task<IActionResult> ConsultarRedesSociales(){
        try{

            if(Cliente.Usuario!.Rol!.IdRol == idRefugio){

                if(Cliente.Usuario.ListaRedes != null){

                    return View(Cliente.Usuario.ListaRedes);

                }

                var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
                cliente?.DefaultRequestHeaders.Clear();
                cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var mensaje = await cliente?.GetAsync("api/usuarios/" + Cliente.Usuario.IdUsuario + "/redesSociales")!;
                List<EnlaceRedSocialDTO>? listaRedes = new List<EnlaceRedSocialDTO>();

                if(mensaje.StatusCode == System.Net.HttpStatusCode.NoContent){

                    cliente?.DefaultRequestHeaders.Clear();
                    cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    mensaje = await cliente?.GetAsync("api/tiposRed/")!;

                    if(mensaje.StatusCode == System.Net.HttpStatusCode.NoContent){

                        return RedirectToAction("ErrorPagina", "Home");

                    }else if(mensaje.IsSuccessStatusCode){

                        string listaTiposString = mensaje.Content.ReadAsStringAsync().Result;
                        List<TipoRedSocialDTO>? listaTiposObtenidos = JsonConvert.DeserializeObject<List<TipoRedSocialDTO>>(listaTiposString);

                        for(int i = 0; i < listaTiposObtenidos!.Count; i ++){

                            EnlaceRedSocialDTO enlace = new EnlaceRedSocialDTO();
                            enlace.Nombre = listaTiposObtenidos[i].Nombre;
                            enlace.IdUsuario = Cliente.Usuario.IdUsuario;
                            enlace.IdTipoRedSocial = listaTiposObtenidos[i].IdTipoRedSocial;
                            listaRedes.Add(enlace);
                            
                        }

                        Cliente.Usuario!.ListaRedes = listaRedes;
                        return View(listaRedes);

                    }

                }else if(mensaje.IsSuccessStatusCode){
                    
                    string listaRedesString = mensaje.Content.ReadAsStringAsync().Result;
                    listaRedes = JsonConvert.DeserializeObject<List<EnlaceRedSocialDTO>>(listaRedesString);
                    Cliente.Usuario.ListaRedes = listaRedes;
                    return View(listaRedes);

                }

                return RedirectToAction("ErrorPagina", "Home");

            }

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }
    }

    public async System.Threading.Tasks.Task<IActionResult> ConsultarMetodosDonacion(){
        try{

            if(Cliente.Usuario!.Rol!.IdRol == idRefugio){

                if(Cliente.Usuario.ListaDonaciones != null){

                    return View(Cliente.Usuario.ListaDonaciones);

                }

                var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
                cliente?.DefaultRequestHeaders.Clear();
                cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var mensaje = await cliente?.GetAsync("api/usuarios/" + Cliente.Usuario.IdUsuario + "/donaciones")!;
                List<EnlaceDonacionDTO>? listaMetodos = new List<EnlaceDonacionDTO>();

                if(mensaje.StatusCode == System.Net.HttpStatusCode.NoContent){

                    cliente?.DefaultRequestHeaders.Clear();
                    cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    mensaje = await cliente?.GetAsync("api/metodosDonacion/")!;

                    if(mensaje.StatusCode == System.Net.HttpStatusCode.NoContent){

                        return RedirectToAction("ErrorPagina", "Home");

                    }else if(mensaje.IsSuccessStatusCode){

                        string listaMetodosString = mensaje.Content.ReadAsStringAsync().Result;
                        List<MetodoDonacionDTO>? listaMetodosObtenidos = JsonConvert.DeserializeObject<List<MetodoDonacionDTO>>(listaMetodosString);

                        for(int i = 0; i < listaMetodosObtenidos!.Count; i ++){

                            EnlaceDonacionDTO enlace = new EnlaceDonacionDTO();
                            enlace.Metodo = listaMetodosObtenidos[i].Metodo;
                            enlace.IdUsuario = Cliente.Usuario.IdUsuario;
                            enlace.IdMetodoDonacion = listaMetodosObtenidos[i].IdMetodoDonacion;
                            listaMetodos.Add(enlace);
                            
                        }

                        Cliente.Usuario!.ListaDonaciones = listaMetodos;
                        return View(listaMetodos);

                    }

                }else if(mensaje.IsSuccessStatusCode){
                    
                    string listaMetodosString = mensaje.Content.ReadAsStringAsync().Result;
                    listaMetodos = JsonConvert.DeserializeObject<List<EnlaceDonacionDTO>>(listaMetodosString);
                    Cliente.Usuario.ListaDonaciones = listaMetodos;
                    return View(listaMetodos);

                }
                
                return RedirectToAction("ErrorPagina", "Home");

            }

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }
    }

    public IActionResult EditarCuenta(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idAnimalista || Cliente.Usuario!.Rol!.IdRol == idRefugio){

                UsuarioConverter usuarioConverter = new UsuarioConverter();
                UsuarioAuxiliar usuarioAuxiliar = usuarioConverter.convertirDesdeUsuarioDTO(Cliente.Usuario);
                return View(usuarioAuxiliar);

            }else{

                return RedirectToAction("PaginaNoEncontrada", "Home");

            }

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public IActionResult EditarEnlacesRedes(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idRefugio){

                return View(Cliente.Usuario.ListaRedes);

            }else{

                return RedirectToAction("PaginaNoEncontrada", "Home");

            }

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public IActionResult EditarMetodosDonacion(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idRefugio){

                return View(Cliente.Usuario.ListaDonaciones);

            }else{

                return RedirectToAction("PaginaNoEncontrada", "Home");

            }

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public async System.Threading.Tasks.Task<IActionResult> GuardarCuentaBtn(UsuarioAuxiliar usuarioAuxiliar){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idRefugio || Cliente.Usuario!.Rol!.IdRol == idAnimalista){

                UsuarioConverter usuarioConverter = new UsuarioConverter();
                UsuarioDTO usuario = usuarioConverter.convertirDesdeUsuario(usuarioAuxiliar);
                usuario.Contrasenia = Cliente.Usuario.Contrasenia;

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
                HttpResponseMessage mensaje = await cliente?.PutAsync("api/usuarios/" + Cliente.Usuario!.IdUsuario, data)!;
                
                if(mensaje.IsSuccessStatusCode){

                    string exito = mensaje.Content.ReadAsStringAsync().Result;

                    if(exito != null){

                        MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(exito);
                        usuario.IdUsuario = Cliente.Usuario.IdUsuario;
                        usuario.Rol = Cliente.Usuario.Rol;
                        usuario.ContadorReportes = Cliente.Usuario.ContadorReportes;
                        usuario.EstadoUsuario = Cliente.Usuario.EstadoUsuario;
                        usuario.ListaDonaciones = Cliente.Usuario.ListaDonaciones;
                        usuario.ListaRedes = Cliente.Usuario.ListaRedes;
                        Cliente.Usuario = usuario;
                        return RedirectToAction("ConsultarCuenta", "Cuenta");

                    }

                }else{

                    string noExito = mensaje.Content.ReadAsStringAsync().Result;

                    if(noExito != null){

                        MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(noExito);
                        return RedirectToAction("EditarCuenta", "Cuenta");

                    }

                }
                
                return RedirectToAction("ErrorPagina", "Home");

            }

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public async System.Threading.Tasks.Task<IActionResult> GuardarRedesBtn(string Facebook, string Instagram, 
    string Telegram, string TikTok, string Twitter, string YouTube){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idRefugio){

                List<EnlaceRedSocialDTO> listaRedes = Cliente.Usuario!.ListaRedes!;

                for(int i = 0; i < listaRedes.Count; i++){

                    if(listaRedes[i].Nombre == "Facebook"){

                        listaRedes[i].Enlace = Facebook;

                    }else if(listaRedes[i].Nombre == "Instagram"){

                        listaRedes[i].Enlace = Instagram;

                    }else if(listaRedes[i].Nombre == "Telegram"){

                        listaRedes[i].Enlace = Telegram;

                    }else if(listaRedes[i].Enlace == "TikTok"){

                        listaRedes[i].Enlace = TikTok;

                    }else if(listaRedes[i].Nombre == "Twitter"){

                        listaRedes[i].Enlace = Twitter;

                    }else if(listaRedes[i].Nombre == "YouTube"){

                        listaRedes[i].Nombre = YouTube;

                    }else{

                        return RedirectToAction("ErrorPagina", "Home");

                    }

                }

                DefaultContractResolver contractResolver = new DefaultContractResolver{

                    NamingStrategy = new CamelCaseNamingStrategy()

                };

                string json = JsonConvert.SerializeObject(listaRedes, new JsonSerializerSettings{

                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented

                });

                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
                cliente?.DefaultRequestHeaders.Clear();
                cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage mensaje = await cliente?.PutAsync("api/usuarios/" + Cliente.Usuario!.IdUsuario + "/redesSociales", data)!;

                if(mensaje.IsSuccessStatusCode){

                    string exito = mensaje.Content.ReadAsStringAsync().Result;

                    if(exito != null){

                        MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(exito);
                        Cliente.Usuario.ListaRedes = listaRedes;
                        return RedirectToAction("ConsultarRedesSociales", "Cuenta");

                    }

                }else{

                    string noExito = mensaje.Content.ReadAsStringAsync().Result;

                    if(noExito != null){

                        MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(noExito);
                        return RedirectToAction("EditarEnlacesRedes", "Cuenta");

                    }

                }

                return RedirectToAction("ErrorPagina", "Home");

            }

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public async System.Threading.Tasks.Task<IActionResult> GuardarMetodosBtn(string Donadora, string Otro, 
    string Paypal){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idRefugio){

                List<EnlaceDonacionDTO> listaMetodos = Cliente.Usuario!.ListaDonaciones!;

                for(int i = 0; i < listaMetodos.Count; i++){

                    if(listaMetodos[i].Metodo == "PayPal"){

                        listaMetodos[i].Enlace = Paypal;

                    }else if(listaMetodos[i].Metodo == "Donadora"){

                        listaMetodos[i].Enlace = Donadora;

                    }else if(listaMetodos[i].Metodo == "Otro"){

                        listaMetodos[i].Enlace = Otro;

                    }else{

                        return RedirectToAction("ErrorPagina", "Home");

                    }

                }

                DefaultContractResolver contractResolver = new DefaultContractResolver{

                    NamingStrategy = new CamelCaseNamingStrategy()

                };

                string json = JsonConvert.SerializeObject(listaMetodos, new JsonSerializerSettings{

                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented

                });

                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var cliente = _IhttpClientFactory?.CreateClient("BackEnd");
                cliente?.DefaultRequestHeaders.Clear();
                cliente?.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage mensaje = await cliente?.PutAsync("api/usuarios/" + Cliente.Usuario!.IdUsuario + "/donaciones", data)!;

                if(mensaje.IsSuccessStatusCode){

                    string exito = mensaje.Content.ReadAsStringAsync().Result;

                    if(exito != null){

                        MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(exito);
                        Cliente.Usuario.ListaDonaciones = listaMetodos;
                        return RedirectToAction("ConsultarMetodosDonacion", "Cuenta");

                    }

                }else{

                    string noExito = mensaje.Content.ReadAsStringAsync().Result;

                    if(noExito != null){

                        MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(noExito);
                        return RedirectToAction("EditarMetodosDonacion", "Cuenta");

                    }

                }

                return RedirectToAction("ErrorPagina", "Home");

            }

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public IActionResult CambiarContrasenia(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idAnimalista || Cliente.Usuario!.Rol!.IdRol == idRefugio){

                return View();

            }else{

                return RedirectToAction("PaginaNoEncontrada", "Home");

            }

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public async System.Threading.Tasks.Task<IActionResult> CambiarContraseniaBtn(String contrasenia, String contraseniaRepetida){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idAnimalista || Cliente.Usuario!.Rol!.IdRol == idAnimalista){

                UsuarioDTO usuario = Cliente.Usuario!;
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
                        Cliente.Usuario!.Contrasenia = contrasenia;
                        return RedirectToAction("ConsultarCuenta", "Cuenta");

                    }

                }else{

                    string noExito = mensaje.Content.ReadAsStringAsync().Result;

                    if(noExito != null){

                        MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(noExito);
                        return RedirectToAction("CambiarContrasenia", "Cuenta");

                    }

                }

                return RedirectToAction("ErrorPagina", "Home");

            }

            return RedirectToAction("PaginaNoEncontrada", "Home");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public IActionResult EliminarCuenta(){

        if(Cliente.Usuario!.Rol!.IdRol == idAnimalista || Cliente.Usuario.Rol.IdRol == idRefugio){

            return View();

        }else{

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public async System.Threading.Tasks.Task<IActionResult> EliminarCuentaBtn(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idAnimalista || Cliente.Usuario!.Rol!.IdRol == idRefugio){

                UsuarioDTO usuario = new UsuarioDTO();
                usuario.EstadoUsuario = "Eliminado";
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
                HttpResponseMessage mensaje = await cliente?.PutAsync("api/usuarios/" + Cliente.Usuario.IdUsuario + "/cuentas", data)!;

                if(mensaje.IsSuccessStatusCode){

                    string exito = mensaje.Content.ReadAsStringAsync().Result;

                    if(exito != null){

                        MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(exito);
                        Cliente.Usuario = null;
                        return RedirectToAction("Index", "Home");

                    }

                }else{

                    string noExito = mensaje.Content.ReadAsStringAsync().Result;

                    if(noExito != null){

                        MensajeResultante? mensajeResultante = JsonConvert.DeserializeObject<MensajeResultante>(noExito);
                        return RedirectToAction("EliminarCuenta", "Cuenta");

                    }

                }

                return RedirectToAction("PaginaNoEncontrada", "Home");

            }

            return RedirectToAction("ErrorPagina", "Home");

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

}