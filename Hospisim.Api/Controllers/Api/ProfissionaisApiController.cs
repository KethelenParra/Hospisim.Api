using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;
using Hospisim.Api.Extensions;
using Hospisim.Api.Models.Dtos.Especialidade;
using Hospisim.Api.Models.Dtos.Profissionais;

namespace Hospisim.Api.Controllers.Api
{
    [ApiController]
    [Route("api/profissionais")]
    public class ProfissionaisApiController : ControllerBase
    {
        private readonly HospisimDbContext _context;

        public ProfissionaisApiController(HospisimDbContext context)
            => _context = context;

        /// <summary>
        /// Obtém todos os profissionais de saúde cadastrados (com especialidade).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var profissionais = await _context.Profissionais
                .Include(p => p.Especialidade) // Inclui a especialidade para o mapeamento
                .AsNoTracking()
                .ToListAsync();

            // 3. Mapeia a lista de entidades para uma lista de DTOs de leitura
            var profissionaisDto = profissionais.Select(p => new ProfissionalDto
            {
                Id = p.Id,
                NomeCompleto = p.NomeCompleto,
                CpfFormatado = p.CPF.ToCPFFormat(), // Usa o método de extensão
                Email = p.Email,
                Telefone = p.Telefone,
                RegistroConselho = p.RegistroConselho,
                TipoRegistro = p.TipoRegistro.GetDisplayName(), // Usa o método de extensão
                Ativo = p.Ativo,
                CargaHorariaSemanal = p.CargaHorariaSemanal,
                Turno = p.Turno.GetDisplayName(),
                DataAdmissao = p.DataAdmissao,
                Especialidade = new EspecialidadeDto // Mapeia a especialidade aninhada
                {
                    Id = p.Especialidade.Id,
                    Nome = p.Especialidade.Nome
                }
            });

            return Ok(profissionaisDto);
        }


        /// <summary>
        /// Obtém um profissional pelo Id (com especialidade).
        /// </summary>
        /// <response code="200">Retorna o profissional</response>
        /// <response code="404">Profissional não encontrado</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var profissional = await _context.Profissionais
                .Include(p => p.Especialidade)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profissional == null) return NotFound();

            // 4. Mapeia a entidade encontrada para o DTO de leitura
            var profissionalDto = new ProfissionalDto
            {
                Id = profissional.Id,
                NomeCompleto = profissional.NomeCompleto,
                CpfFormatado = profissional.CPF.ToCPFFormat(),
                Email = profissional.Email,
                Telefone = profissional.Telefone,
                RegistroConselho = profissional.RegistroConselho,
                TipoRegistro = profissional.TipoRegistro.GetDisplayName(),
                Ativo = profissional.Ativo,
                CargaHorariaSemanal = profissional.CargaHorariaSemanal,
                Turno = profissional.Turno.GetDisplayName(),
                DataAdmissao = profissional.DataAdmissao,
                Especialidade = new EspecialidadeDto
                {
                    Id = profissional.Especialidade.Id,
                    Nome = profissional.Especialidade.Nome
                }
            };

            return Ok(profissionalDto);
        }

        /// <summary>
        /// Cadastra um novo profissional de saúde.
        /// </summary>
        /// <response code="201">Profissional criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateProfissionalDto dto)
        {
            // O mapeamento do DTO para a entidade continua o mesmo
            var profissional = new ProfissionalSaude
            {
                Id = Guid.NewGuid(),
                NomeCompleto = dto.NomeCompleto,
                CPF = dto.CPF,
                Email = dto.Email,
                Telefone = dto.Telefone,
                RegistroConselho = dto.RegistroConselho,
                TipoRegistro = dto.TipoRegistro,
                EspecialidadeId = dto.EspecialidadeId,
                DataAdmissao = dto.DataAdmissao,
                CargaHorariaSemanal = dto.CargaHorariaSemanal,
                Turno = dto.Turno,
                Ativo = true
            };

            await _context.Profissionais.AddAsync(profissional);
            await _context.SaveChangesAsync();

            var profissionalCriado = await _context.Profissionais
                .Include(p => p.Especialidade)
                .AsNoTracking()
                .FirstAsync(p => p.Id == profissional.Id);

            var profissionalDto = new ProfissionalDto
            {
                Id = profissionalCriado.Id,
                NomeCompleto = profissionalCriado.NomeCompleto,
                CpfFormatado = profissionalCriado.CPF.ToCPFFormat(),
                Email = profissionalCriado.Email,
                Telefone = profissionalCriado.Telefone,
                RegistroConselho = profissionalCriado.RegistroConselho,
                TipoRegistro = profissionalCriado.TipoRegistro.GetDisplayName(),
                Ativo = profissionalCriado.Ativo,
                CargaHorariaSemanal = profissionalCriado.CargaHorariaSemanal,
                Turno = profissionalCriado.Turno.GetDisplayName(),
                DataAdmissao = profissionalCriado.DataAdmissao,
                Especialidade = new EspecialidadeDto
                {
                    Id = profissionalCriado.Especialidade.Id,
                    Nome = profissionalCriado.Especialidade.Nome
                }
            };

            return CreatedAtAction(nameof(GetById), new { id = profissional.Id }, profissionalDto);
        }


        /// <summary>
        /// Atualiza os dados de um profissional de saúde.
        /// </summary>
        /// <response code="204">Atualização sucedida</response>
        /// <response code="404">Profissional não encontrado</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateProfissionalDto dto)
        {
            var profissional = await _context.Profissionais.FindAsync(id);
            if (profissional == null) return NotFound();

            // 6. Mapeia os dados do DTO para a entidade que veio do banco
            profissional.NomeCompleto = dto.NomeCompleto;
            profissional.Email = dto.Email;
            profissional.Telefone = dto.Telefone;
            profissional.RegistroConselho = dto.RegistroConselho;
            profissional.EspecialidadeId = dto.EspecialidadeId;
            profissional.CargaHorariaSemanal = dto.CargaHorariaSemanal;
            profissional.Turno = dto.Turno;
            profissional.Ativo = dto.Ativo;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Exclui um profissional de saúde.
        /// </summary>
        /// <response code="204">Exclusão sucedida</response>
        /// <response code="404">Profissional não encontrado</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var prof = await _context.Profissionais.FindAsync(id);
            if (prof == null) return NotFound();

            _context.Profissionais.Remove(prof);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
