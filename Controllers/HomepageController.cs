using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NuevoHogar.Models;

namespace NuevoHogar.Controllers;

public class HomepageController : Controller{

    private String baseURL = "http://127.0.0.1:8080/";
    public IActionResult MostrarListaReportes(){
        return View();
    }
}