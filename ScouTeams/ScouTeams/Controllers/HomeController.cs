using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScouTeams.Areas.Identity.Data;
using ScouTeams.Data;
using ScouTeams.Models;
using ScouTeams.ViewModels;

namespace ScouTeams.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<Scout> _userManager;
        private readonly SignInManager<Scout> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ScouTDBContext _context;


        public HomeController(SignInManager<Scout> signInManager, ILogger<HomeController> logger, UserManager<Scout> userManager, ScouTDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> ShowAssignments()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            List<Assignment> assignments = new List<Assignment>();
            if(user.FunctionInOrganizations != null)
            {
                foreach (var function in user.FunctionInOrganizations)
                {
                    var kwateraGlowna = user.KwateraGlowna.KwateraGlownaId;
                    if (kwateraGlowna == function.ZastepId && kwateraGlowna == function.DruzynaId && kwateraGlowna == function.HufiecId && kwateraGlowna == function.ChorągiewId)
                    {
                        Assignment assignment = new Assignment(TypeOrganization.KwateraGlowna, kwateraGlowna, "Kwatera Główna");
                        assignments.Add(assignment);
                    }
                    if (function.ChorągiewId != -1)
                    {
                        var myAssignment = user.Choragiews.FirstOrDefault(x => x.ChoragiewId == function.ChorągiewId);
                        Assignment assignment = new Assignment(TypeOrganization.Choragiew, myAssignment.ChoragiewId, myAssignment.Choragiew.Name);
                        assignments.Add(assignment);
                    }
                    else if (function.HufiecId != -1)
                    {
                        var myAssignment = user.Hufiecs.FirstOrDefault(x => x.HufiecId == function.HufiecId);
                        Assignment assignment = new Assignment(TypeOrganization.Hufiec, myAssignment.HufiecId, myAssignment.Hufiec.Name);
                        assignments.Add(assignment);
                    }
                    else if (function.DruzynaId != -1)
                    {
                        var myAssignment = user.Druzynas.FirstOrDefault(x => x.DruzynaId == function.DruzynaId);
                        Assignment assignment = new Assignment(TypeOrganization.Druzyna, myAssignment.DruzynaId, myAssignment.Druzyna.Name);
                        assignments.Add(assignment);
                    }
                }
            }
            if(user.FunctionInOrganizations != null)
            {
                foreach (var zastep in user.Zasteps)
                {
                    Assignment assignment = new Assignment(TypeOrganization.Zastep, zastep.ZastepId, zastep.Zastep.Name);
                    assignments.Add(assignment);
                }
            }

            return View(assignments);
        }

        public async Task<IActionResult> ShowContributions()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(_context.Contributions.Where(c => c.ScoutId == user.Id).AsEnumerable().Reverse());
        }        

        public async Task<IActionResult> ShowMeetings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            List<MeetingWithPresence> meetingsWithPresence = new List<MeetingWithPresence>();

            var myMeetings = _context.Meetings;

            foreach (var m in myMeetings)
            {
                if (m.ScoutId == user.Id)
                {
                    MeetingWithPresence meetingWithPresence = new MeetingWithPresence();
                    meetingWithPresence.MeetingId = m.MeetingId;
                    var zastep = _context.Zastep.FirstOrDefault(z => z.ZastepId == m.ZastepId);
                    if(zastep != null)
                    {
                        meetingWithPresence.ZastepName = zastep.Name;
                    }
                    meetingWithPresence.Date = m.Date;
                    meetingWithPresence.Presence = Presence.Attending;
                    meetingsWithPresence.Add(meetingWithPresence);
                }
                foreach (var scoutPresences in m.ScoutPresences)
                {
                    if (scoutPresences.ScoutId == user.Id)
                    {
                        MeetingWithPresence meetingWithPresence = new MeetingWithPresence();
                        meetingWithPresence.MeetingId = m.MeetingId;
                        var zastep = _context.Zastep.FirstOrDefault(z => z.ZastepId == m.ZastepId);
                        if (zastep != null)
                        {
                            meetingWithPresence.ZastepName = zastep.Name;
                        }
                        meetingWithPresence.Date = m.Date;
                        meetingWithPresence.Presence = scoutPresences.Presence;
                        meetingsWithPresence.Add(meetingWithPresence);
                    }
                }
            }

            return View(meetingsWithPresence.AsEnumerable().Reverse());
        }

        public async Task<IActionResult> ShowSkills()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(_context.Skills.Where(s => s.ScoutId == user.Id).AsEnumerable().Reverse());
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public void Details(Assignment myAssignment)
        {
            if (myAssignment == null)
            {
                RedirectToAction(nameof(Index));
            }
            switch (myAssignment.TypeOrganization)
            {
                case TypeOrganization.KwateraGlowna:
                    RedirectToAction(nameof(ShowKwateraGlowna), new { id = myAssignment.OrganizationId });
                    break;
                case TypeOrganization.Choragiew:
                    RedirectToAction(nameof(ShowKwateraGlowna), new { id = myAssignment.OrganizationId });
                    break;
                case TypeOrganization.Hufiec:
                    RedirectToAction(nameof(ShowKwateraGlowna), new { id = myAssignment.OrganizationId });
                    break;
                case TypeOrganization.Druzyna:
                    RedirectToAction(nameof(ShowKwateraGlowna), new { id = myAssignment.OrganizationId });
                    break;
                case TypeOrganization.Zastep:
                    RedirectToAction(nameof(ShowKwateraGlowna), new { id = myAssignment.OrganizationId });
                    break;
                default:
                    RedirectToAction(nameof(Index));
                    break;
            }
        }

        public IActionResult ShowKwateraGlowna(int id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
