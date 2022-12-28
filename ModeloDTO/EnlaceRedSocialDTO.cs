using System;
using System.ComponentModel.DataAnnotations;

namespace NuevoHogar.ModeloDTO{

    public class EnlaceRedSocialDTO{

        [Required]
        [MaxLength(255)]
        public string? IdEnlaceRedSocial { get; set; }
        [MaxLength(500)]
        public string? Enlace { get; set; }
        [Required]
        [MaxLength(255)]
        public string? IdUsuario { get; set; }
        [Required]
        [MaxLength(255)]
        public string? IdTipoRedSocial { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Nombre { get; set; }

    }

}