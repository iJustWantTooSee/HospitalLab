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
    public class WardStaffsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardStaffsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WardStaffs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WardStaffs.Include(w => w.Ward);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WardStaffs/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public IActionResult Create()
        {
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name");
            return View();
        }

        // POST: WardStaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WardStaffId,Name,Position,WardId")] WardStaff wardStaff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wardStaff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", wardStaff.WardId);
            return View(wardStaff);
        }

        // GET: WardStaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaffs.FindAsync(id);
            if (wardStaff == null)
            {
                return NotFound();
            }
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", wardStaff.WardId);
            return View(wardStaff);
        }

        // POST: WardStaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WardStaffId,Name,Position,WardId")] WardStaff wardStaff)
        {
            if (id != wardStaff.WardStaffId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wardStaff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WardStaffExists(wardStaff.WardStaffId))
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
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", wardStaff.WardId);
            return View(wardStaff);
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
            var wardStaff = await _context.WardStaffs.FindAsync(id);
            _context.WardStaffs.Remove(wardStaff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WardStaffExists(int id)
        {
            return _context.WardStaffs.Any(e => e.WardStaffId == id);
        }
    }
}
