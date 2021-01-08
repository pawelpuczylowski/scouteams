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
    public class HufiecsController : Controller
    {
        private readonly ScouTDBContext _context;

        public HufiecsController(ScouTDBContext context)
        {
            _context = context;
        }

        // GET: Hufiecs
        public async Task<IActionResult> Index()
        {
            var scouTDBContext = _context.Hufiecs.Include(h => h.Choragiew);
            return View(await scouTDBContext.ToListAsync());
        }

        // GET: Hufiecs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hufiec = await _context.Hufiecs
                .Include(h => h.Choragiew)
                .FirstOrDefaultAsync(m => m.HufiecId == id);
            if (hufiec == null)
            {
                return NotFound();
            }

            return View(hufiec);
        }

        // GET: Hufiecs/Create
        public IActionResult Create()
        {
            ViewData["ChoragiewId"] = new SelectList(_context.Choragiews, "ChoragiewId", "ChoragiewId");
            return View();
        }

        // POST: Hufiecs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HufiecId,Name,ChoragiewId")] Hufiec hufiec)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hufiec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChoragiewId"] = new SelectList(_context.Choragiews, "ChoragiewId", "ChoragiewId", hufiec.ChoragiewId);
            return View(hufiec);
        }

        // GET: Hufiecs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hufiec = await _context.Hufiecs.FindAsync(id);
            if (hufiec == null)
            {
                return NotFound();
            }
            ViewData["ChoragiewId"] = new SelectList(_context.Choragiews, "ChoragiewId", "ChoragiewId", hufiec.ChoragiewId);
            return View(hufiec);
        }

        // POST: Hufiecs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HufiecId,Name,ChoragiewId")] Hufiec hufiec)
        {
            if (id != hufiec.HufiecId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hufiec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HufiecExists(hufiec.HufiecId))
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
            ViewData["ChoragiewId"] = new SelectList(_context.Choragiews, "ChoragiewId", "ChoragiewId", hufiec.ChoragiewId);
            return View(hufiec);
        }

        // GET: Hufiecs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hufiec = await _context.Hufiecs
                .Include(h => h.Choragiew)
                .FirstOrDefaultAsync(m => m.HufiecId == id);
            if (hufiec == null)
            {
                return NotFound();
            }

            return View(hufiec);
        }

        // POST: Hufiecs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hufiec = await _context.Hufiecs.FindAsync(id);
            _context.Hufiecs.Remove(hufiec);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HufiecExists(int id)
        {
            return _context.Hufiecs.Any(e => e.HufiecId == id);
        }
    }
}
