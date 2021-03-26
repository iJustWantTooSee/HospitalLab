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
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(Int32? patientId)
        {
            if (patientId == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientId == patientId);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View(new PatientModel());
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    Name = model.Name,
                    Birthday = model.Birthday,
                    Address = model.Address,
                    Gender = model.Gender
                };
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(Int32? patientId)
        {
            if (patientId == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.SingleOrDefaultAsync(x => x.PatientId == patientId);
            
            if (patient == null)
            {
                return NotFound();
            }
            ViewBag.PatientId = patient.PatientId;
            var model = new PatientModel
            {
                Name = patient.Name,
                Birthday = patient.Birthday,
                Address = patient.Address,
                Gender = patient.Gender
            };

            return View(model);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? patientId, PatientModel model)
        {
            if (patientId == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.SingleOrDefaultAsync(x => x.PatientId == patientId);

            if (patient == null)
            {
                return NotFound();
            }

            ViewBag.PatientId = patient.PatientId;

            if (ModelState.IsValid)
            {
                patient.Name = model.Name;
                patient.Birthday = model.Birthday;
                patient.Gender = model.Gender;
                patient.Address = model.Address;
                await this._context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? patientId)
        {
            if (patientId == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientId == patientId);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int patientId)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(x=> x.PatientId == patientId);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
