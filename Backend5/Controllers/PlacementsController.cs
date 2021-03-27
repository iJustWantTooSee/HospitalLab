using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend5.Data;
using Backend5.Models;
using Backend5.Models.ViewModels;

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
        public async Task<IActionResult> Index(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards.SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;

            var plasements = await this._context.Placements
                .Include(w => w.Ward)
                .Include(p => p.Patient)
                .Where(x => x.WardId == wardId)
                .ToListAsync();
            return View(plasements);
        }

        // GET: Placements/Details/5
        public async Task<IActionResult> Details(Int32? placementId)
        {
            if (placementId == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .FirstOrDefaultAsync(m => m.PlacementId == placementId);

            if (placement == null)
            {
                return NotFound();
            }

            this.ViewBag.Placement = placement;

            return View(placement);
        }

        // GET: Placements/Create
        public async Task<IActionResult> Create(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards.SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name");
            return View(new PlacementModel());
        }

        // POST: Placements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? wardId, PlacementModel model)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards.SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                var placement = new Placement
                {
                    WardId = ward.Id,
                    PatientId = model.PatientId,
                    Bed = model.Bed
                };
                _context.Add(placement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { wardId = ward.Id });
            }
            this.ViewBag.Ward = ward;
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name");
            return View(model);
        }

        // GET: Placements/Edit/5
        public async Task<IActionResult> Edit(Int32? placementId)
        {
            if (placementId == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements.SingleOrDefaultAsync(x => x.PlacementId == placementId);

            if (placement == null)
            {
                return NotFound();
            }

            var model = new PlacementModel
            {
                Bed = placement.Bed,
                PatientId = placement.PatientId
            };

            this.ViewBag.Placement = placement;

            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", placement.PatientId);

            return View(model);
        }

        // POST: Placements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? placementId, PlacementModel model)
        {
            if (placementId == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements.SingleOrDefaultAsync(x => x.PlacementId == placementId);

            if (placement == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                placement.PatientId = model.PatientId;
                placement.Bed = model.Bed;
                _context.Update(placement);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { wardId=placement.WardId});
            }
            this.ViewBag.Placement = placement;
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", placement.PatientId);

            return View(model);
        }

        // GET: Placements/Delete/5
        public async Task<IActionResult> Delete(Int32? placementId)
        {
            if (placementId == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .FirstOrDefaultAsync(m => m.PlacementId == placementId);
            if (placement == null)
            {
                return NotFound();
            }

            this.ViewBag.Placement = placement;
            return View(placement);
        }

        // POST: Placements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 placementId)
        {
            var placement = await _context.Placements.SingleOrDefaultAsync(x => x.PlacementId == placementId);
            _context.Placements.Remove(placement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { wardId = placement.WardId});
        }
    }
}
