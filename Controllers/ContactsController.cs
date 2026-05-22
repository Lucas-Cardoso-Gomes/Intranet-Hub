using IntranetHub.Data;
using IntranetHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntranetHub.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ContactsController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(string searchString)
        {
            var contacts = _context.Contacts.Include(c => c.Branch).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
                contacts = contacts.Where(c => c.Name.Contains(searchString) || c.Department.Contains(searchString));
            return View(await contacts.OrderBy(c => c.Name).ToListAsync());
        }

        [Authorize] public async Task<IActionResult> Manage() => View(await _context.Contacts.Include(c => c.Branch).ToListAsync());

        [Authorize] [HttpPost]
        public async Task<IActionResult> AddContact(Contact contact)
        {
            _context.Contacts.Add(contact); await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }
    }
}
