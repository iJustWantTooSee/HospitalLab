using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend5.Data;
using Backend5.Models;

namespace Backend5.Controllers
{
    public class LabPhonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LabPhonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LabPhones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LabPhones.Include(l => l.Lab);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LabPhones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labPhone = await _context.LabPhones
                .Include(l => l.Lab)
                .FirstOrDefaultAsync(m => m.LabId == id);
            if (labPhone == null)
            {
                return NotFound();
            }

            return View(labPhone);
        }

        // GET: LabPhones/Create
        public IActionResult Create()
        {
            ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name");
            return View();
        }

        // POST: LabPhones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabId,PhoneId,Number")] LabPhone labPhone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labPhone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name", labPhone.LabId);
            return View(labPhone);
        }

        // GET: LabPhones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labPhone = await _context.LabPhones.FindAsync(id);
            if (labPhone == null)
            {
                return NotFound();
            }
            ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name", labPhone.LabId);
            return View(labPhone);
        }

        // POST: LabPhones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LabId,PhoneId,Number")] LabPhone labPhone)
        {
            if (id != labPhone.LabId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labPhone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabPhoneExists(labPhone.LabId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name", labPhone.LabId);
            return View(labPhone);
        }

        // GET: LabPhones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labPhone = await _context.LabPhones
                .Include(l => l.Lab)
                .FirstOrDefaultAsync(m => m.LabId == id);
            if (labPhone == null)
            {
                return NotFound();
            }

            return View(labPhone);
        }

        // POST: LabPhones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var labPhone = await _context.LabPhones.FindAsync(id);
            _context.LabPhones.Remove(labPhone);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabPhoneExists(int id)
        {
            return _context.LabPhones.Any(e => e.LabId == id);
        }
    }
}
