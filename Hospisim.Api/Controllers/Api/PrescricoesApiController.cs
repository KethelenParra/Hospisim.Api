using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;
using Hospisim.Api.Dtos;
using Hospisim.Api.Enums;
using Hospisim.Api.Extensions;
using Hospisim.Api.Dtos.Prescricao;

namespace Hospisim.Api.Controllers.Api
{
    [ApiController]
    [Route("api/prescricoes")]
    public class PrescricoesApiController : ControllerBase
    {
        private readonly HospisimDbContext _context;

        public PrescricoesApiController(HospisimDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém uma prescrição específica pelo ID.
        /// </summary>
        /// <response code="200">Retorna o prescricao</response>
        /// <response code="404">Prescricao não encontrado</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var prescricao = await _context.Prescricoes
                .Include(p => p.Atendimento.Paciente)
                .Include(p => p.Profissional)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prescricao == null) return NotFound();

            var dto = new PrescricaoDto
            {
                Id = prescricao.Id,
                Medicamento = prescricao.Medicamento,
                Dosagem = prescricao.Dosagem,
                Frequencia = prescricao.Frequencia,
                ViaAdministracao = prescricao.ViaAdministracao.GetDisplayName(),
                DataInicio = prescricao.DataInicio,
                DataFim = prescricao.DataFim,
                Observacoes = prescricao.Observacoes,
                StatusPrescricao = prescricao.StatusPrescricao.GetDisplayName(),
                ReacoesAdversas = prescricao.ReacoesAdversas,
                AtendimentoId = prescricao.AtendimentoId,
                PacienteId = prescricao.Atendimento.PacienteId,
                NomePaciente = prescricao.Atendimento.Paciente.NomeCompleto,
                ProfissionalId = prescricao.ProfissionalId,
                NomeProfissional = prescricao.Profissional.NomeCompleto
            };

            return Ok(dto);
        }

        /// <summary>
        /// Cria uma nova prescrição para um atendimento.
        /// </summary>
        /// <response code="201">Prescricao criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreatePrescricaoDto dto)
        {
            // Valida se o atendimento e o profissional existem
            var atendimentoExiste = await _context.Atendimentos.AnyAsync(a => a.Id == dto.AtendimentoId);
            if (!atendimentoExiste) return BadRequest(new { message = "Atendimento não encontrado." });

            var profissionalExiste = await _context.Profissionais.AnyAsync(p => p.Id == dto.ProfissionalId);
            if (!profissionalExiste) return BadRequest(new { message = "Profissional não encontrado." });

            var prescricao = new Prescricao
            {
                Id = Guid.NewGuid(),
                AtendimentoId = dto.AtendimentoId,
                ProfissionalId = dto.ProfissionalId,
                Medicamento = dto.Medicamento,
                Dosagem = dto.Dosagem,
                Frequencia = dto.Frequencia,
                ViaAdministracao = dto.ViaAdministracao,
                DataInicio = DateTime.UtcNow,
                StatusPrescricao = StatusPrescricao.Ativa, // Status inicial
                Observacoes = dto.Observacoes
            };

            await _context.Prescricoes.AddAsync(prescricao);
            await _context.SaveChangesAsync();

            var resultadoDto = await GetById(prescricao.Id);
            if (resultadoDto is OkObjectResult okResult)
            {
                return CreatedAtAction(nameof(GetById), new { id = prescricao.Id }, okResult.Value);
            }

            return StatusCode(500, "Erro ao recuperar a prescrição criada.");
        }

        /// <summary>
        /// Atualiza uma prescrição existente.
        /// </summary>
        /// <response code="204">Atualização sucedida</response>
        /// <response code="404">Prescricao não encontrado</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdatePrescricaoDto dto)
        {
            var prescricao = await _context.Prescricoes.FindAsync(id);

            if (prescricao == null) return NotFound();

            prescricao.Medicamento = dto.Medicamento;
            prescricao.Dosagem = dto.Dosagem;
            prescricao.Frequencia = dto.Frequencia;
            prescricao.ViaAdministracao = dto.ViaAdministracao;
            prescricao.DataFim = dto.DataFim;
            prescricao.Observacoes = dto.Observacoes;
            prescricao.StatusPrescricao = dto.StatusPrescricao;
            prescricao.ReacoesAdversas = dto.ReacoesAdversas;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Exclui uma prescrição (não recomendado, idealmente se cancela).
        /// </summary>
        /// <response code="204">Exclusão sucedida</response>
        /// <response code="404">Prescricao não encontrado</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var prescricao = await _context.Prescricoes.FindAsync(id);
            if (prescricao == null) return NotFound();

            // Lógica de negócio: em vez de deletar, uma prescrição geralmente é cancelada.
            // Para um CRUD simples, a exclusão é mantida.
            _context.Prescricoes.Remove(prescricao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}