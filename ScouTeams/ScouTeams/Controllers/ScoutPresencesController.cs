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
    public class ScoutPresencesController : Controller
    {
        private readonly ScouTDBContext _context;

        public ScoutPresencesController(ScouTDBContext context)
        {
            _context = context;
        }

        // GET: ScoutPresences
        public async Task<IActionResult> Index()
        {
            var scouTDBContext = _context.ScoutPresence.Include(s => s.Meeting);
            return View(await scouTDBContext.ToListAsync());
        }

        // GET: ScoutPresences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scoutPresence = await _context.ScoutPresence
                .Include(s => s.Meeting)
                .FirstOrDefaultAsync(m => m.ScoutPresenceId == id);
            if (scoutPresence == null)
            {
                return NotFound();
            }

            return View(scoutPresence);
        }

        // GET: ScoutPresences/Create
        public IActionResult Create()
        {
            ViewData["MeetingId"] = new SelectList(_context.Meetings, "MeetingId", "MeetingId");
            return View();
        }

        // POST: ScoutPresences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScoutPresenceId,ScoutId,MeetingId,Presence")] ScoutPresence scoutPresence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scoutPresence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MeetingId"] = new SelectList(_context.Meetings, "MeetingId", "MeetingId", scoutPresence.MeetingId);
            return View(scoutPresence);
        }

        // GET: ScoutPresences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scoutPresence = await _context.ScoutPresence.FindAsync(id);
            if (scoutPresence == null)
            {
                return NotFound();
            }
            ViewData["MeetingId"] = new SelectList(_context.Meetings, "MeetingId", "MeetingId", scoutPresence.MeetingId);
            return View(scoutPresence);
        }

        // POST: ScoutPresences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScoutPresenceId,ScoutId,MeetingId,Presence")] ScoutPresence scoutPresence)
        {
            if (id != scoutPresence.ScoutPresenceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scoutPresence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoutPresenceExists(scoutPresence.ScoutPresenceId))
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
            ViewData["MeetingId"] = new SelectList(_context.Meetings, "MeetingId", "MeetingId", scoutPresence.MeetingId);
            return View(scoutPresence);
        }

        // GET: ScoutPresences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scoutPresence = await _context.ScoutPresence
                .Include(s => s.Meeting)
                .FirstOrDefaultAsync(m => m.ScoutPresenceId == id);
            if (scoutPresence == null)
            {
                return NotFound();
            }

            return View(scoutPresence);
        }

        // POST: ScoutPresences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scoutPresence = await _context.ScoutPresence.FindAsync(id);
            _context.ScoutPresence.Remove(scoutPresence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScoutPresenceExists(int id)
        {
            return _context.ScoutPresence.Any(e => e.ScoutPresenceId == id);
        }
    }
}
