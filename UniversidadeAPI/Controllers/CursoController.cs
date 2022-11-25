using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversidadeApi.Models;

namespace UniversidadeAPI.Controllers{
    [Route("api/curso")]
    [ApiController]
    public class CursoController : ControllerBase{
        private readonly UniversidadeContext _context;

        public CursoController(UniversidadeContext context){
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCurso(){
            if(_context.cursos == null)
                return NotFound();

            return await _context.cursos.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Curso>> GetCursoById(long id){
            var curso = await _context.cursos.Where(x => x.Id == id).FirstAsync();

            if(curso == null)
                return NotFound();

            return curso;
        }

        [HttpGet("{sigla}")]
        public async Task<ActionResult<Curso>> GetCursoBySigla(string sigla){
            Curso curso = await _context.cursos.Where(x => x.Sigla.Equals(sigla)).FirstAsync();

            if(curso == null)
                return NotFound();

            return curso;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurso(long id, Curso curso){
            if(id != curso.Id)
                return BadRequest();

            var c = await _context.cursos.FindAsync(id);

            c.Sigla = curso.Sigla;
            c.Nome = curso.Nome;

            try{
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException) when (!CursoExists(id)){
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Curso>> CreateCurso(Curso curso) {
            if (_context.cursos == null)
                return Problem("Entity set 'UniversidadeContext.cursos'  is null.");

            _context.cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurso", new { id = curso.Id }, curso);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(long id){
            if (_context.cursos == null)
                return NotFound();

            var curso = await _context.cursos.FindAsync(id);
            if (curso == null)
                return NotFound();

            _context.cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursoExists(long id){
            return (_context.cursos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
