using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScouTeams.Data;
using ScouTeams.Models;
using ScouTeams.ViewModels;

namespace ScouTeams.Controllers
{
    [Authorize]
    public class ContributionsController : Controller
    {
        private readonly ScouTDBContext _context;
        private string SessionKeyType;

        public ContributionsController(ScouTDBContext context)
        {
            _context = context;
        }

        // GET: Contributions
        public IActionResult Index(string scoutId, TypeOrganization type)
        {
            if (string.IsNullOrEmpty(scoutId))
            {
                return RedirectToAction("ShowAssignments", "Home");
            }
            HttpContext.Session.SetString(SessionKeyType, type.ToString());
            ViewData["TypeOrganization"] = type;
            ViewData["ScoutID"] = scoutId;
            return View(_context.Contributions.Where(c => c.ScoutId == scoutId).AsEnumerable().Reverse());
        }

        // GET: Contributions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribution = await _context.Contributions
                .FirstOrDefaultAsync(m => m.ContributionId == id);
            if (contribution == null)
            {
                return NotFound();
            }

            return View(contribution);
        }

        // GET: Contributions/Create
        public IActionResult Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contribution = new Contribution();
            contribution.ScoutId = id;

            ViewData["ScoutId"] = id;
            var type = HttpContext.Session.GetString(SessionKeyType);
            if (type == TypeOrganization.KwateraGlowna.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.KwateraGlowna;
            }
            else if (type == TypeOrganization.Choragiew.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Choragiew;
            }
            else if (type == TypeOrganization.Hufiec.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Hufiec;
            }
            else if (type == TypeOrganization.Druzyna.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Druzyna;
            }
            else if (type == TypeOrganization.Zastep.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Zastep;
            }

            return View(contribution);
        }

        // POST: Contributions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContributionId,ScoutId,Amount,Date")] Contribution contribution)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contribution);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScoutId"] = contribution.ScoutId;
            var type = HttpContext.Session.GetString(SessionKeyType);
            if (type == TypeOrganization.KwateraGlowna.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.KwateraGlowna;
            }
            else if (type == TypeOrganization.Choragiew.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Choragiew;
            }
            else if (type == TypeOrganization.Hufiec.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Hufiec;
            }
            else if (type == TypeOrganization.Druzyna.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Druzyna;
            }
            else if (type == TypeOrganization.Zastep.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Zastep;
            }
            return View(contribution);
        }

        // GET: Contributions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribution = await _context.Contributions.FindAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }

            ViewData["ScoutId"] = contribution.ScoutId;
            var type = HttpContext.Session.GetString(SessionKeyType);
            if (type == TypeOrganization.KwateraGlowna.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.KwateraGlowna;
            }
            else if (type == TypeOrganization.Choragiew.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Choragiew;
            }
            else if (type == TypeOrganization.Hufiec.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Hufiec;
            }
            else if (type == TypeOrganization.Druzyna.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Druzyna;
            }
            else if (type == TypeOrganization.Zastep.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Zastep;
            }
            return View(contribution);
        }

        // POST: Contributions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContributionId,ScoutId,Amount,Date")] Contribution contribution)
        {
            if (id != contribution.ContributionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contribution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContributionExists(contribution.ContributionId))
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

            ViewData["ScoutId"] = contribution.ScoutId;
            var type = HttpContext.Session.GetString(SessionKeyType);
            if (type == TypeOrganization.KwateraGlowna.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.KwateraGlowna;
            }
            else if (type == TypeOrganization.Choragiew.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Choragiew;
            }
            else if (type == TypeOrganization.Hufiec.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Hufiec;
            }
            else if (type == TypeOrganization.Druzyna.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Druzyna;
            }
            else if (type == TypeOrganization.Zastep.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Zastep;
            }
            return View(contribution);
        }

        // GET: Contributions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribution = await _context.Contributions
                .FirstOrDefaultAsync(m => m.ContributionId == id);
            if (contribution == null)
            {
                return NotFound();
            }

            return View(contribution);
        }

        // POST: Contributions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contribution = await _context.Contributions.FindAsync(id);
            _context.Contributions.Remove(contribution);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContributionExists(int id)
        {
            return _context.Contributions.Any(e => e.ContributionId == id);
        }
    }
}
