namespace UniversidadeApi.Models{
    public class AlunoDTO{
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public long Saldo { get; set; }
        public string siglaCurso { get; set; }
    }
}