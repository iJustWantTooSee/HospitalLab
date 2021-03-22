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
    public class HospitalDoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalDoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HospitalDoctors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HospitalDoctors.Include(h => h.Doctor).Include(h => h.Hospital);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HospitalDoctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalDoctor = await _context.HospitalDoctors
                .Include(h => h.Doctor)
                .Include(h => h.Hospital)
                .FirstOrDefaultAsync(m => m.HospitalId == id);
            if (hospitalDoctor == null)
            {
                return NotFound();
            }

            return View(hospitalDoctor);
        }

        // GET: HospitalDoctors/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name");
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Name");
            return View();
        }

        // POST: HospitalDoctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HospitalId,DoctorId")] HospitalDoctor hospitalDoctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospitalDoctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name", hospitalDoctor.DoctorId);
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Name", hospitalDoctor.HospitalId);
            return View(hospitalDoctor);
        }

        // GET: HospitalDoctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalDoctor = await _context.HospitalDoctors.FindAsync(id);
            if (hospitalDoctor == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name", hospitalDoctor.DoctorId);
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Name", hospitalDoctor.HospitalId);
            return View(hospitalDoctor);
        }

        // POST: HospitalDoctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HospitalId,DoctorId")] HospitalDoctor hospitalDoctor)
        {
            if (id != hospitalDoctor.HospitalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospitalDoctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalDoctorExists(hospitalDoctor.HospitalId))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name", hospitalDoctor.DoctorId);
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Name", hospitalDoctor.HospitalId);
            return View(hospitalDoctor);
        }

        // GET: HospitalDoctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalDoctor = await _context.HospitalDoctors
                .Include(h => h.Doctor)
                .Include(h => h.Hospital)
                .FirstOrDefaultAsync(m => m.HospitalId == id);
            if (hospitalDoctor == null)
            {
                return NotFound();
            }

            return View(hospitalDoctor);
        }

        // POST: HospitalDoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospitalDoctor = await _context.HospitalDoctors.FindAsync(id);
            _context.HospitalDoctors.Remove(hospitalDoctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalDoctorExists(int id)
        {
            return _context.HospitalDoctors.Any(e => e.HospitalId == id);
        }
    }
}
