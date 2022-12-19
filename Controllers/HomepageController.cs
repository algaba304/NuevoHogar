using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.Utils;

namespace NuevoHogar.Controllers;

public class HomepageController : Controller{

    private String idAnimalista = "AN_123_R";
    private String idRefugio = "RF_123_R";
    private String idAdministrador = "AD_123_R";

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

    public IActionResult HomepageRefugio(){

        if(Cliente.Usuario!.Rol!.IdRol == idRefugio){

            return View();

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