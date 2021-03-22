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
    public class HospitalPhonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalPhonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HospitalPhones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HospitalPhones.Include(h => h.Hospital);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HospitalPhones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalPhone = await _context.HospitalPhones
                .Include(h => h.Hospital)
                .FirstOrDefaultAsync(m => m.HospitalId == id);
            if (hospitalPhone == null)
            {
                return NotFound();
            }

            return View(hospitalPhone);
        }

        // GET: HospitalPhones/Create
        public IActionResult Create()
        {
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Name");
            return View();
        }

        // POST: HospitalPhones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HospitalId,PhoneId,Number")] HospitalPhone hospitalPhone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospitalPhone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Name", hospitalPhone.HospitalId);
            return View(hospitalPhone);
        }

        // GET: HospitalPhones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalPhone = await _context.HospitalPhones.FindAsync(id);
            if (hospitalPhone == null)
            {
                return NotFound();
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Name", hospitalPhone.HospitalId);
            return View(hospitalPhone);
        }

        // POST: HospitalPhones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HospitalId,PhoneId,Number")] HospitalPhone hospitalPhone)
        {
            if (id != hospitalPhone.HospitalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospitalPhone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalPhoneExists(hospitalPhone.HospitalId))
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
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Name", hospitalPhone.HospitalId);
            return View(hospitalPhone);
        }

        // GET: HospitalPhones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalPhone = await _context.HospitalPhones
                .Include(h => h.Hospital)
                .FirstOrDefaultAsync(m => m.HospitalId == id);
            if (hospitalPhone == null)
            {
                return NotFound();
            }

            return View(hospitalPhone);
        }

        // POST: HospitalPhones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospitalPhone = await _context.HospitalPhones.FindAsync(id);
            _context.HospitalPhones.Remove(hospitalPhone);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalPhoneExists(int id)
        {
            return _context.HospitalPhones.Any(e => e.HospitalId == id);
        }
    }
}
