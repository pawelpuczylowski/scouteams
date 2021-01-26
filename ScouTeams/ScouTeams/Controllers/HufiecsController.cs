using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScouTeams.Data;
using ScouTeams.Models;
using ScouTeams.ViewModels;

namespace ScouTeams.Controllers
{
    [Authorize]
    public class HufiecsController : Controller
    {
        private readonly ScouTDBContext _context;

        public HufiecsController(ScouTDBContext context)
        {
            _context = context;
        }

        // GET: Hufiecs
        public async Task<IActionResult> Index(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ShowChoragiew", "Home");
            }
            ViewData["OrganizationID"] = id;
            ViewData["TypeOrganization"] = TypeOrganization.Hufiec;
            var scouTDBContext = await _context.Hufiecs.Where(c => c.ChoragiewId == id).ToListAsync();
            return View(scouTDBContext);
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
        public IActionResult Create(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Hufiec hufiec = new Hufiec();
            hufiec.ChoragiewId = id;
            return View(hufiec);
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
            return RedirectToAction(nameof(Index), new { id = hufiec.ChoragiewId });
        }

        private bool HufiecExists(int id)
        {
            return _context.Hufiecs.Any(e => e.HufiecId == id);
        }
    }
}
