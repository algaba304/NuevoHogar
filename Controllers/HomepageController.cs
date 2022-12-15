using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.Models;

namespace NuevoHogar.Controllers;

public class HomepageController : Controller{

    public IActionResult HomepageAnimalista(){

        if(Cliente.Usuario!.Rol!.IdRol == "AN_123_R"){

            return View();

        }else{

            return RedirectToAction("ErrorPagina", "Home");

        }
        
    }

    public IActionResult HomepageRefugio(){

        if(Cliente.Usuario!.Rol!.IdRol == "RF_123_R"){

            return View();

        }else{

            return RedirectToAction("ErrorPagina", "Home");

        }

    }

    public IActionResult HomepageAdministrador(){

        if(Cliente.Usuario!.Rol!.IdRol == "AS_123_R"){

            return View();

        }else{

            return RedirectToAction("ErrorPagina", "Home");

        }

    }
}