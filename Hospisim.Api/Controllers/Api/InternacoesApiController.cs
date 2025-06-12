using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;
using Hospisim.Api.Enums;
using Hospisim.Api.Extensions;
using Hospisim.Api.Dtos.Internacao;
using Hospisim.Api.Models.Dtos.AltaHospitalar;

namespace Hospisim.Api.Controllers.Api
{
    [ApiController]
    [Route("api/internacoes")]
    public class InternacoesApiController : ControllerBase
    {
        private readonly HospisimDbContext _context;

        public InternacoesApiController(HospisimDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém os detalhes de uma internação específica.
        /// </summary>
        /// <response code="200">Retorna o internação</response>
        /// <response code="404">Internação não encontrado</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var internacao = await _context.Internacoes
                .Include(i => i.Atendimento.Paciente)
                .Include(i => i.AltaHospitalar)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            if (internacao == null) return NotFound();

            var dto = new InternacaoDto
            {
                Id = internacao.Id,
                DataEntrada = internacao.DataEntrada,
                PrevisaoAlta = internacao.PrevisaoAlta,
                MotivoInternacao = internacao.MotivoInternacao,
                Leito = internacao.Leito,
                Quarto = internacao.Quarto,
                Setor = internacao.Setor,
                PlanoSaudeUtilizado = internacao.PlanoSaudeUtilizado,
                ObservacoesClinicas = internacao.ObservacoesClinicas,
                StatusInternacao = internacao.StatusInternacao.GetDisplayName(),
                AtendimentoId = internacao.AtendimentoId,
                PacienteId = internacao.PacienteId,
                NomePaciente = internacao.Atendimento.Paciente.NomeCompleto,
                CpfPaciente = internacao.Atendimento.Paciente.CPF.ToCPFFormat(),

                Alta = internacao.AltaHospitalar != null ? new AltaHospitalarDto
                {
                    Id = internacao.AltaHospitalar.Id,
                    Data = internacao.AltaHospitalar.Data,
                    CondicaoPaciente = internacao.AltaHospitalar.CondicaoPaciente,
                    InstrucoesPosAlta = internacao.AltaHospitalar.InstrucoesPosAlta,
                    InternacaoId = internacao.Id,
                    NomePaciente = internacao.Atendimento.Paciente.NomeCompleto,
                    DataEntradaInternacao = internacao.DataEntrada
                } : null
            };

            return Ok(dto);
        }

        /// <summary>
        /// Cria uma nova internação a partir de um atendimento.
        /// </summary>
        /// <response code="201">Interção criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateInternacaoDto dto)
        {
            var atendimento = await _context.Atendimentos.FindAsync(dto.AtendimentoId);
            if (atendimento == null)
            {
                return BadRequest(new { message = "Atendimento não encontrado." });
            }

            var internacaoExistente = await _context.Internacoes.AnyAsync(i => i.AtendimentoId == dto.AtendimentoId);
            if (internacaoExistente)
            {
                return BadRequest(new { message = "Este atendimento já gerou uma internação." });
            }

            var internacao = new Internacao
            {
                Id = Guid.NewGuid(),
                AtendimentoId = dto.AtendimentoId,
                PacienteId = atendimento.PacienteId,
                DataEntrada = DateTime.UtcNow,
                StatusInternacao = StatusInternacao.Ativa,
                MotivoInternacao = dto.MotivoInternacao,
                PrevisaoAlta = dto.PrevisaoAlta,
                Setor = dto.Setor,
                Quarto = dto.Quarto,
                Leito = dto.Leito,
                PlanoSaudeUtilizado = dto.PlanoSaudeUtilizado,
                ObservacoesClinicas = dto.ObservacoesClinicas
            };

            await _context.Internacoes.AddAsync(internacao);
            await _context.SaveChangesAsync();

            var paciente = await _context.Pacientes
                .AsNoTracking()
                .FirstAsync(p => p.Id == internacao.PacienteId);

            var dtoDeRetorno = new InternacaoDto
            {
                Id = internacao.Id,
                DataEntrada = internacao.DataEntrada,
                PrevisaoAlta = internacao.PrevisaoAlta,
                MotivoInternacao = internacao.MotivoInternacao,
                Leito = internacao.Leito,
                Quarto = internacao.Quarto,
                Setor = internacao.Setor,
                PlanoSaudeUtilizado = internacao.PlanoSaudeUtilizado,
                ObservacoesClinicas = internacao.ObservacoesClinicas,
                StatusInternacao = internacao.StatusInternacao.GetDisplayName(),
                AtendimentoId = internacao.AtendimentoId,
                PacienteId = internacao.PacienteId,
                NomePaciente = paciente.NomeCompleto, 
                CpfPaciente = paciente.CPF.ToCPFFormat(),
                Alta = null 
            };

            return CreatedAtAction(nameof(GetById), new { id = internacao.Id }, dtoDeRetorno);
        }

        /// <summary>
        /// Exclui uma internação (use com cuidado, geralmente se cancela).
        /// </summary>
        /// <response code="204">Exclusão sucedida</response>
        /// <response code="404">Internação não encontrado</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var internacao = await _context.Internacoes.Include(i => i.AltaHospitalar).FirstOrDefaultAsync(i => i.Id == id);
            if (internacao == null) return NotFound();

            if (internacao.AltaHospitalar != null)
            {
                return BadRequest(new { message = "Não é possível excluir uma internação que já possui alta registrada." });
            }

            _context.Internacoes.Remove(internacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}