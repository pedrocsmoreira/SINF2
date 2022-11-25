using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using UniversidadeApi.Models;

namespace UniversidadeApi.Models{
    public class UniversidadeContext : DbContext{
        public UniversidadeContext(DbContextOptions<UniversidadeContext> options) : base(options){}

        public DbSet<Aluno>? alunos { get; set; }
        public DbSet<Curso>? cursos { get; set; }
        public DbSet<Nota>? notas { get; set; }
        public DbSet<UnidadeCurricular>? unidadesCurriculares { get; set; }
    }
}