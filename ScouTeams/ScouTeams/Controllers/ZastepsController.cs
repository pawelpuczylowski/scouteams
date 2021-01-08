using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScouTeams.Data;
using ScouTeams.Models;

namespace ScouTeams.Controllers
{
    public class ZastepsController : Controller
    {
        private readonly ScouTDBContext _context;

        public ZastepsController(ScouTDBContext context)
        {
            _context = context;
        }

        // GET: Zasteps
        public async Task<IActionResult> Index()
        {
            var scouTDBContext = _context.Zastep.Include(z => z.Druzyna);
            return View(await scouTDBContext.ToListAsync());
        }

        // GET: Zasteps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zastep = await _context.Zastep
                .Include(z => z.Druzyna)
                .FirstOrDefaultAsync(m => m.ZastepId == id);
            if (zastep == null)
            {
                return NotFound();
            }

            return View(zastep);
        }

        // GET: Zasteps/Create
        public IActionResult Create()
        {
            ViewData["DruzynaId"] = new SelectList(_context.Druzynas, "HufiecId", "HufiecId");
            return View();
        }

        // POST: Zasteps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZastepId,Name,DruzynaId")] Zastep zastep)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zastep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DruzynaId"] = new SelectList(_context.Druzynas, "HufiecId", "HufiecId", zastep.DruzynaId);
            return View(zastep);
        }

        // GET: Zasteps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zastep = await _context.Zastep.FindAsync(id);
            if (zastep == null)
            {
                return NotFound();
            }
            ViewData["DruzynaId"] = new SelectList(_context.Druzynas, "HufiecId", "HufiecId", zastep.DruzynaId);
            return View(zastep);
        }

        // POST: Zasteps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZastepId,Name,DruzynaId")] Zastep zastep)
        {
            if (id != zastep.ZastepId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zastep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZastepExists(zastep.ZastepId))
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
            ViewData["DruzynaId"] = new SelectList(_context.Druzynas, "HufiecId", "HufiecId", zastep.DruzynaId);
            return View(zastep);
        }

        // GET: Zasteps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zastep = await _context.Zastep
                .Include(z => z.Druzyna)
                .FirstOrDefaultAsync(m => m.ZastepId == id);
            if (zastep == null)
            {
                return NotFound();
            }

            return View(zastep);
        }

        // POST: Zasteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zastep = await _context.Zastep.FindAsync(id);
            _context.Zastep.Remove(zastep);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZastepExists(int id)
        {
            return _context.Zastep.Any(e => e.ZastepId == id);
        }
    }
}
