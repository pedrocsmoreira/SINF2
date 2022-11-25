using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversidadeApi.Models;

namespace UniversidadeAPI.Controllers{
    [Route("api/nota")]
    [ApiController]
    public class NotaController : ControllerBase{
        private readonly UniversidadeContext _context;

        public NotaController(UniversidadeContext context){
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotaDTO>>> GetNota(){
            return await _context.notas.Include(x => x.Aluno).Include(x => x.UC).Select(x => NotaToDTO(x)).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<NotaDTO>> GetNotaById(long id){
            if(_context.notas == null)
                return NotFound();

            var nota = await _context.notas.Include(x => x.Aluno).Include(x => x.UC).Where(x => x.Id == id).FirstAsync();

            if (nota == null)
                return NotFound();

            return NotaToDTO(nota);
        }

        [HttpGet("{sigla}")]
        public async Task<ActionResult<IEnumerable<NotaDTO>>> GetNotaBySigla(string sigla){
            if(_context.notas == null)
                return NotFound();

            var uc = await _context.unidadesCurriculares.Where(x => x.Sigla.Equals(sigla)).FirstAsync();
            if(uc == null)
                return NotFound();

            var nota = await _context.notas.Include(x => x.Aluno).Include(x => x.UC).Where(x => x.UC.Equals(uc)).Select(x => NotaToDTO(x)).ToListAsync();

            if (nota == null)
                return NotFound();

            return nota;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNota(long id, NotaDTO notaDTO) {
            if (id != notaDTO.Id)
                return BadRequest();

            var nota = await _context.notas.FindAsync(id);
            if(nota == null)
                return NotFound();


            var aluno = await _context.alunos.Where(x => x.Nome.Equals(notaDTO.nomeAluno)).FirstAsync();
            if(aluno == null)
                return NotFound();

            var uc = await _context.unidadesCurriculares.Where(x => x.Sigla.Equals(notaDTO.siglaUC)).FirstAsync();
            if(uc == null)
                return NotFound();

            nota.Valor = notaDTO.Valor;
            nota.Aluno = aluno;
            nota.UC = uc;

            try{
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException) when (!NotaExists(id)) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Nota>> CreateNota(NotaDTO notaDTO){
            if (_context.notas == null)
                return Problem("Entity set 'UniversidadeContext.Nota'  is null.");

            var aluno = await _context.alunos.Where(x => x.Nome.Equals(notaDTO.nomeAluno)).FirstAsync();
            if(aluno == null)
                return NotFound();

            var uc = await _context.unidadesCurriculares.Where(x => x.Sigla.Equals(notaDTO.siglaUC)).FirstAsync();
            if(uc == null)
                return NotFound();

            var nota = new Nota{
                Valor = notaDTO.Valor,
                Aluno = aluno,
                UC = uc
            };

            _context.notas.Add(nota);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNota", new { id = nota.Id }, NotaToDTO(nota));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNota(long id){
            if(_context.notas == null)
                return NotFound();

            var nota = await _context.notas.FindAsync(id);
            if (nota == null)
                return NotFound();

            _context.notas.Remove(nota);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotaExists(long id){
            return (_context.notas?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static NotaDTO NotaToDTO(Nota nota){
            return new NotaDTO{
                Id = nota.Id,
                Valor = nota.Valor,
                nomeAluno = nota.Aluno.Nome,
                siglaUC = nota.UC.Sigla
            };
        }
    }
}
