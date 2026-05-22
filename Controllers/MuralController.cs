using IntranetHub.Data;
using IntranetHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntranetHub.Controllers
{
    public class MuralController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MuralController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var viewModel = new MuralViewModel
            {
                Avisos = await _context.Avisos.Where(a => a.IsActive).OrderByDescending(a => a.CreatedAt).Take(10).ToListAsync(),
                Aniversarios = await _context.Aniversarios.Where(a => a.Date.Month == today.Month).ToListAsync()
            };
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Manage()
        {
            var viewModel = new MuralViewModel
            {
                Avisos = await _context.Avisos.ToListAsync(),
                Aniversarios = await _context.Aniversarios.ToListAsync()
            };
            return View(viewModel);
        }

        [Authorize] [HttpPost]
        public async Task<IActionResult> AddAviso(Aviso aviso)
        {
            _context.Avisos.Add(aviso); await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        [Authorize] [HttpPost]
        public async Task<IActionResult> DeleteAviso(int id)
        {
            var aviso = await _context.Avisos.FindAsync(id);
            if (aviso != null) { _context.Avisos.Remove(aviso); await _context.SaveChangesAsync(); }
            return RedirectToAction(nameof(Manage));
        }

        [Authorize] [HttpPost]
        public async Task<IActionResult> AddAniversario(Aniversario aniversario)
        {
            _context.Aniversarios.Add(aniversario); await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        [Authorize] [HttpPost]
        public async Task<IActionResult> DeleteAniversario(int id)
        {
            var aniv = await _context.Aniversarios.FindAsync(id);
            if (aniv != null) { _context.Aniversarios.Remove(aniv); await _context.SaveChangesAsync(); }
            return RedirectToAction(nameof(Manage));
        }
    }
}
