using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;
using Hospisim.Api.Enums;
using Hospisim.Api.Dtos.AltaHospitalar;
using Hospisim.Api.Models.Dtos.AltaHospitalar;

namespace Hospisim.Api.Controllers.Api
{
    [ApiController]
    [Route("api/altashospitalares")]
    public class AltasHospitalaresApiController : ControllerBase
    {
        private readonly HospisimDbContext _context;

        public AltasHospitalaresApiController(HospisimDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém uma alta hospitalar específica pelo ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var alta = await _context.AltasHospitalares
                .Include(ah => ah.Internacao.Paciente) 
                .AsNoTracking()
                .FirstOrDefaultAsync(ah => ah.Id == id);

            if (alta == null) return NotFound();

            var dto = new AltaHospitalarDto
            {
                Id = alta.Id,
                Data = alta.Data,
                CondicaoPaciente = alta.CondicaoPaciente,
                InstrucoesPosAlta = alta.InstrucoesPosAlta,
                InternacaoId = alta.InternacaoId,
                NomePaciente = alta.Internacao.Paciente.NomeCompleto,
                DataEntradaInternacao = alta.Internacao.DataEntrada
            };

            return Ok(dto);
        }

        /// <summary>
        /// Registra uma nova alta hospitalar para uma internação.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAltaHospitalarDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var internacao = await _context.Internacoes.FirstOrDefaultAsync(i => i.Id == dto.InternacaoId);
                if (internacao == null)
                {
                    return BadRequest(new { message = "Internação não encontrada." });
                }

                if (internacao.StatusInternacao == StatusInternacao.AltaConcedida)
                {
                    return BadRequest(new { message = "Esta internação já possui uma alta registrada." });
                }

                internacao.StatusInternacao = StatusInternacao.AltaConcedida;

                var alta = new AltaHospitalar
                {
                    Id = Guid.NewGuid(),
                    InternacaoId = dto.InternacaoId,
                    Data = DateTime.UtcNow,
                    CondicaoPaciente = dto.CondicaoPaciente,
                    InstrucoesPosAlta = dto.InstrucoesPosAlta
                };

                await _context.AltasHospitalares.AddAsync(alta);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var resultadoDaAcao = await GetById(alta.Id);
                if (resultadoDaAcao is OkObjectResult okResult)
                {
                    return CreatedAtAction(nameof(GetById), new { id = alta.Id }, okResult.Value);
                }

                return StatusCode(500, "A alta foi criada, mas houve um erro ao recuperar os detalhes.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { message = "Ocorreu um erro interno ao registrar a alta.", error = innerExceptionMessage });
            }
        }

        /// <summary>
        /// Atualiza os dados de uma alta hospitalar existente.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateAltaHospitalarDto dto)
        {
            var alta = await _context.AltasHospitalares.FindAsync(id);

            if (alta == null) return NotFound();

            alta.CondicaoPaciente = dto.CondicaoPaciente;
            alta.InstrucoesPosAlta = dto.InstrucoesPosAlta;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}