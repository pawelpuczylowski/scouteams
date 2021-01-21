using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public const string SessionKeyType = "_Type";
        public const string SessionKeyOrganization = "_Organization";

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
            var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == user.Id);
            if (functions != null)
            {
                foreach (var function in functions)
                {
                    if (function.DruzynaId == function.ZastepId && function.HufiecId == function.DruzynaId && function.DruzynaId == function.ChorągiewId)
                    {
                        Assignment assignment = new Assignment(TypeOrganization.KwateraGlowna, function.DruzynaId, "Kwatera Główna");
                        assignments.Add(assignment);
                    }
                    else if (function.ChorągiewId != -1)
                    {
                        var myAssignment = await _context.Choragiews.FirstOrDefaultAsync(x => x.ChoragiewId == function.ChorągiewId);
                        Assignment assignment = new Assignment(TypeOrganization.Choragiew, myAssignment.ChoragiewId, myAssignment.Name);
                        assignments.Add(assignment);
                    }
                    else if (function.HufiecId != -1)
                    {
                        var myAssignment = await _context.Hufiecs.FirstOrDefaultAsync(x => x.HufiecId == function.HufiecId);
                        Assignment assignment = new Assignment(TypeOrganization.Hufiec, myAssignment.HufiecId, myAssignment.Name);
                        assignments.Add(assignment);
                    }
                    else if (function.DruzynaId != -1)
                    {
                        var myAssignment = await _context.Druzynas.FirstOrDefaultAsync(x => x.DruzynaId == function.DruzynaId);
                        Assignment assignment = new Assignment(TypeOrganization.Druzyna, myAssignment.DruzynaId, myAssignment.Name);
                        assignments.Add(assignment);
                    }
                }
            }
            try
            {
                var zasteps = _context.UserZasteps.Where(x => x.ScoutId == user.Id);
                if (zasteps != null)
                {
                    foreach (var zastep in zasteps)
                    {
                        Assignment assignment = new Assignment(TypeOrganization.Zastep, zastep.ZastepId, zastep.Zastep.Name);
                        assignments.Add(assignment);
                    }
                }
            }
            catch (Exception)
            {

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
                    if (zastep != null)
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

        public IActionResult Details(int id, TypeOrganization type/*Assignment myAssignment*/)
        {
            switch (type)
            {
                case TypeOrganization.KwateraGlowna:
                    HttpContext.Session.SetString(SessionKeyType, type.ToString());
                    HttpContext.Session.SetInt32(SessionKeyOrganization, id);
                    return RedirectToAction(nameof(ShowKwateraGlowna));
                case TypeOrganization.Choragiew:
                    HttpContext.Session.SetString(SessionKeyType, type.ToString());
                    HttpContext.Session.SetInt32(SessionKeyOrganization, id);
                    return RedirectToAction(nameof(ShowKwateraGlowna));
                case TypeOrganization.Hufiec:
                    HttpContext.Session.SetString(SessionKeyType, type.ToString());
                    HttpContext.Session.SetInt32(SessionKeyOrganization, id);
                    return RedirectToAction(nameof(ShowKwateraGlowna));
                case TypeOrganization.Druzyna:
                    HttpContext.Session.SetString(SessionKeyType, type.ToString());
                    HttpContext.Session.SetInt32(SessionKeyOrganization, id);
                    return RedirectToAction(nameof(ShowKwateraGlowna));
                case TypeOrganization.Zastep:
                    HttpContext.Session.SetString(SessionKeyType, type.ToString());
                    HttpContext.Session.SetInt32(SessionKeyOrganization, id);
                    return RedirectToAction(nameof(ShowKwateraGlowna));
                default:
                    return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> ShowKwateraGlowna(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var type = HttpContext.Session.GetString(SessionKeyType);
            var id = HttpContext.Session.GetInt32(SessionKeyOrganization);
            if (type == null || id == null || type != TypeOrganization.KwateraGlowna.ToString())  
            {
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var tmp = await _context.KwateraGlowna.FirstOrDefaultAsync(x => x.KwateraGlownaId == id);
            if (tmp == null)
            {
                return NotFound($"Unable to load Kwatera Główna with this ID.");
            }
            if (user.KwateraGlowna == null || user.KwateraGlowna.KwateraGlownaId != id)
            {
                return RedirectToAction(nameof(Index));
            }

            var myScouts = new List<ScoutViewModel>();
            var scouts = _userManager.Users.Where(u => u.KwateraGlowna != null && u.KwateraGlowna.KwateraGlownaId == id).ToList();

            foreach (var scout in scouts)
            {
                var scoutViewModel = new ScoutViewModel();
                scoutViewModel.Id = scout.Id;
                scoutViewModel.FirstName = scout.FirstName;
                scoutViewModel.LastName = scout.LastName;

                var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == id && f.HufiecId == id && f.DruzynaId == id && f.ZastepId == id);
                if (functions != null)
                {
                    var tmpList = new List<string>();

                    foreach (var function in functions)
                    {
                        tmpList.Add(function.FunctionName.GetType()
                        .GetMember(function.FunctionName.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName());
                    }

                    scoutViewModel.Functions = string.Join(", ", tmpList);
                }

                scoutViewModel.Email = scout.Email;
                scoutViewModel.DateOfBirth = scout.DateOfBirth;

                var contributions = _context.Contributions.Where(c => c.ScoutId == scout.Id).ToList();
                if (contributions == null || contributions.Count == 0) scoutViewModel.PaidContributions = false;
                else
                {
                    if (contributions.Count() - 1 > MonthDifference(contributions.First().Date)) scoutViewModel.PaidContributions = true;
                    else scoutViewModel.PaidContributions = false;
                }


                scoutViewModel.ScoutDegree = scout.ScoutDegree;
                scoutViewModel.InstructorDegree = scout.InstructorDegree;
                myScouts.Add(scoutViewModel);
            }


            ViewData["TypeOrganization"] = TypeOrganization.KwateraGlowna;
            ViewData["OrganizationID"] = id;

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

            int pageSize = 10;
            return View(await PaginatedList<ScoutViewModel>.CreateAsync(myScouts, pageNumber ?? 1, pageSize));
        }

        public int MonthDifference(DateTime dateTime)
        {
            var now = DateTime.Now;
            return ((now.Month - dateTime.Month) + 12 * (now.Year - dateTime.Year));
        }

        public async Task<IActionResult> ShowScoutsForRecruitment(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var type = HttpContext.Session.GetString(SessionKeyType);
            var id = HttpContext.Session.GetInt32(SessionKeyOrganization);
            if (type == null || id == null)   
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            List<Scout> scouts = new List<Scout>();
            if (type == TypeOrganization.KwateraGlowna.ToString())
            scouts = _userManager.Users.Where(u => u.Recruitment == true && (u.KwateraGlowna == null || u.KwateraGlowna.KwateraGlownaId != id)).ToList();
            else if(type == TypeOrganization.Choragiew.ToString())
            {
                var userInChoragiew = _context.UserChoragiews.Where(x => x.ChoragiewId == id).ToList();
                scouts = _userManager.Users.Where(u => u.Recruitment == true && (!userInChoragiew.Exists(x => x.ScoutId == u.Id))).ToList();

            }
            else if (type == TypeOrganization.Hufiec.ToString())
            {
                var userInHufiec = _context.UserHufiecs.Where(x => x.HufiecId == id).ToList();
                scouts = _userManager.Users.Where(u => u.Recruitment == true && (!userInHufiec.Exists(x => x.ScoutId == u.Id))).ToList();
            }
            else if (type == TypeOrganization.Druzyna.ToString())
            {
                var userInDruzyna = _context.UserDruzynas.Where(x => x.DruzynaId == id).ToList();
                scouts = _userManager.Users.Where(u => u.Recruitment == true && (!userInDruzyna.Exists(x => x.ScoutId == u.Id))).ToList();
            }
            else if (type == TypeOrganization.Zastep.ToString())
            {
                var userInZastep = _context.UserZasteps.Where(x => x.ZastepId == id).ToList();
                scouts = _userManager.Users.Where(u => u.Recruitment == true && (!userInZastep.Exists(x => x.ScoutId == u.Id))).ToList();
            }


            var myScouts = new List<ScoutViewModel>();
            foreach (var scout in scouts)
            {
                var scoutViewModel = new ScoutViewModel();
                scoutViewModel.Id = scout.Id;
                scoutViewModel.FirstName = scout.FirstName;
                scoutViewModel.LastName = scout.LastName;
                scoutViewModel.Email = scout.Email;
                scoutViewModel.DateOfBirth = scout.DateOfBirth;
                scoutViewModel.ScoutDegree = scout.ScoutDegree;
                scoutViewModel.InstructorDegree = scout.InstructorDegree;
                myScouts.Add(scoutViewModel);
            }

            ViewData["TypeOrganization"] = TypeOrganization.KwateraGlowna;
            ViewData["OrganizationID"] = id;

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

            int pageSize = 20;
            return View(await PaginatedList<ScoutViewModel>.CreateAsync(myScouts, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> AddScout(string scoutId, int OrganizationId, TypeOrganization type)
        {
            switch (type)
            {
                case TypeOrganization.KwateraGlowna:

                    var scout = await _userManager.FindByIdAsync(scoutId);
                    if (scout == null)
                    {
                        return NotFound($"Nie znaleziono harcerza.");
                    }

                    var tmp = await _context.KwateraGlowna.FirstOrDefaultAsync(x => x.KwateraGlownaId == OrganizationId);
                    if (tmp == null)
                    {
                        return NotFound($"Nie znaleziono Kwatery Głównej.");
                    }

                    scout.KwateraGlowna = tmp;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ShowScoutsForRecruitment));
                    break;
                case TypeOrganization.Choragiew:

                    break;
                case TypeOrganization.Hufiec:

                    break;
                case TypeOrganization.Druzyna:

                    break;
                case TypeOrganization.Zastep:

                    break;
                default:

                    break;
            }
            //_context.Add(contribution);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddFunctionScout(string scoutId, int OrganizationId, TypeOrganization type)
        {
            switch (type)
            {
                case TypeOrganization.KwateraGlowna:

                    var scout = await _userManager.FindByIdAsync(scoutId);
                    if (scout == null)
                    {
                        return NotFound($"Nie znaleziono harcerza.");
                    }

                    var tmp = await _context.KwateraGlowna.FirstOrDefaultAsync(x => x.KwateraGlownaId == OrganizationId);
                    if (tmp == null)
                    {
                        return NotFound($"Nie znaleziono Kwatery Głównej.");
                    }

                    var scoutViewModel = new ScoutViewModel();
                    scoutViewModel.Id = scout.Id;
                    scoutViewModel.FirstName = scout.FirstName;
                    scoutViewModel.LastName = scout.LastName;

                    var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == tmp.KwateraGlownaId && f.HufiecId == tmp.KwateraGlownaId && f.DruzynaId == tmp.KwateraGlownaId && f.ZastepId == tmp.KwateraGlownaId);
                    if (functions != null)
                    {
                        var tmpList = new List<string>();

                        foreach (var function in functions)
                        {
                            tmpList.Add(function.FunctionName.GetType()
                            .GetMember(function.FunctionName.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName());
                        }

                        scoutViewModel.Functions = string.Join(", ", tmpList);
                    }

                    scoutViewModel.ThisOrganizationId = OrganizationId;
                    scoutViewModel.ThisTypeOrganization = type;

                    scoutViewModel.ScoutDegree = scout.ScoutDegree;
                    scoutViewModel.InstructorDegree = scout.InstructorDegree;

                    return View(scoutViewModel);
                    break;
                case TypeOrganization.Choragiew:

                    break;
                case TypeOrganization.Hufiec:

                    break;
                case TypeOrganization.Druzyna:

                    break;
                case TypeOrganization.Zastep:

                    break;
                default:

                    break;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFunctionScout(ScoutViewModel scoutViewModel)
        {
            switch (scoutViewModel.ThisTypeOrganization)
            {
                case TypeOrganization.KwateraGlowna:

                    if (scoutViewModel.Id == null)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        var scout = await _userManager.FindByIdAsync(scoutViewModel.Id);
                        if (scout == null)
                        {
                            return NotFound($"Nie znaleziono harcerza.");
                        }
                        FunctionInOrganization functionInOrganization = new FunctionInOrganization();
                        functionInOrganization.ScoutId = scout.Id;
                        functionInOrganization.FunctionName = scoutViewModel.functionName;

                        functionInOrganization.ChorągiewId = scoutViewModel.ThisOrganizationId;
                        functionInOrganization.HufiecId = scoutViewModel.ThisOrganizationId;
                        functionInOrganization.DruzynaId = scoutViewModel.ThisOrganizationId;
                        functionInOrganization.ZastepId = scoutViewModel.ThisOrganizationId;

                        _context.Add(functionInOrganization);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    break;
                case TypeOrganization.Choragiew:

                    break;
                case TypeOrganization.Hufiec:

                    break;
                case TypeOrganization.Druzyna:

                    break;
                case TypeOrganization.Zastep:

                    break;
                default:
                    return NotFound();
            }
            return View(scoutViewModel);
        }

        public async Task<IActionResult> EditScoutDegree(string scoutId)
        {
            var scout = await _userManager.FindByIdAsync(scoutId);
            if (scout == null)
            {
                return NotFound($"Nie znaleziono harcerza.");
            }

            var scoutViewModel = new ScoutViewModel();
            scoutViewModel.Id = scout.Id;
            scoutViewModel.FirstName = scout.FirstName;
            scoutViewModel.LastName = scout.LastName;
            scoutViewModel.ScoutDegree = scout.ScoutDegree;
            scoutViewModel.InstructorDegree = scout.InstructorDegree;

            return View(scoutViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditScoutDegree(ScoutViewModel scoutViewModel)
        {
            if (scoutViewModel.Id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var scout = await _userManager.FindByIdAsync(scoutViewModel.Id);
                if (scout == null)
                {
                    return NotFound($"Nie znaleziono harcerza.");
                }

                scout.ScoutDegree = scoutViewModel.ScoutDegree;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scoutViewModel);
        }

        public async Task<IActionResult> EditInstructorDegree(string scoutId)
        {
            var scout = await _userManager.FindByIdAsync(scoutId);
            if (scout == null)
            {
                return NotFound($"Nie znaleziono harcerza.");
            }

            var scoutViewModel = new ScoutViewModel();
            scoutViewModel.Id = scout.Id;
            scoutViewModel.FirstName = scout.FirstName;
            scoutViewModel.LastName = scout.LastName;
            scoutViewModel.ScoutDegree = scout.ScoutDegree;
            scoutViewModel.InstructorDegree = scout.InstructorDegree;

            return View(scoutViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstructorDegree(ScoutViewModel scoutViewModel)
        {
            if (scoutViewModel.Id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var scout = await _userManager.FindByIdAsync(scoutViewModel.Id);
                if (scout == null)
                {
                    return NotFound($"Nie znaleziono harcerza.");
                }

                scout.InstructorDegree = scoutViewModel.InstructorDegree;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scoutViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
