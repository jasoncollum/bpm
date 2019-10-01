using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bpm.Data;
using bpm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace bpm.Controllers
{
    [Authorize]
    public class EntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EntriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Entries
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

            var entries = await _context.Entry.Where(e => e.ApplicationUserId == user.Id).ToListAsync();

            return View(entries.OrderByDescending(ent => ent.DateEntered));
        }

        // GET: Last 7 Days Entries
        public async Task<IActionResult> Last7Days()
        {
            var user = await GetCurrentUserAsync();
            var entries = await _context.Entry.Where(e => e.ApplicationUserId == user.Id &&
            e.DateEntered > DateTime.Now.AddDays(-7)).ToListAsync();

            if (entries.Count != 0)
            {
                double SysAverage = entries.Average(e => e.Systolic);
                var sysAvg = Math.Round(SysAverage, 0, MidpointRounding.AwayFromZero);
                double DiaAverage = entries.Average(e => e.Diastolic);
                var diaAvg = Math.Round(DiaAverage, 0, MidpointRounding.AwayFromZero);

                ViewBag.Avg = $"7 Day Average: {sysAvg}/{diaAvg}";
                return View("Days", entries.OrderByDescending(ent => ent.DateEntered));
            }
            else
            {
                ViewBag.Avg = $"There have been no entries in the last 7 days";
                return View("Days", entries.OrderByDescending(ent => ent.DateEntered));
            }
        }

        // GET: Last 30 Days Entries
        public async Task<IActionResult> Last30Days()
        {
            var user = await GetCurrentUserAsync();
            var entries = await _context.Entry.Where(e => e.ApplicationUserId == user.Id &&
            e.DateEntered > DateTime.Now.AddDays(-30)).ToListAsync();

            if (entries.Count != 0)
            {
                double SysAverage = entries.Average(e => e.Systolic);
                var sysAvg = Math.Round(SysAverage, 0, MidpointRounding.AwayFromZero);
                double DiaAverage = entries.Average(e => e.Diastolic);
                var diaAvg = Math.Round(DiaAverage, 0, MidpointRounding.AwayFromZero);

                ViewBag.Avg = $"30 Day Average: {sysAvg}/{diaAvg}";
                return View("Days", entries.OrderByDescending(ent => ent.DateEntered));
            }
            else
            {
                ViewBag.Avg = $"There have been no entries in the last 30 days";
                return View("Days", entries.OrderByDescending(ent => ent.DateEntered));
            }
        }

        // GET: Last 12 Months Entries
        public async Task<IActionResult> Last12Months()
        {
            var user = await GetCurrentUserAsync();
            var entries = await _context.Entry.Where(e => e.ApplicationUserId == user.Id &&
            e.DateEntered > DateTime.Now.AddMonths(-12)).ToListAsync();

            if (entries.Count != 0)
            {
                return View("Months", entries);
            }
            else
            {
                ViewBag.Avg = $"There have been no entries in the last 12 Months";
                return View("Months", entries.OrderByDescending(ent => ent.DateEntered));
            }
        }

        // GET: Entries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entry
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }

        // GET: Entries/Create
        public IActionResult Create()
        {
            //ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Entries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateEntered,Systolic,Diastolic,Pulse,Weight,Notes,ApplicationUserId")] Entry entry)
        {
            var user = await GetCurrentUserAsync();

            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                entry.ApplicationUserId = user.Id;

                _context.Add(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", entry.ApplicationUserId);
            return View(entry);
        }

        // GET: Entries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entry.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", entry.ApplicationUserId);
            return View(entry);
        }

        // POST: Entries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateEntered,Systolic,Diastolic,Pulse,Weight,Notes,ApplicationUserId")] Entry entry)
        {
            if (id != entry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryExists(entry.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", entry.ApplicationUserId);
            return View(entry);
        }

        // GET: Entries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entry
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }

        // POST: Entries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entry = await _context.Entry.FindAsync(id);
            _context.Entry.Remove(entry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntryExists(int id)
        {
            return _context.Entry.Any(e => e.Id == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
