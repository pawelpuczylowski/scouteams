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
    public class ChoragiewsController : Controller
    {
        private readonly ScouTDBContext _context;

        public ChoragiewsController(ScouTDBContext context)
        {
            _context = context;
        }

        // GET: Choragiews
        public async Task<IActionResult> Index(int id)
        {
            if (id == 0) 
            {
                return RedirectToAction("ShowKwateraGlowna", "Home");
            }
            ViewData["KwateraGlownaID"] = id;
            ViewData["TypeOrganization"] = TypeOrganization.Choragiew;
            var scouTDBContext = await _context.Choragiews.Where(c => c.KwateraGlownaId == id).ToListAsync();
            return View(scouTDBContext);
        }

        // GET: Choragiews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var choragiew = await _context.Choragiews
                .Include(c => c.KwateraGlowna)
                .FirstOrDefaultAsync(m => m.ChoragiewId == id);
            if (choragiew == null)
            {
                return NotFound();
            }

            return View(choragiew);
        }

        // GET: Choragiews/Create
        public IActionResult Create(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            Choragiew choragiew = new Choragiew();
            choragiew.KwateraGlownaId = id;
            return View(choragiew);
        }

        // POST: Choragiews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChoragiewId,Name,KwateraGlownaId")] Choragiew choragiew)
        {
            if (ModelState.IsValid)
            {
                _context.Add(choragiew);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = choragiew.KwateraGlownaId });
            }
            return View(choragiew);
        }

        // GET: Choragiews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var choragiew = await _context.Choragiews.FindAsync(id);
            if (choragiew == null)
            {
                return NotFound();
            }
            return View(choragiew);
        }

        // POST: Choragiews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChoragiewId,Name,KwateraGlownaId")] Choragiew choragiew)
        {
            if (id != choragiew.ChoragiewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(choragiew);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoragiewExists(choragiew.ChoragiewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = choragiew.KwateraGlownaId });
            }
            return View(choragiew);
        }

        // GET: Choragiews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var choragiew = await _context.Choragiews
                .Include(c => c.KwateraGlowna)
                .FirstOrDefaultAsync(m => m.ChoragiewId == id);
            if (choragiew == null)
            {
                return NotFound();
            }

            return View(choragiew);
        }

        // POST: Choragiews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var choragiew = await _context.Choragiews.FindAsync(id);
            //if (choragiew == null)
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //try
            //{
            //    choragiew.KwateraGlowna = null;
            //    choragiew.KwateraGlownaId = 0;
            //    _context.Update(choragiew);
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ChoragiewExists(choragiew.ChoragiewId))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            var choragiew = await _context.Choragiews.FindAsync(id);
            _context.Choragiews.Remove(choragiew);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = choragiew.KwateraGlownaId });
        }

        private bool ChoragiewExists(int id)
        {
            return _context.Choragiews.Any(e => e.ChoragiewId == id);
        }
    }
}
