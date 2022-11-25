using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversidadeApi.Models;

namespace UniversidadeAPI.Controllers {
    [Route("api/uc")]
    [ApiController]
    public class UnidadeCurricularController : ControllerBase {
        private readonly UniversidadeContext _context;

        public UnidadeCurricularController(UniversidadeContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnidadeCurricularDTO>>> GetUnidadeCurricular(){
            if(_context.unidadesCurriculares == null)
                return NotFound();

            return await _context.unidadesCurriculares.Include(x => x.Curso).Select(x => UnidadeCurricularToDTO(x)).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UnidadeCurricularDTO>> GetUnidadeCurricularById(long id){
            if(_context.unidadesCurriculares == null)
                return NotFound();

            var unidadeCurricular = await _context.unidadesCurriculares.Include(x => x.Curso).Where(x => x.Id == id).FirstAsync();

            if (unidadeCurricular == null)
                return NotFound();

            return UnidadeCurricularToDTO(unidadeCurricular);
        }

        [HttpGet("{sigla}")]
        public async Task<ActionResult<UnidadeCurricularDTO>> GetUnidadeCurricularBySigla(string sigla){
            if(_context.unidadesCurriculares == null)
                return NotFound();

            var unidadeCurricular = await _context.unidadesCurriculares.Include(x => x.Curso).Where(x => x.Sigla.Equals(sigla)).FirstAsync();
            if (unidadeCurricular == null)
                return NotFound();

            return UnidadeCurricularToDTO(unidadeCurricular);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnidadeCurricular(long id, UnidadeCurricularDTO unidadeCurricularDTO){
            if (id != unidadeCurricularDTO.Id)
                return BadRequest();

            var uc = await _context.unidadesCurriculares.FindAsync(id);

            var curso = await _context.cursos.Where(x => x.Sigla.Equals(unidadeCurricularDTO.siglaCurso)).FirstAsync();
            if(curso == null)
                return NotFound();

            uc.Sigla = unidadeCurricularDTO.Sigla;
            uc.Nome = unidadeCurricularDTO.Nome;
            uc.Curso = curso;
            uc.Ano = unidadeCurricularDTO.Ano;

            try{
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException) when (!UnidadeCurricularExists(id)){
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<UnidadeCurricular>> CreateUnidadeCurricular(UnidadeCurricularDTO unidadeCurricularDTO){
            if(_context.unidadesCurriculares == null){
                return Problem("Entity set 'UniversidadeContext.UnidadeCurricular'  is null.");
            }

            var curso = await _context.cursos.Where(x => x.Sigla.Equals(unidadeCurricularDTO.siglaCurso)).FirstAsync();
            if(curso == null)
                return NotFound();
            
            var uc = new UnidadeCurricular{
                Sigla = unidadeCurricularDTO.Sigla,
                Nome = unidadeCurricularDTO.Nome,
                Curso = curso,
                Ano = unidadeCurricularDTO.Ano
            };

            _context.unidadesCurriculares.Add(uc);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnidadeCurricular", new { id = uc.Id }, UnidadeCurricularToDTO(uc));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidadeCurricular(long id){
            if (_context.unidadesCurriculares == null)
                return NotFound();

            var unidadeCurricular = await _context.unidadesCurriculares.FindAsync(id);
            if (unidadeCurricular == null)
                return NotFound();

            _context.unidadesCurriculares.Remove(unidadeCurricular);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnidadeCurricularExists(long id){
            return (_context.unidadesCurriculares?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static UnidadeCurricularDTO UnidadeCurricularToDTO(UnidadeCurricular uc){
            return new UnidadeCurricularDTO{
                Id = uc.Id,
                Sigla = uc.Sigla,
                Nome = uc.Nome,
                siglaCurso = uc.Curso.Sigla,
                Ano = uc.Ano
            };
        }
    }
}
