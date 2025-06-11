using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;

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

        /// <summary>Obtém todos os prontuários cadastrados</summary>
        /// <response code="204">Retorna lista dos prontuarios</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll() =>
            Ok(_context.Prontuarios
                       .Include(pr => pr.Paciente)
                       .Include(pr => pr.Atendimentos)
                       .AsNoTracking()
                       .ToList());

        /// <summary>Obtém um prontuário pelo Id</summary>
        /// <response code="204">Retorna o prontuario</response>
        /// <response code="404">Prontuario não encontrado</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(Guid id)
        {
            var pr = _context.Prontuarios
                             .Include(pr => pr.Paciente)
                             .Include(pr => pr.Atendimentos)
                             .AsNoTracking()
                             .FirstOrDefault(pr => pr.Id == id);
            if (pr == null) return NotFound();
            return Ok(pr);
        }

        /// <summary>Cadastra um novo prontuário</summary>
        /// <response code="201">Prontruario criado com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(Prontuario pr)
        {
            pr.Id = Guid.NewGuid();
            _context.Prontuarios.Add(pr);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = pr.Id }, pr);
        }

        /// <summary>Atualiza um prontuário existente</summary>
        /// <response code="204">Atualização sucedida</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(Guid id, Prontuario input)
        {
            var pr = _context.Prontuarios.Find(id);
            if (pr == null) return NotFound();

            pr.Numero = input.Numero;
            pr.DataAbertura = input.DataAbertura;
            pr.Observacoes = input.Observacoes;
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>Exclui um prontuário</summary>
        /// <response code="204">Atualização sucedida</response>
        /// <response code="404">Prontuario não encontrado</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            var pr = _context.Prontuarios.Find(id);
            if (pr == null) return NotFound();
            _context.Prontuarios.Remove(pr);
            _context.SaveChanges();
            return NoContent();
        }
    }
}