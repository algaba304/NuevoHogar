using System;
using System.ComponentModel.DataAnnotations;

namespace NuevoHogar.ModeloDTO{

    public class RolDTO{

        [MaxLength(255)]
        public string? IdRol { get; set; }
        [MaxLength(10)]
        public string? Nombre { get; set; }
    }
}