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
    public class WardStaffsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardStaffsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WardStaffs
        public async Task<IActionResult> Index(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;
            var wardStaffs = await this._context.WardStaffs
                .Include(w => w.Ward)
                .Where(x => x.Ward.Id == wardId)
                .ToListAsync();

            return View(wardStaffs);
        }

        // GET: WardStaffs/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaffs
                .Include(w => w.Ward)
                .FirstOrDefaultAsync(m => m.WardStaffId == id);
            if (wardStaff == null)
            {
                return NotFound();
            }

            return View(wardStaff);
        }

        // GET: WardStaffs/Create
        public async Task<IActionResult> Create(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            this.ViewBag.Ward = ward;
            return View(new WardStaffModel());
        }

        // POST: WardStaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? wardId, WardStaffModel model)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                var wardStaff = new WardStaff
                {
                    WardId = ward.Id,
                    Ward = ward,
                    Name = model.Name,
                    Position = model.Position
                    
                };
                _context.Add(wardStaff);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new {wardId = ward.Id});
            }
            this.ViewBag.Ward = ward;
            return View(model);
        }

        // GET: WardStaffs/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaffs.Include(x=>x.Ward).SingleOrDefaultAsync(x=>x.WardStaffId == id);
            if (wardStaff == null)
            {
                return NotFound();
            }
            var model = new WardStaffModel
            {
                Name = wardStaff.Name,
                Position = wardStaff.Position
            };

            this.ViewBag.Ward = wardStaff.Ward;
            return View(model);
        }

        // POST: WardStaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, WardStaffModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var wardStaff = await this._context.WardStaffs
                .SingleOrDefaultAsync(x => x.WardStaffId == id);

            if (wardStaff == null)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                wardStaff.Name = model.Name;
                wardStaff.Position = model.Position;
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = wardStaff.WardId });
            }
            this.ViewBag.Ward = wardStaff.Ward;
            return View(model);
        }

        // GET: WardStaffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaffs
                .Include(w => w.Ward)
                .FirstOrDefaultAsync(m => m.WardStaffId == id);
            if (wardStaff == null)
            {
                return NotFound();
            }

            return View(wardStaff);
        }

        // POST: WardStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wardStaff = await _context.WardStaffs.SingleOrDefaultAsync(x=>x.WardStaffId==id);
            this._context.WardStaffs.Remove(wardStaff);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { wardId = wardStaff.WardId });
        }
    }
}
