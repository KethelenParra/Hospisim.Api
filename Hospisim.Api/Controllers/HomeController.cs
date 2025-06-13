using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Data;
using Hospisim.Api.Models;

namespace Hospisim.Api.Controllers
{
    // ViewModel para carregar os dados do Dashboard
    public class DashboardViewModel
    {
        public int TotalPacientes { get; set; }
        public int TotalProfissionaisAtivos { get; set; }
        public int AtendimentosHoje { get; set; }
        public int InternacoesAtivas { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly HospisimDbContext _context;

        public HomeController(HospisimDbContext context)
        {
            _context = context;
        }

        // Action principal que vai montar e exibir o Dashboard
        public async Task<IActionResult> Index()
        {
            var hoje = DateTime.UtcNow.Date;

            var viewModel = new DashboardViewModel
            {
                TotalPacientes = await _context.Pacientes.CountAsync(),
                TotalProfissionaisAtivos = await _context.Profissionais.CountAsync(p => p.Ativo),
                AtendimentosHoje = await _context.Atendimentos.CountAsync(a => a.DataHora.Date == hoje),
                InternacoesAtivas = await _context.Internacoes.CountAsync(i => i.StatusInternacao == Enums.StatusInternacao.Ativa)
            };

            return View(viewModel);
        }
    }
}