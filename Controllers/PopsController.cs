using IntranetHub.Data;
using IntranetHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace IntranetHub.Controllers
{
    public class PopsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PopsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context; _env = env;
        }

        public async Task<IActionResult> Index(string searchString, int? branchId)
        {
            ViewBag.Branches = new SelectList(await _context.Branches.ToListAsync(), "Id", "Name");
            var pops = _context.Pops.Include(p => p.Branch).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                pops = pops.Where(p => p.Subject.Contains(searchString) || p.Body.Contains(searchString));
            if (branchId.HasValue)
                pops = pops.Where(p => p.BranchId == branchId.Value);

            return View(await pops.OrderByDescending(p => p.Date).ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Branches = new SelectList(await _context.Branches.ToListAsync(), "Id", "Name");
            return View();
        }

        [Authorize] [HttpPost] [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pop pop, IFormFile? uploadFile)
        {
            if (uploadFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "pops");
                Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, uploadFile.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await uploadFile.CopyToAsync(fileStream);
                pop.FileAttachmentPath = "/uploads/pops/" + uploadFile.FileName;
            }
            _context.Add(pop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize] [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var pop = await _context.Pops.FindAsync(id);
            if (pop != null) { pop.IsActive = !pop.IsActive; await _context.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}
