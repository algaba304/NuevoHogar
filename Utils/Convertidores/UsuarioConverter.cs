using System.Globalization;
using NuevoHogar.ModeloAuxiliar;
using NuevoHogar.ModeloDTO;

namespace NuevoHogar.Utils.Convertidores{

    public class UsuarioConverter{

        public UsuarioAuxiliar convertirDesdeUsuarioDTO(UsuarioDTO usuario){

            UsuarioAuxiliar usuarioAuxiliar = new UsuarioAuxiliar();
            usuarioAuxiliar.Nombre = Cliente.Usuario!.Nombre;
            usuarioAuxiliar.NombreUsuario = Cliente.Usuario.NombreUsuario;
            usuarioAuxiliar.CorreoElectronico = Cliente.Usuario.CorreoElectronico;
            usuarioAuxiliar.NumeroTelefono = Cliente.Usuario.NumeroTelefono;
            usuarioAuxiliar.Direccion = Cliente.Usuario.Direccion;
            string fecha = DateTime.Parse(Cliente.Usuario.FechaNacimiento!).ToShortDateString();
            usuarioAuxiliar.FechaNacimiento = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            usuarioAuxiliar.FotoPerfilUsuario = Cliente.Usuario.FotoPerfilUsuario;
            usuarioAuxiliar.Biografia = Cliente.Usuario.Biografia;
            return usuarioAuxiliar;

        }

        public UsuarioDTO convertirDesdeUsuario(UsuarioAuxiliar usuarioAuxiliar){

            UsuarioDTO usuario = new UsuarioDTO();
            usuario.Nombre = usuarioAuxiliar.Nombre;
            usuario.NombreUsuario = usuarioAuxiliar.NombreUsuario;
            usuario.CorreoElectronico = usuarioAuxiliar.CorreoElectronico;
            usuario.Contrasenia = usuarioAuxiliar.Contrasenia;
            usuario.NumeroTelefono = usuarioAuxiliar.NumeroTelefono;
            usuario.Direccion = usuarioAuxiliar.Direccion;
            usuario.FechaNacimiento = usuarioAuxiliar.FechaNacimiento.ToString("yyyy-MM-dd");
            usuario.Biografia = usuarioAuxiliar.Biografia;
            usuario.FotoPerfilUsuario = usuarioAuxiliar.FotoPerfilUsuario;
            return usuario;

        }
    }
}