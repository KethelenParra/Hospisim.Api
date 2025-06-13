using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;
using Hospisim.Api.Extensions;
using Hospisim.Api.Dtos.Atendimento;
using Hospisim.Api.Dtos.Prontuario;

namespace Hospisim.Api.Controllers.Api
{
    [ApiController]
    [Route("api/prontuarios")]
    public class ProntuariosApiController : ControllerBase
    {
        private readonly HospisimDbContext _context;

        public ProntuariosApiController(HospisimDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém os detalhes de um prontuário, incluindo seu histórico de atendimentos.
        /// </summary>
        /// <response code="200">Retorna o prontuario</response>
        /// <response code="404">Prontuario não encontrado</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var prontuario = await _context.Prontuarios
                .Include(p => p.Paciente)
                .Include(p => p.Atendimentos).ThenInclude(a => a.Profissional)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prontuario == null) return NotFound();

            var dto = new ProntuarioDetalhesDto
            {
                Id = prontuario.Id,
                Numero = prontuario.Numero,
                DataAbertura = prontuario.DataAbertura,
                Observacoes = prontuario.Observacoes,
                PacienteId = prontuario.PacienteId,
                NomePaciente = prontuario.Paciente.NomeCompleto,
                CpfPaciente = prontuario.Paciente.CPF.ToCPFFormat(),
                Atendimentos = prontuario.Atendimentos.Select(a => new AtendimentoResumoDto
                {
                    Id = a.Id,
                    DataHora = a.DataHora,
                    Tipo = a.Tipo.GetDisplayName(),
                    Status = a.Status.GetDisplayName(),
                    NomeProfissional = a.Profissional.NomeCompleto
                }).OrderByDescending(a => a.DataHora).ToList()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Cria um novo prontuário para um paciente (se ele ainda não tiver um).
        /// </summary>
        /// <response code="201">Prontuario criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateProntuarioDto dto)
        {
            // REGRA DE NEGÓCIO: Um paciente só pode ter um prontuário.
            var prontuarioExistente = await _context.Prontuarios.AnyAsync(p => p.PacienteId == dto.PacienteId);
            if (prontuarioExistente)
            {
                return BadRequest(new { message = "Este paciente já possui um prontuário." });
            }

            var paciente = await _context.Pacientes.FindAsync(dto.PacienteId);
            if (paciente == null) return BadRequest(new { message = "Paciente não encontrado." });

            var prontuario = new Prontuario
            {
                Id = Guid.NewGuid(),
                PacienteId = dto.PacienteId,
                Numero = $"PRT-{new Random().Next(10000, 99999)}", 
                DataAbertura = DateTime.UtcNow,
                Observacoes = dto.Observacoes
            };

            await _context.Prontuarios.AddAsync(prontuario);
            await _context.SaveChangesAsync();

            var resultadoDto = await GetById(prontuario.Id);
            if (resultadoDto is OkObjectResult okResult)
            {
                return CreatedAtAction(nameof(GetById), new { id = prontuario.Id }, okResult.Value);
            }

            return StatusCode(500, "Erro ao recuperar o prontuário criado.");
        }

        /// <summary>
        /// Atualiza as observações gerais de um prontuário.
        /// </summary>
        /// <response code="204">Atualização sucedida</response>
        /// <response code="404">Prontuario não encontrado</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateProntuarioDto dto)
        {
            var prontuario = await _context.Prontuarios.FindAsync(id);
            if (prontuario == null) return NotFound();

            prontuario.Observacoes = dto.Observacoes;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Exclui um prontuário (só é permitido se não tiver atendimentos registrados).
        /// </summary>
        /// <response code="204">Exclusão sucedida</response>
        /// <response code="404">Prontuario não encontrado</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var prontuario = await _context.Prontuarios
                .Include(p => p.Atendimentos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prontuario == null) return NotFound();

            // REGRA DE NEGÓCIO: Não permitir excluir prontuário com histórico.
            if (prontuario.Atendimentos.Any())
            {
                return BadRequest(new { message = "Não é possível excluir um prontuário que possui atendimentos registrados." });
            }

            _context.Prontuarios.Remove(prontuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}