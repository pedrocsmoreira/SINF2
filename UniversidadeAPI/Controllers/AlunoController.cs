using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversidadeApi.Models;

namespace UniversidadeAPI.Controllers{
    [Route("api/aluno")]
    [ApiController]
    public class AlunoController : ControllerBase{
        private readonly UniversidadeContext _context;

        public AlunoController(UniversidadeContext context){
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunos(){
            return await _context.alunos.Include(x => x.Curso).Select(x => AlunoToDTO(x)).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AlunoDTO>> GetAluno(long id){
            var aluno = await _context.alunos.Include(x => x.Curso).Where(x => x.Id==id).FirstAsync();

            if (aluno == null)
                return NotFound();

            return AlunoToDTO(aluno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAluno(long id, AlunoDTO alunoDTO) {
            if (id != alunoDTO.Id)
                return BadRequest();

            var aluno = await _context.alunos.FindAsync(id);

            if(aluno == null)
                return NotFound();

            var curso = await _context.cursos.Where(x => x.Sigla.Equals(alunoDTO.siglaCurso)).FirstAsync();
            if(curso == null)
                return NotFound();

            aluno.Nome = alunoDTO.Nome;
            aluno.Email = alunoDTO.Email;
            aluno.Saldo = alunoDTO.Saldo;
            aluno.Curso = curso;

            try{
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException) when (!AlunoExists(id)){
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("saldo")]
        public async Task<IActionResult> UpdateSaldoAluno(long id, long custo) {
            var aluno = await _context.alunos.FindAsync(id);


            if(aluno == null)
                return NotFound();

            aluno.Saldo = aluno.Saldo - custo;

            try{
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException) when (!AlunoExists(id)){
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Aluno>> CreateAluno(AlunoDTO alunoDTO){
            var curso = await _context.cursos.Where(x => x.Sigla.Equals(alunoDTO.siglaCurso)).FirstAsync();
            
            if(curso == null)
                return NotFound();
            
            var aluno = new Aluno{
                Nome = alunoDTO.Nome,
                Email = alunoDTO.Email,
                Saldo = alunoDTO.Saldo,
                Curso = curso
            };
            
            _context.alunos.Add(aluno);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAluno", new { id = aluno.Id }, AlunoToDTO(aluno));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluno(long id){
            if (_context.alunos == null)
                return NotFound();
                

            var aluno = await _context.alunos.FindAsync(id);
            if (aluno == null)
                return NotFound();

            _context.alunos.Remove(aluno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlunoExists(long id){
            return (_context.alunos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        private static AlunoDTO AlunoToDTO(Aluno aluno){
            return new AlunoDTO{
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Saldo = aluno.Saldo,
                siglaCurso = aluno.Curso.Sigla
            };
        }
    }
}
