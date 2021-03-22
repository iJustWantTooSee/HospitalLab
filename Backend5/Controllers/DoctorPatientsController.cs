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
    public class DoctorPatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorPatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoctorPatients
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DoctorPatients.Include(d => d.Doctor).Include(d => d.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DoctorPatients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorPatient = await _context.DoctorPatients
                .Include(d => d.Doctor)
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (doctorPatient == null)
            {
                return NotFound();
            }

            return View(doctorPatient);
        }

        // GET: DoctorPatients/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name");
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name");
            return View();
        }

        // POST: DoctorPatients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorId,PatientId")] DoctorPatient doctorPatient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctorPatient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name", doctorPatient.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", doctorPatient.PatientId);
            return View(doctorPatient);
        }

        // GET: DoctorPatients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorPatient = await _context.DoctorPatients.FindAsync(id);
            if (doctorPatient == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name", doctorPatient.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", doctorPatient.PatientId);
            return View(doctorPatient);
        }

        // POST: DoctorPatients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorId,PatientId")] DoctorPatient doctorPatient)
        {
            if (id != doctorPatient.DoctorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorPatient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorPatientExists(doctorPatient.DoctorId))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name", doctorPatient.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", doctorPatient.PatientId);
            return View(doctorPatient);
        }

        // GET: DoctorPatients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorPatient = await _context.DoctorPatients
                .Include(d => d.Doctor)
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (doctorPatient == null)
            {
                return NotFound();
            }

            return View(doctorPatient);
        }

        // POST: DoctorPatients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctorPatient = await _context.DoctorPatients.FindAsync(id);
            _context.DoctorPatients.Remove(doctorPatient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorPatientExists(int id)
        {
            return _context.DoctorPatients.Any(e => e.DoctorId == id);
        }
    }
}
