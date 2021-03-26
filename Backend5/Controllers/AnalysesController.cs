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
    public class AnalysesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalysesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Analyses
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

            var analyses = await this._context.Analyses
                .Include(p => p.Patient)
                .Include(l => l.Lab)
                .Where(x => x.PatientId == patientId)
                .ToListAsync();
            return View(analyses);
        }

        // GET: Analyses/Details/5
        public async Task<IActionResult> Details(Int32? analysisId)
        {
            if (analysisId == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses
                .Include(a => a.Lab)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AnalysisId == analysisId);
            if (analysis == null)
            {
                return NotFound();
            }

            ViewBag.Analysis = analysis;

            return View(analysis);
        }

        // GET: Analyses/Create
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

            this.ViewBag.Patient = patient;
            this.ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name");
            return View(new AnalysisModel());
        }

        // POST: Analyses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, AnalysisModel model)
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

            if (ModelState.IsValid)
            {
                var analysis = new Analysis()
                {
                    Status = model.Status,
                    Date = model.Date,
                    Type = model.Type,
                    LabId = model.LabId,
                    PatientId = patient.PatientId
                   
                };
                _context.Add(analysis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { patientId = patient.PatientId});
            }
            this.ViewBag.Patient = patient;
            this.ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name");
            return View(model);
        }

        // GET: Analyses/Edit/5
        public async Task<IActionResult> Edit(Int32? analysisId)
        {
            if (analysisId == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses.SingleOrDefaultAsync(x => x.AnalysisId == analysisId);

            if (analysis == null)
            {
                return NotFound();
            }

            var model = new AnalysisModel
            {
                Date = analysis.Date,
                Status = analysis.Status,
                Type = analysis.Type
            };
            this.ViewBag.Analysis = analysis;
            this.ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name", analysis.LabId);
            return View(model);
        }

        // POST: Analyses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? analysisId, AnalysisModel model)
        {
            if (analysisId == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses.SingleOrDefaultAsync(x => x.AnalysisId == analysisId);

            if (analysis == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                analysis.Type = model.Type;
                analysis.Date = model.Date;
                analysis.Status = model.Status;
                _context.Update(analysis);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { patientId = analysis.PatientId });
            }
            
            ViewBag.Analysis = analysis;
            ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name", analysis.LabId);

            return View(model);
        }

        // GET: Analyses/Delete/5
        public async Task<IActionResult> Delete(Int32? analysisId)
        {
            if (analysisId == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses
                .Include(a => a.Lab)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AnalysisId == analysisId);
            if (analysis == null)
            {
                return NotFound();
            }
            ViewBag.Analysis = analysis;
            return View(analysis);
        }

        // POST: Analyses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? analysisId)
        {
            var analysis = await _context.Analyses.SingleOrDefaultAsync(x=>x.AnalysisId ==  analysisId);
            _context.Analyses.Remove(analysis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { patientId = analysis.PatientId});
        }
    }
}
