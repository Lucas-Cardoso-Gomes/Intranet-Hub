using IntranetHub.Data;
using IntranetHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace IntranetHub.Controllers
{
    public class CincoSController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CincoSController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context; _env = env;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Ranking = await _context.Branches
                .Select(b => new { BranchName = b.Name, TotalScore = _context.CincoSAudits.Where(a => a.BranchId == b.Id).ToList().Sum(a => a.TotalScore) })
                .OrderByDescending(r => r.TotalScore).ToListAsync();

            return View(await _context.CincoSAudits.Include(a => a.Branch).OrderByDescending(a => a.AuditDate).Take(10).ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Branches = new SelectList(await _context.Branches.ToListAsync(), "Id", "Name");
            return View();
        }

        [Authorize] [HttpPost]
        public async Task<IActionResult> Create(CincoSAudit audit, List<IFormFile> images)
        {
            _context.CincoSAudits.Add(audit);
            await _context.SaveChangesAsync();

            if (images != null)
            {
                var folder = Path.Combine(_env.WebRootPath, "uploads", "5s");
                Directory.CreateDirectory(folder);
                foreach (var file in images)
                {
                    var filePath = Path.Combine(folder, file.FileName);
                    using (var fs = new FileStream(filePath, FileMode.Create)) await file.CopyToAsync(fs);
                    _context.CincoSImages.Add(new CincoSImage { CincoSAuditId = audit.Id, ImagePath = "/uploads/5s/" + file.FileName });
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Gallery(int? branchId)
        {
            ViewBag.Branches = new SelectList(await _context.Branches.ToListAsync(), "Id", "Name");
            var query = _context.CincoSImages.Include(i => i.CincoSAudit).ThenInclude(a => a.Branch).AsQueryable();
            if (branchId.HasValue) query = query.Where(i => i.CincoSAudit.BranchId == branchId.Value);
            return View(await query.ToListAsync());
        }
    }
}
