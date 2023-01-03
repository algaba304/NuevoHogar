using System;
using System.ComponentModel.DataAnnotations;

namespace NuevoHogar.ModeloDTO{
    public class ReporteDTO{
        public String idReporteDeMaltrato {get; set;}
        public String ubicacion {get; set;}
        public String descripcion {get; set;}
        public String fecha {get; set;}
        public String idRefugio {get; set;}
        public String idUsuario {get; set;}
    }
}