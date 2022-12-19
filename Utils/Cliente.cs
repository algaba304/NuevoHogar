using NuevoHogar.ModeloDTO;

namespace NuevoHogar.Utils{
    public class Cliente{
        public static UsuarioDTO? Usuario { get; set; }
        private static Cliente instancia = new Cliente();
        public static Cliente getInstancia() {
            return instancia;
        }
        private Cliente(){}
    }
}