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
    public class KwateraGlownasController : Controller
    {
        private readonly ScouTDBContext _context;

        public KwateraGlownasController(ScouTDBContext context)
        {
            _context = context;
        }

        // GET: KwateraGlownas
        public async Task<IActionResult> Index()
        {
            return View(await _context.KwateraGlowna.ToListAsync());
        }

        // GET: KwateraGlownas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kwateraGlowna = await _context.KwateraGlowna
                .FirstOrDefaultAsync(m => m.KwateraGlownaId == id);
            if (kwateraGlowna == null)
            {
                return NotFound();
            }

            return View(kwateraGlowna);
        }

        // GET: KwateraGlownas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KwateraGlownas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KwateraGlownaId")] KwateraGlowna kwateraGlowna)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kwateraGlowna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kwateraGlowna);
        }

        // GET: KwateraGlownas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kwateraGlowna = await _context.KwateraGlowna.FindAsync(id);
            if (kwateraGlowna == null)
            {
                return NotFound();
            }
            return View(kwateraGlowna);
        }

        // POST: KwateraGlownas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KwateraGlownaId")] KwateraGlowna kwateraGlowna)
        {
            if (id != kwateraGlowna.KwateraGlownaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kwateraGlowna);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KwateraGlownaExists(kwateraGlowna.KwateraGlownaId))
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
            return View(kwateraGlowna);
        }

        // GET: KwateraGlownas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kwateraGlowna = await _context.KwateraGlowna
                .FirstOrDefaultAsync(m => m.KwateraGlownaId == id);
            if (kwateraGlowna == null)
            {
                return NotFound();
            }

            return View(kwateraGlowna);
        }

        // POST: KwateraGlownas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kwateraGlowna = await _context.KwateraGlowna.FindAsync(id);
            _context.KwateraGlowna.Remove(kwateraGlowna);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KwateraGlownaExists(int id)
        {
            return _context.KwateraGlowna.Any(e => e.KwateraGlownaId == id);
        }
    }
}
