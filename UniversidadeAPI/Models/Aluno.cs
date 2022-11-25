namespace UniversidadeApi.Models{
    public class Aluno{
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public long Saldo { get; set; } = 100;
        public Curso Curso { get; set; }
    }
}
