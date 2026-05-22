using IntranetHub.Data;
using IntranetHub.Models;
using IntranetHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntranetHub.Controllers
{
    public class OuvidoriaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public OuvidoriaController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet] public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(OuvidoriaMessage model)
        {
            if (model.IsAnonymous) { model.Name = "Anônimo"; model.Email = null; }
            model.SubmittedAt = DateTime.Now;
            _context.OuvidoriaMessages.Add(model);
            await _context.SaveChangesAsync();
            
            await _emailService.SendEmailAsync("rh@pmlogistica.com.br", $"Ouvidoria: {model.Subject}", model.Message);
            TempData["SuccessMessage"] = "Enviado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize] [HttpGet]
        public async Task<IActionResult> List() => View(await _context.OuvidoriaMessages.OrderByDescending(m => m.SubmittedAt).ToListAsync());
    }
}
