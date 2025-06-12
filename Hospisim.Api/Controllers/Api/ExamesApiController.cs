using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;
using Hospisim.Api.Dtos.Exame;

namespace Hospisim.Api.Controllers.Api
{
    [ApiController]
    [Route("api/exames")]
    public class ExamesApiController : ControllerBase
    {
        private readonly HospisimDbContext _context;

        public ExamesApiController(HospisimDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém uma lista de todos os exames.
        /// </summary>
        /// <response code="200">Retorna lista de exames</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var exames = await _context.Exames
                .Include(ex => ex.Atendimento.Paciente)      
                .Include(ex => ex.Atendimento.Profissional) 
                .AsNoTracking()
                .OrderByDescending(ex => ex.DataSolicitacao)
                .ToListAsync();

            // Mapeia para o DTO de leitura
            var examesDto = exames.Select(ex => new ExameDto
            {
                Id = ex.Id,
                Tipo = ex.Tipo,
                DataSolicitacao = ex.DataSolicitacao,
                DataRealizacao = ex.DataRealizacao,
                Resultado = ex.Resultado,
                AtendimentoId = ex.AtendimentoId,
                DataAtendimento = ex.Atendimento.DataHora,
                NomePaciente = ex.Atendimento.Paciente.NomeCompleto,
                NomeProfissional = ex.Atendimento.Profissional.NomeCompleto
            });

            return Ok(examesDto);
        }

        /// <summary>
        /// Obtém um exame específico pelo seu ID.
        /// </summary>
        /// <response code="200">Retorna o exame</response>
        /// <response code="404">Exame não encontrado</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var exame = await _context.Exames
                .Include(ex => ex.Atendimento.Paciente)
                .Include(ex => ex.Atendimento.Profissional)
                .AsNoTracking()
                .FirstOrDefaultAsync(ex => ex.Id == id);

            if (exame == null) return NotFound();

            var exameDto = new ExameDto
            {
                Id = exame.Id,
                Tipo = exame.Tipo,
                DataSolicitacao = exame.DataSolicitacao,
                DataRealizacao = exame.DataRealizacao,
                Resultado = exame.Resultado,
                AtendimentoId = exame.AtendimentoId,
                DataAtendimento = exame.Atendimento.DataHora,
                NomePaciente = exame.Atendimento.Paciente.NomeCompleto,
                NomeProfissional = exame.Atendimento.Profissional.NomeCompleto
            };

            return Ok(exameDto);
        }

        /// <summary>
        /// Solicita um novo exame para um atendimento.
        /// </summary>
        /// <response code="201">Exame criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateExameDto dto)
        {
            // Valida se o atendimento informado existe
            var atendimentoExiste = await _context.Atendimentos.AnyAsync(a => a.Id == dto.AtendimentoId);
            if (!atendimentoExiste)
            {
                return BadRequest(new { message = "O Atendimento informado não foi encontrado." });
            }

            var exame = new Exame
            {
                Id = Guid.NewGuid(),
                AtendimentoId = dto.AtendimentoId,
                Tipo = dto.Tipo,
                DataSolicitacao = DateTime.UtcNow, // Usa a data/hora atual do servidor
                Resultado = "Aguardando resultado" 
            };

            await _context.Exames.AddAsync(exame);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = exame.Id }, exame);
        }

        /// <summary>
        /// Adiciona o resultado a um exame existente.
        /// </summary>
        /// <response code="204">Atualização sucedida</response>
        /// <response code="404">Exame não encontrado</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateExameResultDto dto)
        {
            var exame = await _context.Exames.FindAsync(id);

            if (exame == null) return NotFound();

            exame.DataRealizacao = dto.DataRealizacao;
            exame.Resultado = dto.Resultado;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Exclui uma solicitação de exame.
        /// </summary>
        /// <response code="204">Exclusão sucedida</response>
        /// <response code="404">Exame não encontrado</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exame = await _context.Exames.FindAsync(id);

            if (exame == null) return NotFound();

            _context.Exames.Remove(exame);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}