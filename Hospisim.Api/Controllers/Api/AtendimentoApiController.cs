using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;
using Hospisim.Api.Extensions;
using Hospisim.Api.Enums;
using Hospisim.Api.Dtos.Atendimento;
using Hospisim.Api.Dtos.Exame;
using Hospisim.Api.Dtos.Prescricao;

namespace Hospisim.Api.Controllers.Api
{
    [ApiController]
    [Route("api/atendimentos")]
    public class AtendimentosApiController : ControllerBase
    {
        private readonly HospisimDbContext _context;

        public AtendimentosApiController(HospisimDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém os detalhes de um atendimento específico.
        /// </summary>
        /// <response code="200">Retorna o atendimento</response>
        /// <response code="404">Atendimento não encontrado</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var atendimento = await _context.Atendimentos
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                .Include(a => a.Prescricoes)
                .Include(a => a.Exames)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (atendimento == null) return NotFound();

            // Mapeia para o DTO de detalhes
            var dto = new AtendimentoDetalhesDto
            {
                Id = atendimento.Id,
                DataHora = atendimento.DataHora,
                Tipo = atendimento.Tipo.GetDisplayName(),
                Status = atendimento.Status.GetDisplayName(),
                Local = atendimento.Local,
                PacienteId = atendimento.PacienteId,
                NomePaciente = atendimento.Paciente.NomeCompleto,
                ProfissionalId = atendimento.ProfissionalId,
                NomeProfissional = atendimento.Profissional.NomeCompleto,
                ProntuarioId = atendimento.ProntuarioId,
                Prescricoes = atendimento.Prescricoes.Select(p => new PrescricaoDto
                {
                    Id = p.Id,
                    Medicamento = p.Medicamento,
                    Dosagem = p.Dosagem,
                    Frequencia = p.Frequencia,
                    ViaAdministracao = p.ViaAdministracao.GetDisplayName(),
                    DataInicio = p.DataInicio,
                    DataFim = p.DataFim,
                    StatusPrescricao = p.StatusPrescricao.GetDisplayName()
                }).ToList(),

                Exames = atendimento.Exames.Select(e => new ExameDto
                {
                    Id = e.Id,
                    Tipo = e.Tipo,
                    DataSolicitacao = e.DataSolicitacao,
                    DataRealizacao = e.DataRealizacao,
                    Resultado = e.Resultado,
                    AtendimentoId = e.AtendimentoId,

                    // Adicionamos os dados do atendimento "pai" aqui
                    DataAtendimento = atendimento.DataHora,
                    NomePaciente = atendimento.Paciente.NomeCompleto,
                    NomeProfissional = atendimento.Profissional.NomeCompleto
                }).ToList()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Cria um novo atendimento para um paciente e profissional.
        /// </summary>
        /// <response code="201">Atendimento criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateAtendimentoDto dto)
        {
            var paciente = await _context.Pacientes.FindAsync(dto.PacienteId);
            if (paciente == null) return BadRequest(new { message = "Paciente não encontrado." });

            var profissional = await _context.Profissionais.FindAsync(dto.ProfissionalId);
            if (profissional == null) return BadRequest(new { message = "Profissional não encontrado." });

            var prontuario = await _context.Prontuarios.FirstOrDefaultAsync(p => p.PacienteId == dto.PacienteId);
            if (prontuario == null)
            {
                prontuario = new Prontuario
                {
                    Id = Guid.NewGuid(),
                    Numero = $"PRT-{new Random().Next(10000, 99999)}",
                    DataAbertura = DateTime.UtcNow,
                    PacienteId = dto.PacienteId,
                    Observacoes = "Prontuário gerado automaticamente no primeiro atendimento."
                };
                await _context.Prontuarios.AddAsync(prontuario);
            }

            var atendimento = new Atendimento
            {
                Id = Guid.NewGuid(),
                DataHora = DateTime.UtcNow,
                Tipo = dto.Tipo,
                Status = StatusAtendimento.EmAndamento,
                Local = dto.Local,
                PacienteId = dto.PacienteId,
                ProfissionalId = dto.ProfissionalId,
                ProntuarioId = prontuario.Id
            };

            await _context.Atendimentos.AddAsync(atendimento);
            await _context.SaveChangesAsync();

            var dtoDeRetorno = new AtendimentoDetalhesDto
            {
                Id = atendimento.Id,
                DataHora = atendimento.DataHora,
                Tipo = atendimento.Tipo.GetDisplayName(),
                Status = atendimento.Status.GetDisplayName(),
                Local = atendimento.Local,
                PacienteId = paciente.Id,
                NomePaciente = paciente.NomeCompleto, 
                ProfissionalId = profissional.Id,
                NomeProfissional = profissional.NomeCompleto, 
                ProntuarioId = prontuario.Id,
                Prescricoes = new List<PrescricaoDto>(), 
                Exames = new List<ExameDto>() 
            };

            return CreatedAtAction(nameof(GetById), new { id = atendimento.Id }, dtoDeRetorno);
        }

        /// <summary>
        /// Exclui um atendimento.
        /// </summary>
        /// <response code="204">Exclusão sucedida</response>
        /// <response code="404">Atendimento não encontrado</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var atendimento = await _context.Atendimentos.FindAsync(id);
            if (atendimento == null) return NotFound();

            _context.Atendimentos.Remove(atendimento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}