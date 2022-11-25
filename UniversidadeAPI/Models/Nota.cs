namespace UniversidadeApi.Models{
    public class Nota{
        public long Id { get; set; }
        public long Valor { get; set; }
        public Aluno Aluno { get; set; }
        public UnidadeCurricular UC { get; set; }
    }
}