using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuevoHogar.ModeloAuxiliar;
using NuevoHogar.Utils;

namespace NuevoHogar.Controllers;

public class HomepageController : Controller{

    private readonly IHttpClientFactory? _IhttpClientFactory;
    public HomepageController(IHttpClientFactory httpClientFactory){

        _IhttpClientFactory = httpClientFactory;

    }

    private String idAnimalista = "AN_123_R";
    private String idRefugio = "RF_123_R";
    private String idAdministrador = "AD_123_R";

    public IActionResult Homepage(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idAnimalista){
            
                return RedirectToAction("HomepageAnimalista", "Homepage");

            }else if(Cliente.Usuario!.Rol!.IdRol == idRefugio){

                return RedirectToAction("HomepageRefugio", "Homepage");
                
            }else if(Cliente.Usuario!.Rol!.IdRol == idAdministrador){

                return RedirectToAction("HomepageAdministrador", "Homepage");

            }else{

                return RedirectToAction("PaginaNoEncontrada", "Home");

            }

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public IActionResult HomepageAnimalista(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idAnimalista){
            
                return View();

            }else{

                return RedirectToAction("PaginaNoEncontrada", "Home");

            }

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }
        
    }

    public IActionResult HomepageRefugio(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idRefugio){

                return View();

            }else{

                return RedirectToAction("PaginaNoEncontrada", "Home");

            }

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public IActionResult HomepageAdministrador(){

        try{

            if(Cliente.Usuario!.Rol!.IdRol == idAdministrador){

                return View();

            }else{

                return RedirectToAction("PaginaNoEncontrada", "Home");

            }

        }catch(Exception ex){

            return RedirectToAction("ErrorPagina", "Home");

        }

    }
}