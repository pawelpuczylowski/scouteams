using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScouTeams.Areas.Identity.Data;
using ScouTeams.Data;
using ScouTeams.Models;
using ScouTeams.ViewModels;

namespace ScouTeams.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
        private readonly ScouTDBContext _context;
        private readonly UserManager<Scout> _userManager;
        private string SessionKeyZastepId = "_id";

        public MeetingsController(ScouTDBContext context, UserManager<Scout> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Meetings
        public IActionResult Index(int OrganizationId, TypeOrganization type)
        {
            if (type != TypeOrganization.Zastep)
            {
                return RedirectToAction("ShowAssignments", "Home");
            }
            HttpContext.Session.SetInt32(SessionKeyZastepId, OrganizationId);
            ViewData["OrganizationId"] = OrganizationId;
            return View(_context.Meetings.Where(m => m.ZastepId == OrganizationId).AsEnumerable().Reverse());
        }

        public async Task<IActionResult> CreateMeeting(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie znaleziono tego użytkownika");
            }

            Meeting meeting = new Meeting();
            meeting.ZastepId = id;
            meeting.Date = DateTime.Now;
            meeting.ScoutId = user.Id;

            _context.Add(meeting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CreatePresenceInMeeting));
        }

        public async Task<IActionResult> CreatePresenceInMeeting()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie znaleziono tego użytkownika");
            }
            var meeting = _context.Meetings.ToList().LastOrDefault(m => m.ScoutId == user.Id);
            if(meeting!= null)
            {
                var usersInZastep = _context.UserZasteps.Where(x => x.ZastepId == meeting.ZastepId).ToList();
                foreach (var item in usersInZastep)
                {
                    ScoutPresence scoutPresence = new ScoutPresence();
                    scoutPresence.ScoutId = item.ScoutId;
                    scoutPresence.Meeting = meeting;
                    scoutPresence.MeetingId = meeting.MeetingId;
                    scoutPresence.Presence = Presence.Attending;
                    _context.Add(scoutPresence);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index), new { OrganizationId = HttpContext.Session.GetInt32(SessionKeyZastepId), type = TypeOrganization.Zastep });
        }

        // GET: Meetings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .FirstOrDefaultAsync(m => m.MeetingId == id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }


        // GET: Meetings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }
            ViewData["OrganizationId"] = HttpContext.Session.GetInt32(SessionKeyZastepId);
            ViewData["OrganizationType"] = TypeOrganization.Zastep;

            return View(meeting);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MeetingId,ZastepId,Date,ScoutId")] Meeting meeting)
        {
            if (id != meeting.MeetingId)
            {
                return NotFound();
            }
            var zastepId = HttpContext.Session.GetInt32(SessionKeyZastepId);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meeting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingExists(meeting.MeetingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { OrganizationId = zastepId, type = TypeOrganization.Zastep });
            }
            ViewData["OrganizationId"] = zastepId;
            ViewData["OrganizationType"] = TypeOrganization.Zastep;
            return View(meeting);
        }

        // GET: Meetings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .FirstOrDefaultAsync(m => m.MeetingId == id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

        // POST: Meetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingExists(int id)
        {
            return _context.Meetings.Any(e => e.MeetingId == id);
        }
    }
}
