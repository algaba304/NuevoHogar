using System;
using System.ComponentModel.DataAnnotations;

namespace NuevoHogar.ModeloDTO{
    public class Reporte2DTO{
        public String idReporteDeMaltrato {get; set;}
        public String ubicacion {get; set;}
        public String descripcion {get; set;}
        public String fecha {get; set;}
        public String idRefugio {get; set;}
        public String idUsuario {get; set;}
        public String nombreUsuario {get; set;}
    }
}