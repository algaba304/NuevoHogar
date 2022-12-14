using System.ComponentModel.DataAnnotations;
using NuevoHogar.ModeloDTO;

namespace NuevoHogar.ModeloAuxiliar{

    public class UsuarioAuxiliar{

        [Required]
        [MaxLength(255)]
        public string? IdUsuario { get; set; }
        [Required]
        [MaxLength(255)]
        public string? CorreoElectronico { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Nombre { get; set; }
        [Required]
        [MaxLength(20)]
        public string? NumeroTelefono { get; set; }
        [MaxLength(100)]
        public string? Direccion { get; set; }
        [Required]
        [MaxLength(16)]
        [MinLength(8)]
        public string? Contrasenia { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [MaxLength(100)]
        public string? Biografia { get; set; }
        [MaxLength(500)]
        public string? FotoPerfilUsuario { get; set; }
        [Required]
        [MaxLength(50)]
        public string? NombreUsuario { get; set; }
        public int ContadorReportes { get; set; }
        [Required]
        [MaxLength(10)]
        public string? EstadoUsuario { get; set; }
        [Required]
        public List<RolDTO>? Roles { get; set; }
        [Required]
        public string? RolSeleccionado { get; set; }

    }

}