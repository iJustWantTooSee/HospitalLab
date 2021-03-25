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
    public class HospitalDoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalDoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HospitalDoctors
        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this._context.Hospitals
               .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            var items = await this._context.HospitalDoctors
                .Include(h => h.Hospital)
                .Include(h => h.Doctor)
                .Where(x => x.HospitalId == hospital.Id)
                .ToListAsync();
            this.ViewBag.Hospital = hospital;
            return View(items);
        }


        // GET: HospitalDoctors/Create
        public async Task<IActionResult> Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this._context.Hospitals
               .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Hospital = hospital;

            this.ViewBag.DoctorsId = new SelectList(this._context.Doctors
                .Where(l => !this._context.HospitalDoctors
                .Any(x => x.HospitalId == hospital.Id && x.DoctorId == l.DoctorId)), "DoctorId", "Name");
            return View(new HospitalDoctorModel());
        }

        // POST: HospitalDoctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, HospitalDoctorModel model)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this._context.Hospitals
               .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                var hospitalDoctor = new HospitalDoctor
                {
                    DoctorId = model.DoctorId,
                    HospitalId = hospital.Id
                };
                _context.Add(hospitalDoctor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { hospitalId = hospital.Id });
            }
            this.ViewBag.Hospital = hospital;

            this.ViewBag.DoctorsId = new SelectList(this._context.Doctors
                .Where(l => !this._context.HospitalDoctors
                .Any(x => x.HospitalId == hospital.Id && x.DoctorId == l.DoctorId)), "Id", "Name");
            return View(model);
        }


        // GET: HospitalDoctors/Delete/5
        public async Task<IActionResult> Delete(Int32? hospitalId, Int32? doctorId)
        {
            if (hospitalId == null || doctorId == null)
            {
                return NotFound();
            }

            var hospitalDoctor = await _context.HospitalDoctors
                .Include(h => h.Doctor)
                .Include(h => h.Hospital)
                .FirstOrDefaultAsync(m => m.HospitalId == hospitalId && m.DoctorId == doctorId);
            if (hospitalDoctor == null)
            {
                return NotFound();
            }

            return View(hospitalDoctor);
        }

        // POST: HospitalDoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 hospitalId, Int32 doctorId)
        {
            var hospitalDoctor = await _context.HospitalDoctors.SingleOrDefaultAsync(x => x.HospitalId == hospitalId && x.DoctorId == doctorId);
            _context.HospitalDoctors.Remove(hospitalDoctor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { hospitalId = hospitalId });
        }
    }
}
