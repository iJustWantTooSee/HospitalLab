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
    public class DiagnosesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiagnosesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Diagnoses
        public async Task<IActionResult> Index(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.PatientId == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            ViewBag.Patient = patient;

            var diagnoses = await this._context.Diagnoses
                .Include(x => x.Patient)
                .Where(h => h.PatientId == patientId)
                .ToListAsync();

            return View(diagnoses);
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(Int32? diagnosisId)
        {
            if (diagnosisId == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.DiagnosisId == diagnosisId);
            if (diagnosis == null)
            {
                return NotFound();
            }

            ViewBag.PatientId = diagnosis.PatientId;
            return View(diagnosis);
        }

        // GET: Diagnoses/Create
        public async Task<IActionResult> Create(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.PatientId == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            ViewBag.Patient = patient;
            return View(new DiagnosisModel());
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, DiagnosisModel model)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.PatientId == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            ViewBag.Patient = patient;
            if (ModelState.IsValid)
            {
                var diagnosis = new Diagnosis
                {
                    PatientId = patient.PatientId,
                    Patient = patient,
                    Complications = model.Complications,
                    Type = model.Type,
                    Details = model.Details
                };
                _context.Add(diagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { patientId = patient.PatientId });
            }

            return View(model);
        }

        // GET: Diagnoses/Edit/5
        public async Task<IActionResult> Edit(Int32? diagnosisId)
        {
            if (diagnosisId == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.SingleOrDefaultAsync(x => x.DiagnosisId == diagnosisId);
            if (diagnosis == null)
            {
                return NotFound();
            }

            var model = new DiagnosisModel
            {
                Complications = diagnosis.Complications,
                Type = diagnosis.Type,
                Details = diagnosis.Details
            };
            ViewBag.Diagnosis = diagnosis;
            return View(model);
        }

        // POST: Diagnoses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? diagnosisId, DiagnosisModel model)
        {
            if (diagnosisId == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.SingleOrDefaultAsync(x => x.DiagnosisId == diagnosisId);

            if (diagnosis == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                diagnosis.Complications = model.Complications;
                diagnosis.Type = model.Type;
                diagnosis.Details = model.Details;
                _context.Update(diagnosis);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { patientId = diagnosis.PatientId});
            }
            ViewBag.Diagnosis = diagnosis;
            return View(model);
        }

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(Int32? diagnosisId)
        {
            if (diagnosisId == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.DiagnosisId == diagnosisId);
            ViewBag.Diagnosis = diagnosis;
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 diagnosisId)
        {
            var diagnosis = await _context.Diagnoses.SingleOrDefaultAsync(x=> x.DiagnosisId == diagnosisId);
            _context.Diagnoses.Remove(diagnosis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { patientId = diagnosis.PatientId});
        }

    }
}
