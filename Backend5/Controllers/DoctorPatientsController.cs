﻿using System;
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
    public class DoctorPatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorPatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoctorPatients
        public async Task<IActionResult> Index(Int32? doctorId)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors.SingleOrDefaultAsync(x => x.DoctorId == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            var items = await this._context.DoctorPatients
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Where(k => k.DoctorId == doctorId)
                .ToListAsync();
            this.ViewBag.Doctor = doctor;
           
            return View(items);
        }

        

        // GET: DoctorPatients/Create
        public async Task<IActionResult> Create(Int32? doctorId)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors.SingleOrDefaultAsync(x => x.DoctorId == doctorId);

            if(doctor == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Doctor = doctor;

            this.ViewBag.PatientId = new SelectList(this._context.Patients
                .Where(l => !this._context.DoctorPatients
                .Any(x => x.DoctorId == doctor.DoctorId && x.PatientId == l.PatientId)), "PatientId", "Name");
            return View(new DoctorPatientModel());
        }

        // POST: DoctorPatients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? doctorId, DoctorPatientModel model)
        {

            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors.SingleOrDefaultAsync(x => x.DoctorId == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                var doctorPatient = new DoctorPatient
                {
                    DoctorId = doctor.DoctorId,
                    PatientId = model.PatientId
                };
                _context.Add(doctorPatient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { doctorId = doctor.DoctorId});
            }
            this.ViewBag.Doctor = doctor;

            this.ViewBag.PatientId = new SelectList(this._context.Patients
                .Where(l => !this._context.DoctorPatients
                .Any(x => x.DoctorId == doctor.DoctorId && x.PatientId == l.PatientId)), "PatientId", "Name");
            return View(model);
        }

    

        // GET: DoctorPatients/Delete/5
        public async Task<IActionResult> Delete(Int32? doctorId, Int32? patientId)
        {
            if (doctorId == null || patientId==null)
            {
                return NotFound();
            }

            var doctorPatient = await _context.DoctorPatients
                .Include(d => d.Doctor)
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.DoctorId == doctorId && m.PatientId == patientId);
            if (doctorPatient == null)
            {
                return NotFound();
            }

            return View(doctorPatient);
        }

        // POST: DoctorPatients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 doctorId, Int32 patientId)
        {
            var doctorPatient = await _context.DoctorPatients
                .FirstOrDefaultAsync(m => m.DoctorId == doctorId && m.PatientId == patientId);
            _context.DoctorPatients.Remove(doctorPatient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { doctorId = doctorId});
        }
    }
}
