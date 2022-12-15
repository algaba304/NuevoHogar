using NuevoHogar.Models;

namespace NuevoHogar.ModeloAuxiliar{

    public class UsuarioAuxiliar{

        public string? IdUsuario { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Nombre { get; set; }
        public string? NumeroTelefono { get; set; }
        public string? Direccion { get; set; }
        public string? Contrasenia { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string? Biografia { get; set; }
        public string? FotoPerfilUsuario { get; set; }
        public string? IdRol { get; set; }
        public string? NombreUsuario { get; set; }
        public int ContadorReportes { get; set; }
        public string? EstadoUsuario { get; set; }
        public List<Rol>? Roles { get; set; }
        public string? RolSeleccionado { get; set; }

    }

}