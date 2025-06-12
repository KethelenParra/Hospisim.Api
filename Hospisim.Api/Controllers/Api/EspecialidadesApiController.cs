using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;
using Hospisim.Api.Models.Dtos.Especialidade;
using Hospisim.Api.Models.Dtos.Profissionais;

namespace Hospisim.Api.Controllers.Api
{
    [ApiController]
    [Route("api/especialidades")]
    public class EspecialidadesApiController : ControllerBase
    {
        private readonly HospisimDbContext _context;

        public EspecialidadesApiController(HospisimDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém uma lista de todas as especialidades.
        /// </summary>
        /// <response code="200">Retorna lista de Especialidades</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var especialidades = await _context.Especialidades
                .AsNoTracking()
                .OrderBy(e => e.Nome)
                .ToListAsync();

            // Mapeia para o DTO simples (sem a lista de profissionais)
            var especialidadesDto = especialidades.Select(e => new EspecialidadeDto
            {
                Id = e.Id,
                Nome = e.Nome
            });

            return Ok(especialidadesDto);
        }

        /// <summary>
        /// Obtém os detalhes de uma especialidade, incluindo os profissionais associados.
        /// </summary>
        /// <response code="200">Retorna o especialidade</response>
        /// <response code="404">Especialidade não encontrado</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var especialidade = await _context.Especialidades
                .Include(e => e.Profissionais) // Inclui a lista de profissionais
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (especialidade == null) return NotFound();

            // Mapeia para o DTO de detalhes
            var dto = new EspecialidadeDetalhesDto
            {
                Id = especialidade.Id,
                Nome = especialidade.Nome,
                QuantidadeProfissionais = especialidade.Profissionais.Count,
                Profissionais = especialidade.Profissionais.Select(p => new ProfissionalResumoDto
                {
                    Id = p.Id,
                    NomeCompleto = p.NomeCompleto,
                    RegistroConselho = p.RegistroConselho
                }).ToList()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Cadastra uma nova especialidade.
        /// </summary>
        /// <response code="201">Especialidade criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateUpdateEspecialidadeDto dto)
        {
            var especialidade = new Especialidade
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome
            };

            await _context.Especialidades.AddAsync(especialidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = especialidade.Id }, especialidade);
        }

        /// <summary>
        /// Atualiza o nome de uma especialidade existente.
        /// </summary>
        /// <response code="204">Atualização sucedida</response>
        /// <response code="404">Especialidade não encontrado</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] CreateUpdateEspecialidadeDto dto)
        {
            var especialidade = await _context.Especialidades.FindAsync(id);

            if (especialidade == null) return NotFound();

            especialidade.Nome = dto.Nome;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Exclui uma especialidade. A exclusão só é permitida se não houver profissionais associados.
        /// </summary>
        /// <response code="204">Exclusão sucedida</response>
        /// <response code="404">Especialidade não encontrado</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var especialidade = await _context.Especialidades
                .Include(e => e.Profissionais)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (especialidade == null) return NotFound();

            // REGRA DE NEGÓCIO: Não permitir excluir especialidade com profissionais.
            if (especialidade.Profissionais.Any())
            {
                return BadRequest(new { message = "Não é possível excluir uma especialidade que possui profissionais associados." });
            }

            _context.Especialidades.Remove(especialidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}