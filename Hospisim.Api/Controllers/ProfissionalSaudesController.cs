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
    public class ProfissionalSaudesController : Controller
    {
        private readonly HospisimDbContext _context;

        public ProfissionalSaudesController(HospisimDbContext context)
        {
            _context = context;
        }

        // GET: ProfissionalSaudes
        public async Task<IActionResult> Index()
        {
            var profissionais = await _context.Profissionais
                .Include(p => p.Especialidade)
                .AsNoTracking()
                .ToListAsync();
            return View(profissionais);
        }

        // GET: ProfissionalSaudes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var profissional = await _context.Profissionais
                .Include(p => p.Especialidade)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (profissional == null) return NotFound();

            return View(profissional);
        }

        // GET: ProfissionalSaudes/Create
        public async Task<IActionResult> Create()
        {
            await PopulateViewDataAsync(); // Popula todos os dropdowns
            return View();
        }

        // POST: ProfissionalSaudes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeCompleto,CPF,Email,Telefone,RegistroConselho,TipoRegistro,EspecialidadeId,DataAdmissao,CargaHorariaSemanal,Turno,Ativo")] ProfissionalSaude profissionalSaude)
        {
            System.Diagnostics.Debugger.Break();
            // A validação de CPF único deve ser feita aqui se necessário
            var cpfExistente = await _context.Profissionais.AnyAsync(p => p.CPF == profissionalSaude.CPF);
            if (cpfExistente)
            {
                ModelState.AddModelError("CPF", "Este CPF já está cadastrado.");
            }

            if (ModelState.IsValid)
            {
                profissionalSaude.Id = Guid.NewGuid();
                _context.Add(profissionalSaude);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Se o modelo não for válido, repopulamos os dados e retornamos a view
            await PopulateViewDataAsync(profissionalSaude.EspecialidadeId);
            return View(profissionalSaude);
        }

        // GET: ProfissionalSaudes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var profissional = await _context.Profissionais.FindAsync(id);
            if (profissional == null) return NotFound();

            await PopulateViewDataAsync(profissional.EspecialidadeId); // Popula dropdowns com o valor selecionado
            return View(profissional);
        }

        // POST: ProfissionalSaudes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,NomeCompleto,CPF,Email,Telefone,RegistroConselho,TipoRegistro,EspecialidadeId,DataAdmissao,CargaHorariaSemanal,Turno,Ativo")] ProfissionalSaude profissionalSaude)
        {
            System.Diagnostics.Debugger.Break();
            if (id != profissionalSaude.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profissionalSaude);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfissionalSaudeExists(profissionalSaude.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateViewDataAsync(profissionalSaude.EspecialidadeId);
            return View(profissionalSaude);
        }

        // GET: ProfissionalSaudes/Delete/5
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var profissional = await _context.Profissionais.FindAsync(id);
            if (profissional == null)
            {
                return NotFound();
            }

            _context.Profissionais.Remove(profissional);
            await _context.SaveChangesAsync();
            return Ok(); // Retorna sucesso para o JavaScript do modal
        }

        private bool ProfissionalSaudeExists(Guid id)
        {
            return _context.Profissionais.Any(e => e.Id == id);
        }

        // Método centralizado para popular os dropdowns
        private async Task PopulateViewDataAsync(object selectedEspecialidade = null)
        {
            ViewData["EspecialidadeId"] = new SelectList(await _context.Especialidades.OrderBy(e => e.Nome).ToListAsync(), "Id", "Nome", selectedEspecialidade);

            ViewData["TipoRegistroList"] = new SelectList(
                Enum.GetValues(typeof(TipoRegistro)).Cast<TipoRegistro>().Select(e => new { Value = e, Text = e.GetDisplayName() }),
                "Value", "Text"
            );

            ViewData["TurnoList"] = new SelectList(
                Enum.GetValues(typeof(Turno)).Cast<Turno>().Select(e => new { Value = e, Text = e.GetDisplayName() }),
                "Value", "Text"
            );
        }
    }
}