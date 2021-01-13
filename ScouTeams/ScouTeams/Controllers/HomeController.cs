using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private int OrganizationId;

        public HomeController(SignInManager<Scout> signInManager, ILogger<HomeController> logger, UserManager<Scout> userManager, ScouTDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            OrganizationId = -1;
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
                    var zastep = await _context.Zastep.FirstOrDefaultAsync(z => z.ZastepId == m.ZastepId);
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
                        var zastep = await _context.Zastep.FirstOrDefaultAsync(z => z.ZastepId == m.ZastepId);
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
                    OrganizationId = myAssignment.OrganizationId;
                    RedirectToAction(nameof(ShowKwateraGlowna));
                    break;
                case TypeOrganization.Choragiew:
                    OrganizationId = myAssignment.OrganizationId;
                    RedirectToAction(nameof(ShowKwateraGlowna));
                    break;
                case TypeOrganization.Hufiec:
                    OrganizationId = myAssignment.OrganizationId;
                    RedirectToAction(nameof(ShowKwateraGlowna));//, new { id = myAssignment.OrganizationId });
                    break;
                case TypeOrganization.Druzyna:
                    OrganizationId = myAssignment.OrganizationId;
                    RedirectToAction(nameof(ShowKwateraGlowna));
                    break;
                case TypeOrganization.Zastep:
                    OrganizationId = myAssignment.OrganizationId;
                    RedirectToAction(nameof(ShowKwateraGlowna));
                    break;
                default:
                    RedirectToAction(nameof(Index));
                    break;
            }
        }

        public async Task<IActionResult> ShowKwateraGlowna(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            if (OrganizationId < 0) 
            {
                RedirectToAction(nameof(Index));
            }

            var id = OrganizationId;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var tmp = await _context.KwateraGlowna.FirstOrDefaultAsync(x => x.KwateraGlownaId == id);
            if (tmp == null)
            {
                return NotFound();
            }
            if(user.KwateraGlowna.KwateraGlownaId != id)
            {
                RedirectToAction(nameof(Index));
            }            

            var myScouts = new List<ScoutViewModel>();
            foreach (var scout in tmp.Scouts)
            {
                var scoutViewModel = new ScoutViewModel();
                scoutViewModel.FirstName = scout.FirstName;
                scoutViewModel.LastName = scout.LastName;

                if(scout.FunctionInOrganizations != null)
                {
                    var tmpList = new List<FunctionName>();

                    foreach (var function in scout.FunctionInOrganizations)
                    {
                        if (function.ChorągiewId == id && function.HufiecId == id && function.DruzynaId == id && function.ZastepId == id)
                        {
                            tmpList.Add(function.FunctionName);
                        }
                    }
                    scoutViewModel.Functions = string.Join(", ", tmpList);
                }
                scoutViewModel.Email = scout.Email;
                scoutViewModel.DateOfBirth = scout.DateOfBirth;

                if (scout.Contributions != null || scout.Contributions.Count - 1 < MonthDifference(scout.Contributions.First().Date)) scoutViewModel.PaidContributions = true;
                else scoutViewModel.PaidContributions = false;
                
                scoutViewModel.ScoutDegree = scout.ScoutDegree;
                scoutViewModel.InstructorDegree = scout.InstructorDegree;
                myScouts.Add(scoutViewModel);
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                myScouts = myScouts.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper()) || s.FirstName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    myScouts = myScouts.OrderByDescending(s => s.LastName).ToList();
                    break;
                case "Date":
                    myScouts = myScouts.OrderBy(s => s.DateOfBirth).ToList();
                    break;
                case "date_desc":
                    myScouts = myScouts.OrderByDescending(s => s.DateOfBirth).ToList();
                    break;
                default:
                    myScouts = myScouts.OrderBy(s => s.LastName).ToList();
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<ScoutViewModel>.CreateAsync(myScouts, pageNumber ?? 1, pageSize));
        
            //return View(myScouts);
        }

        public int MonthDifference(DateTime dateTime)
        {
            var now = DateTime.Now;
            return ((now.Month - dateTime.Month) + 12 * (now.Year - dateTime.Year));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
