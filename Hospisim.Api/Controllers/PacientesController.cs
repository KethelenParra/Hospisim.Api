using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;
using Hospisim.Api.Enums;
using Hospisim.Api.Extensions;

namespace Hospisim.Api.Controllers
{
    public class PacientesController : Controller
    {
        private readonly HospisimDbContext _context;

        public PacientesController(HospisimDbContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pacientes.ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            PopulateEnums();
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeCompleto,CPF,DataNascimento,Sexo,TipoSanguineo,Telefone,Email,EnderecoCompleto,NumeroCartaoSUS,EstadoCivil,PossuiPlanoSaude")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                paciente.Id = Guid.NewGuid();
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Se cair aqui, precisa repopular os enums antes de retornar a View
            PopulateEnums();
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            PopulateEnums();
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,NomeCompleto,CPF,DataNascimento,Sexo,TipoSanguineo,Telefone,Email,EnderecoCompleto,NumeroCartaoSUS,EstadoCivil,PossuiPlanoSaude")] Paciente paciente)
        {
            if (id != paciente.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id)) return NotFound();
                    throw;
                }
            }

            // Se houver erro de validação, repopular enums
            PopulateEnums();
            return View(paciente);
        }
        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(Guid id)
        {
            return _context.Pacientes.Any(e => e.Id == id);
        }

        private void PopulateEnums()
        {
            ViewData["SexoList"] = new SelectList(Enum.GetValues(typeof(Sexo))
                                                      .Cast<Sexo>()
                                                      .Select(e => new { Value = e, Text = e.ToString() }),
                                                      "Value", "Text");
            ViewData["TipoSanguineoList"] = new SelectList(Enum.GetValues(typeof(TipoSanguineo))
                                                      .Cast<TipoSanguineo>()
                                                      .Select(e => new { Value = e, Text = e.GetDisplayName() }),
                                                      "Value", "Text");
            ViewData["EstadoCivilList"] = new SelectList(Enum.GetValues(typeof(EstadoCivil))
                                                      .Cast<EstadoCivil>()
                                                      .Select(e => new { Value = e, Text = e.ToString() }),
                                                      "Value", "Text");
        }

    }
}
