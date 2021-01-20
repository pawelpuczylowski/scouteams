using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScouTeams.Data;
using ScouTeams.Models;
using ScouTeams.ViewModels;

namespace ScouTeams.Controllers
{
    public class FunctionInOrganizationsController : Controller
    {
        private readonly ScouTDBContext _context;

        public FunctionInOrganizationsController(ScouTDBContext context)
        {
            _context = context;
        }

        // GET: FunctionInOrganizations
        public async Task<IActionResult> Index(string scoutId, int OrganizationId, TypeOrganization type)
        {
            switch (type)
            {
                case TypeOrganization.KwateraGlowna:

                    return View(await _context.FunctionInOrganizations.Where(f => f.ScoutId == scoutId && f.ChorągiewId == OrganizationId && f.HufiecId == OrganizationId && f.DruzynaId == OrganizationId && f.ZastepId == OrganizationId).ToListAsync());
                case TypeOrganization.Choragiew:
                    return View(await _context.FunctionInOrganizations.Where(f => f.ScoutId == scoutId && f.ChorągiewId == OrganizationId && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == -1).ToListAsync());
                case TypeOrganization.Hufiec:
                    return View(await _context.FunctionInOrganizations.Where(f => f.ScoutId == scoutId && f.ChorągiewId == -1 && f.HufiecId == OrganizationId && f.DruzynaId == -1 && f.ZastepId == -1).ToListAsync());
                case TypeOrganization.Druzyna:
                    return View(await _context.FunctionInOrganizations.Where(f => f.ScoutId == scoutId && f.ChorągiewId == -1 && f.HufiecId == -1 && f.DruzynaId == OrganizationId && f.ZastepId == -1).ToListAsync());
                case TypeOrganization.Zastep:
                    return View(await _context.FunctionInOrganizations.Where(f => f.ScoutId == scoutId && f.ChorągiewId == -1 && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == OrganizationId).ToListAsync());
                default:
                    return NotFound();
            }
        }

        // GET: FunctionInOrganizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionInOrganization = await _context.FunctionInOrganizations
                .FirstOrDefaultAsync(m => m.FunctionInOrganizationId == id);
            if (functionInOrganization == null)
            {
                return NotFound();
            }

            return View(functionInOrganization);
        }

        // GET: FunctionInOrganizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FunctionInOrganizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FunctionInOrganizationId,ScoutId,FunctionName,ChorągiewId,HufiecId,DruzynaId,ZastepId")] FunctionInOrganization functionInOrganization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(functionInOrganization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(functionInOrganization);
        }

        // GET: FunctionInOrganizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionInOrganization = await _context.FunctionInOrganizations.FindAsync(id);
            if (functionInOrganization == null)
            {
                return NotFound();
            }
            return View(functionInOrganization);
        }

        // POST: FunctionInOrganizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FunctionInOrganizationId,ScoutId,FunctionName,ChorągiewId,HufiecId,DruzynaId,ZastepId")] FunctionInOrganization functionInOrganization)
        {
            if (id != functionInOrganization.FunctionInOrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(functionInOrganization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionInOrganizationExists(functionInOrganization.FunctionInOrganizationId))
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
            return View(functionInOrganization);
        }

        // GET: FunctionInOrganizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionInOrganization = await _context.FunctionInOrganizations
                .FirstOrDefaultAsync(m => m.FunctionInOrganizationId == id);
            if (functionInOrganization == null)
            {
                return NotFound();
            }

            return View(functionInOrganization);
        }

        // POST: FunctionInOrganizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var functionInOrganization = await _context.FunctionInOrganizations.FindAsync(id);
            _context.FunctionInOrganizations.Remove(functionInOrganization);
            await _context.SaveChangesAsync();
            return RedirectToAction("ShowAssignments", "Home");//, new { area = "" });
        }

        private bool FunctionInOrganizationExists(int id)
        {
            return _context.FunctionInOrganizations.Any(e => e.FunctionInOrganizationId == id);
        }
    }
}
