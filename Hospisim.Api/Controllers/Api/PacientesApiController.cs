using Hospisim.Api.Data;
using Hospisim.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospisim.Api.Controllers.Api
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacientesApiController : ControllerBase
    {
        private readonly HospisimDbContext _context;

        public PacientesApiController(HospisimDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os pacientes cadastrados
        /// </summary>
        /// <response code="200">Retorna lista de pacientes</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
            => Ok(_context.Pacientes
                          .Include(p => p.Prontuarios)   
                          .AsNoTracking()
                          .ToList());

        /// <summary>
        /// Obtém um paciente pelo Id
        /// </summary>
        /// <response code="200">Retorna o paciente</response>
        /// <response code="404">Paciente não encontrado</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(Guid id)
        {
            var paciente = _context.Pacientes
                            .Include(p => p.Prontuarios)
                            .AsNoTracking()
                            .FirstOrDefault(p => p.Id == id);
            if (paciente == null) return NotFound();
            return Ok(paciente);
        }

        /// <summary>
        /// Cadastra um novo paciente
        /// </summary>
        /// <response code="201">Paciente criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(Paciente paciente)
        {
            paciente.Id = Guid.NewGuid();
            _context.Pacientes.Add(paciente);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = paciente.Id }, paciente);
        }

        /// <summary>
        /// Atualiza os dados de um paciente
        /// </summary>
        /// <response code="204">Atualização sucedida</response>
        /// <response code="404">Paciente não encontrado</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(Guid id, Paciente input)
        {
            var paciente = _context.Pacientes.SingleOrDefault(p => p.Id == id);
            if (paciente == null) return NotFound();

            // Atualiza campos permitidos
            paciente.NomeCompleto = input.NomeCompleto;
            paciente.CPF = input.CPF;
            paciente.DataNascimento = input.DataNascimento;
            paciente.Sexo = input.Sexo;
            paciente.TipoSanguineo = input.TipoSanguineo;
            paciente.Telefone = input.Telefone;
            paciente.Email = input.Email;
            paciente.EnderecoCompleto = input.EnderecoCompleto;
            paciente.NumeroCartaoSUS = input.NumeroCartaoSUS;
            paciente.EstadoCivil = input.EstadoCivil;
            paciente.PossuiPlanoSaude = input.PossuiPlanoSaude;

            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Exclui um paciente
        /// </summary>
        /// <response code="204">Exclusão sucedida</response>
        /// <response code="404">Paciente não encontrado</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            var paciente = _context.Pacientes.SingleOrDefault(p => p.Id == id);
            if (paciente == null) return NotFound();

            _context.Pacientes.Remove(paciente);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
