using System;
using System.ComponentModel.DataAnnotations;

namespace NuevoHogar.ModeloDTO{

    public class EnlaceDonacionDTO{

        [Required]
        [MaxLength(255)]
        public string? IdEnlaceDonacion { get; set; }
        [MaxLength(500)]
        public string? Enlace { get; set; }
        [Required]
        [MaxLength(255)]
        public string? IdUsuario { get; set; }
        [Required]
        [MaxLength(255)]
        public string? IdMetodoDonacion { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Metodo { get; set; }

    }

}