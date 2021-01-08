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
    public class DruzynasController : Controller
    {
        private readonly ScouTDBContext _context;

        public DruzynasController(ScouTDBContext context)
        {
            _context = context;
        }

        // GET: Druzynas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Druzynas.ToListAsync());
        }

        // GET: Druzynas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var druzyna = await _context.Druzynas
                .FirstOrDefaultAsync(m => m.HufiecId == id);
            if (druzyna == null)
            {
                return NotFound();
            }

            return View(druzyna);
        }

        // GET: Druzynas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Druzynas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DruzynaId,Name,HufiecId")] Druzyna druzyna)
        {
            if (ModelState.IsValid)
            {
                _context.Add(druzyna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(druzyna);
        }

        // GET: Druzynas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var druzyna = await _context.Druzynas.FindAsync(id);
            if (druzyna == null)
            {
                return NotFound();
            }
            return View(druzyna);
        }

        // POST: Druzynas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DruzynaId,Name,HufiecId")] Druzyna druzyna)
        {
            if (id != druzyna.HufiecId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(druzyna);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DruzynaExists(druzyna.HufiecId))
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
            return View(druzyna);
        }

        // GET: Druzynas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var druzyna = await _context.Druzynas
                .FirstOrDefaultAsync(m => m.HufiecId == id);
            if (druzyna == null)
            {
                return NotFound();
            }

            return View(druzyna);
        }

        // POST: Druzynas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var druzyna = await _context.Druzynas.FindAsync(id);
            _context.Druzynas.Remove(druzyna);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DruzynaExists(int id)
        {
            return _context.Druzynas.Any(e => e.HufiecId == id);
        }
    }
}
