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
    public class PlacementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Placements
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Placements.Include(p => p.Patient).Include(p => p.Ward);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Placements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .FirstOrDefaultAsync(m => m.PlacementId == id);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        // GET: Placements/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name");
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name");
            return View();
        }

        // POST: Placements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlacementId,Bed,WardId,PatientId")] Placement placement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(placement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", placement.PatientId);
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", placement.WardId);
            return View(placement);
        }

        // GET: Placements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements.FindAsync(id);
            if (placement == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", placement.PatientId);
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", placement.WardId);
            return View(placement);
        }

        // POST: Placements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlacementId,Bed,WardId,PatientId")] Placement placement)
        {
            if (id != placement.PlacementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(placement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlacementExists(placement.PlacementId))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", placement.PatientId);
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", placement.WardId);
            return View(placement);
        }

        // GET: Placements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .FirstOrDefaultAsync(m => m.PlacementId == id);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        // POST: Placements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var placement = await _context.Placements.FindAsync(id);
            _context.Placements.Remove(placement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlacementExists(int id)
        {
            return _context.Placements.Any(e => e.PlacementId == id);
        }
    }
}
