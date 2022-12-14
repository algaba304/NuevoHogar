namespace NuevoHogar.Models{
    public class Cliente{
        public static Usuario? Usuario { get; set; }
        private static Cliente instancia = new Cliente();
        public static Cliente getInstancia() {
            return instancia;
        }
        private Cliente(){}
    }
}