using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScouTeams.Areas.Identity.Data;
using ScouTeams.Data;
using ScouTeams.Models;
using ScouTeams.Services;
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
        private IMailService _mailService;
        public const string SessionKeyType = "_Type";
        public const string SessionKeyOrganization = "_Organization";

        public HomeController(SignInManager<Scout> signInManager, ILogger<HomeController> logger, UserManager<Scout> userManager, ScouTDBContext context, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _mailService = mailService;
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

        public IActionResult Details(int id, TypeOrganization type)
        {
            HttpContext.Session.SetString(SessionKeyType, type.ToString());
            HttpContext.Session.SetInt32(SessionKeyOrganization, id);
            switch (type)
            {
                case TypeOrganization.KwateraGlowna:
                    return RedirectToAction(nameof(ShowKwateraGlowna));
                case TypeOrganization.Choragiew:
                    return RedirectToAction(nameof(ShowChoragiew));
                case TypeOrganization.Hufiec:
                    return RedirectToAction(nameof(ShowHufiec));
                case TypeOrganization.Druzyna:
                    return RedirectToAction(nameof(ShowDruzyna));
                case TypeOrganization.Zastep:
                    return RedirectToAction(nameof(ShowZastep));
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
            var myFunctionInKG = await _context.FunctionInOrganizations.FirstOrDefaultAsync(f => f.ScoutId == user.Id && f.ChorągiewId == id && f.HufiecId == id && f.DruzynaId == id && f.ZastepId == id);
            if (myFunctionInKG == null)
            {
                return RedirectToAction(nameof(ShowAssignments));
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
            ViewData["OrganizationName"] = "Kwatera Główna";

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

            ViewData["TypeOrganization"] = "";
            List<Scout> scouts = new List<Scout>();
            if (type == TypeOrganization.KwateraGlowna.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.KwateraGlowna;
                scouts = _userManager.Users.Where(u => u.Recruitment == true && (u.KwateraGlowna == null || u.KwateraGlowna.KwateraGlownaId != id)).ToList();
            }
            else if (type == TypeOrganization.Choragiew.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Choragiew;
                var userInChoragiew = _context.UserChoragiews.Where(x => x.ChoragiewId == id).ToList();
                scouts = _userManager.Users.ToList().Where(u => u.Recruitment == true && (!userInChoragiew.Exists(x => x.ScoutId == u.Id))).ToList();
            }
            else if (type == TypeOrganization.Hufiec.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Hufiec;
                var userInHufiec = _context.UserHufiecs.Where(x => x.HufiecId == id).ToList();
                scouts = _userManager.Users.ToList().Where(u => u.Recruitment == true && (!userInHufiec.Exists(x => x.ScoutId == u.Id))).ToList();
            }
            else if (type == TypeOrganization.Druzyna.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Druzyna;
                var userInDruzyna = _context.UserDruzynas.Where(x => x.DruzynaId == id).ToList();
                scouts = _userManager.Users.ToList().Where(u => u.Recruitment == true && (!userInDruzyna.Exists(x => x.ScoutId == u.Id))).ToList();
            }
            else if (type == TypeOrganization.Zastep.ToString())
            {
                ViewData["TypeOrganization"] = TypeOrganization.Zastep;
                var userInZastep = _context.UserZasteps.Where(x => x.ZastepId == id).ToList();
                scouts = _userManager.Users.ToList().Where(u => u.Recruitment == true && (!userInZastep.Exists(x => x.ScoutId == u.Id))).ToList();
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
            var scout = await _userManager.FindByIdAsync(scoutId);
            if (scout == null)
            {
                return NotFound($"Nie znaleziono harcerza.");
            }
            switch (type)
            {
                case TypeOrganization.KwateraGlowna:
                    var tmpKG = await _context.KwateraGlowna.FirstOrDefaultAsync(x => x.KwateraGlownaId == OrganizationId);
                    if (tmpKG == null)
                    {
                        return NotFound($"Nie znaleziono Kwatery Głównej.");
                    }

                    scout.KwateraGlowna = tmpKG;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ShowScoutsForRecruitment));

                case TypeOrganization.Choragiew:
                    var tmpC = await _context.Choragiews.FirstOrDefaultAsync(x => x.ChoragiewId == OrganizationId);
                    if (tmpC == null)
                    {
                        return NotFound($"Nie znaleziono tej chorągwi.");
                    }
                    UserChoragiew userChoragiew = new UserChoragiew();
                    userChoragiew.ChoragiewId = OrganizationId;
                    userChoragiew.ScoutId = scoutId;
                    _context.Add(userChoragiew);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ShowScoutsForRecruitment));

                case TypeOrganization.Hufiec:
                    var tmpH = await _context.Hufiecs.FirstOrDefaultAsync(x => x.HufiecId == OrganizationId);
                    if (tmpH == null)
                    {
                        return NotFound($"Nie znaleziono tego hufca.");
                    }
                    UserHufiec userHufiec = new UserHufiec();
                    userHufiec.HufiecId = OrganizationId;
                    userHufiec.ScoutId = scoutId;
                    _context.Add(userHufiec);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ShowScoutsForRecruitment));

                case TypeOrganization.Druzyna:
                    var tmpD = await _context.Druzynas.FirstOrDefaultAsync(x => x.DruzynaId == OrganizationId);
                    if (tmpD == null)
                    {
                        return NotFound($"Nie znaleziono tej drużyny.");
                    }
                    UserDruzyna userDruzyna = new UserDruzyna();
                    userDruzyna.DruzynaId= OrganizationId;
                    userDruzyna.ScoutId = scoutId;
                    _context.Add(userDruzyna);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ShowScoutsForRecruitment));

                case TypeOrganization.Zastep:
                    var tmpZ = await _context.Zastep.FirstOrDefaultAsync(x => x.ZastepId == OrganizationId);
                    if (tmpZ == null)
                    {
                        return NotFound($"Nie znaleziono tego zastępu.");
                    }
                    UserZastep userZastep = new UserZastep();
                    userZastep.ZastepId = OrganizationId;
                    userZastep.ScoutId = scoutId;
                    _context.Add(userZastep);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ShowScoutsForRecruitment));

                default:
                    return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> DeleteScout(string scoutId, int OrganizationId, TypeOrganization type)
        {
            var scout = await _userManager.FindByIdAsync(scoutId);
            if (scout == null)
            {
                return NotFound($"Nie znaleziono harcerza.");
            }
            switch (type)
            {
                case TypeOrganization.KwateraGlowna:
                    var tmp = await _context.KwateraGlowna.FirstOrDefaultAsync(x => x.KwateraGlownaId == OrganizationId);
                    if (tmp == null)
                    {
                        return NotFound($"Nie znaleziono Kwatery Głównej.");
                    }

                    scout.KwateraGlowna = null;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ShowKwateraGlowna));

                case TypeOrganization.Choragiew:
                    var tmpC = await _context.Choragiews.FirstOrDefaultAsync(x => x.ChoragiewId == OrganizationId);
                    if (tmpC == null)
                    {
                        return NotFound($"Nie znaleziono tej chorągwi.");
                    }
                    var userChoragiew = await _context.UserChoragiews.FirstOrDefaultAsync(u => u.ChoragiewId == OrganizationId && u.ScoutId == scoutId);
                    if (userChoragiew != null)
                    {
                        _context.UserChoragiews.Remove(userChoragiew);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction(nameof(ShowChoragiew));

                case TypeOrganization.Hufiec:
                    var tmpH = await _context.Hufiecs.FirstOrDefaultAsync(x => x.HufiecId == OrganizationId);
                    if (tmpH == null)
                    {
                        return NotFound($"Nie znaleziono tego hufca.");
                    }
                    var userHufiec = await _context.UserHufiecs.FirstOrDefaultAsync(u => u.HufiecId == OrganizationId && u.ScoutId == scoutId);
                    if (userHufiec != null)
                    {
                        _context.UserHufiecs.Remove(userHufiec);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction(nameof(ShowHufiec));

                case TypeOrganization.Druzyna:
                    var tmpD = await _context.Druzynas.FirstOrDefaultAsync(x => x.DruzynaId == OrganizationId);
                    if (tmpD == null)
                    {
                        return NotFound($"Nie znaleziono tej drużyny.");
                    }
                    var userDruzyna = await _context.UserDruzynas.FirstOrDefaultAsync(u => u.DruzynaId == OrganizationId && u.ScoutId == scoutId);
                    if (userDruzyna != null)
                    {
                        _context.UserDruzynas.Remove(userDruzyna);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction(nameof(ShowDruzyna));

                case TypeOrganization.Zastep:
                    var tmpZ = await _context.Zastep.FirstOrDefaultAsync(x => x.ZastepId == OrganizationId);
                    if (tmpZ == null)
                    {
                        return NotFound($"Nie znaleziono tego zastępu.");
                    }
                    var userZastep = await _context.UserZasteps.FirstOrDefaultAsync(u => u.ZastepId == OrganizationId && u.ScoutId == scoutId);
                    if (userZastep != null)
                    {
                        _context.UserZasteps.Remove(userZastep);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction(nameof(ShowZastep));
                default:
                    return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> AddFunctionScout(string scoutId, int OrganizationId, TypeOrganization type)
        {
            var scout = await _userManager.FindByIdAsync(scoutId);
            if (scout == null)
            {
                return NotFound($"Nie znaleziono harcerza.");
            }
            switch (type)
            {
                case TypeOrganization.KwateraGlowna:
                    var tmpKG = await _context.KwateraGlowna.FirstOrDefaultAsync(x => x.KwateraGlownaId == OrganizationId);
                    if (tmpKG == null)
                    {
                        return NotFound($"Nie znaleziono Kwatery Głównej.");
                    }

                    var scoutViewModelKG = new ScoutViewModel();
                    scoutViewModelKG.Id = scout.Id;
                    scoutViewModelKG.FirstName = scout.FirstName;
                    scoutViewModelKG.LastName = scout.LastName;
                    scoutViewModelKG.ThisOrganizationId = OrganizationId;
                    scoutViewModelKG.ThisTypeOrganization = type;
                    scoutViewModelKG.ScoutDegree = scout.ScoutDegree;
                    scoutViewModelKG.InstructorDegree = scout.InstructorDegree;

                    return View(scoutViewModelKG);

                case TypeOrganization.Choragiew:
                    var tmpC = await _context.Choragiews.FirstOrDefaultAsync(x => x.ChoragiewId == OrganizationId);
                    if (tmpC == null)
                    {
                        return NotFound($"Nie znaleziono tej chorągwi.");
                    }

                    var scoutViewModelC = new ScoutViewModel();
                    scoutViewModelC.Id = scout.Id;
                    scoutViewModelC.FirstName = scout.FirstName;
                    scoutViewModelC.LastName = scout.LastName;
                    scoutViewModelC.ThisOrganizationId = OrganizationId;
                    scoutViewModelC.ThisTypeOrganization = type;
                    scoutViewModelC.ScoutDegree = scout.ScoutDegree;
                    scoutViewModelC.InstructorDegree = scout.InstructorDegree;

                    return View(scoutViewModelC);

                case TypeOrganization.Hufiec:
                    var tmpH = await _context.Hufiecs.FirstOrDefaultAsync(x => x.HufiecId == OrganizationId);
                    if (tmpH == null)
                    {
                        return NotFound($"Nie znaleziono tego hufca.");
                    }

                    var scoutViewModelH = new ScoutViewModel();
                    scoutViewModelH.Id = scout.Id;
                    scoutViewModelH.FirstName = scout.FirstName;
                    scoutViewModelH.LastName = scout.LastName;
                    scoutViewModelH.ThisOrganizationId = OrganizationId;
                    scoutViewModelH.ThisTypeOrganization = type;
                    scoutViewModelH.ScoutDegree = scout.ScoutDegree;
                    scoutViewModelH.InstructorDegree = scout.InstructorDegree;

                    return View(scoutViewModelH);

                case TypeOrganization.Druzyna:
                    var tmpD = await _context.Druzynas.FirstOrDefaultAsync(x => x.DruzynaId == OrganizationId);
                    if (tmpD == null)
                    {
                        return NotFound($"Nie znaleziono tej drużyny.");
                    }

                    var scoutViewModelD = new ScoutViewModel();
                    scoutViewModelD.Id = scout.Id;
                    scoutViewModelD.FirstName = scout.FirstName;
                    scoutViewModelD.LastName = scout.LastName;
                    scoutViewModelD.ThisOrganizationId = OrganizationId;
                    scoutViewModelD.ThisTypeOrganization = type;
                    scoutViewModelD.ScoutDegree = scout.ScoutDegree;
                    scoutViewModelD.InstructorDegree = scout.InstructorDegree;

                    return View(scoutViewModelD);

                case TypeOrganization.Zastep:
                    var tmpZ = await _context.Zastep.FirstOrDefaultAsync(x => x.ZastepId == OrganizationId);
                    if (tmpZ == null)
                    {
                        return NotFound($"Nie znaleziono tego zastępu.");
                    }

                    var scoutViewModelZ = new ScoutViewModel();
                    scoutViewModelZ.Id = scout.Id;
                    scoutViewModelZ.FirstName = scout.FirstName;
                    scoutViewModelZ.LastName = scout.LastName;
                    scoutViewModelZ.ThisOrganizationId = OrganizationId;
                    scoutViewModelZ.ThisTypeOrganization = type;
                    scoutViewModelZ.ScoutDegree = scout.ScoutDegree;
                    scoutViewModelZ.InstructorDegree = scout.InstructorDegree;

                    return View(scoutViewModelZ);

                default:
                    break;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFunctionScout(ScoutViewModel scoutViewModel)
        {
            if (scoutViewModel.Id == null)
            {
                return NotFound();
            }
            var scout = await _userManager.FindByIdAsync(scoutViewModel.Id);
            if (scout == null)
            {
                return NotFound($"Nie znaleziono harcerza.");
            }
            switch (scoutViewModel.ThisTypeOrganization)
            {
                case TypeOrganization.KwateraGlowna:
                    if (ModelState.IsValid)
                    {
                        var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == scoutViewModel.ThisOrganizationId && f.HufiecId == scoutViewModel.ThisOrganizationId && f.DruzynaId == scoutViewModel.ThisOrganizationId && f.ZastepId == scoutViewModel.ThisOrganizationId).ToList();
                        if (functions == null || !functions.Exists(x => x.FunctionName == scoutViewModel.functionName)) 
                        {
                            if(scoutViewModel.functionName != FunctionName.Brak)
                            {
                                FunctionInOrganization functionInOrganization = new FunctionInOrganization();
                                functionInOrganization.ScoutId = scout.Id;
                                functionInOrganization.FunctionName = scoutViewModel.functionName;

                                functionInOrganization.ChorągiewId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.HufiecId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.DruzynaId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.ZastepId = scoutViewModel.ThisOrganizationId;

                                _context.Add(functionInOrganization);
                                await _context.SaveChangesAsync();
                            }
                            return RedirectToAction(nameof(ShowKwateraGlowna));
                        }
                    }
                    break;
                case TypeOrganization.Choragiew:
                    if (ModelState.IsValid)
                    {
                        var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == scoutViewModel.ThisOrganizationId && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == -1).ToList();
                        if (functions == null || !functions.Exists(x => x.FunctionName == scoutViewModel.functionName))
                        {
                            if (scoutViewModel.functionName != FunctionName.Brak)
                            {
                                FunctionInOrganization functionInOrganization = new FunctionInOrganization();
                                functionInOrganization.ScoutId = scout.Id;
                                functionInOrganization.FunctionName = scoutViewModel.functionName;

                                functionInOrganization.ChorągiewId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.HufiecId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.DruzynaId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.ZastepId = scoutViewModel.ThisOrganizationId;

                                _context.Add(functionInOrganization);
                                await _context.SaveChangesAsync();
                            }
                            return RedirectToAction(nameof(ShowChoragiew));
                        }
                    }
                    break;
                case TypeOrganization.Hufiec:
                    if (ModelState.IsValid)
                    {
                        var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == -1 && f.HufiecId == scoutViewModel.ThisOrganizationId && f.DruzynaId == -1 && f.ZastepId == -1).ToList();
                        if (functions == null || !functions.Exists(x => x.FunctionName == scoutViewModel.functionName))
                        {
                            if (scoutViewModel.functionName != FunctionName.Brak)
                            {
                                FunctionInOrganization functionInOrganization = new FunctionInOrganization();
                                functionInOrganization.ScoutId = scout.Id;
                                functionInOrganization.FunctionName = scoutViewModel.functionName;

                                functionInOrganization.ChorągiewId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.HufiecId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.DruzynaId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.ZastepId = scoutViewModel.ThisOrganizationId;

                                _context.Add(functionInOrganization);
                                await _context.SaveChangesAsync();
                            }
                            return RedirectToAction(nameof(ShowHufiec));
                        }
                    }
                    break;
                case TypeOrganization.Druzyna:
                    if (ModelState.IsValid)
                    {
                        var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == -1 && f.HufiecId == -1 && f.DruzynaId == scoutViewModel.ThisOrganizationId && f.ZastepId == -1).ToList();
                        if (functions == null || !functions.Exists(x => x.FunctionName == scoutViewModel.functionName))
                        {
                            if (scoutViewModel.functionName != FunctionName.Brak)
                            {
                                FunctionInOrganization functionInOrganization = new FunctionInOrganization();
                                functionInOrganization.ScoutId = scout.Id;
                                functionInOrganization.FunctionName = scoutViewModel.functionName;

                                functionInOrganization.ChorągiewId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.HufiecId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.DruzynaId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.ZastepId = scoutViewModel.ThisOrganizationId;

                                _context.Add(functionInOrganization);
                                await _context.SaveChangesAsync();
                            }
                            return RedirectToAction(nameof(ShowDruzyna));
                        }
                    }
                    break;
                case TypeOrganization.Zastep:
                    if (ModelState.IsValid)
                    {
                        var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == -1 && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == scoutViewModel.ThisOrganizationId).ToList();
                        if (functions == null || !functions.Exists(x => x.FunctionName == scoutViewModel.functionName))
                        {
                            if (scoutViewModel.functionName != FunctionName.Brak)
                            {
                                FunctionInOrganization functionInOrganization = new FunctionInOrganization();
                                functionInOrganization.ScoutId = scout.Id;
                                functionInOrganization.FunctionName = scoutViewModel.functionName;

                                functionInOrganization.ChorągiewId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.HufiecId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.DruzynaId = scoutViewModel.ThisOrganizationId;
                                functionInOrganization.ZastepId = scoutViewModel.ThisOrganizationId;

                                _context.Add(functionInOrganization);
                                await _context.SaveChangesAsync();
                            }
                            return RedirectToAction(nameof(ShowZastep));
                        }
                    }
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

                var type = HttpContext.Session.GetString(SessionKeyType);
                if (type == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (type == TypeOrganization.KwateraGlowna.ToString()) return RedirectToAction(nameof(ShowKwateraGlowna));
                else if (type == TypeOrganization.Choragiew.ToString()) return RedirectToAction(nameof(ShowChoragiew));
                else if (type == TypeOrganization.Hufiec.ToString()) return RedirectToAction(nameof(ShowHufiec));
                else if (type == TypeOrganization.Druzyna.ToString()) return RedirectToAction(nameof(ShowDruzyna));
                else if (type == TypeOrganization.Zastep.ToString()) return RedirectToAction(nameof(ShowZastep));
                return RedirectToAction(nameof(ShowAssignments));
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

                var type = HttpContext.Session.GetString(SessionKeyType);
                if (type == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (type == TypeOrganization.KwateraGlowna.ToString()) return RedirectToAction(nameof(ShowKwateraGlowna));
                else if (type == TypeOrganization.Choragiew.ToString()) return RedirectToAction(nameof(ShowChoragiew));
                else if (type == TypeOrganization.Hufiec.ToString()) return RedirectToAction(nameof(ShowHufiec));
                else if (type == TypeOrganization.Druzyna.ToString()) return RedirectToAction(nameof(ShowDruzyna));
                else if (type == TypeOrganization.Zastep.ToString()) return RedirectToAction(nameof(ShowZastep));
            }
            return View(scoutViewModel);
        }

        public IActionResult ShowSubordinateUnits(int OrganizationId, TypeOrganization type)
        {
            switch (type)
            {
                case TypeOrganization.KwateraGlowna:
                    return RedirectToAction("Index", "Choragiews", new { id = OrganizationId });
                case TypeOrganization.Choragiew:
                    return RedirectToAction("Index", "Hufiecs", new { id = OrganizationId });
                case TypeOrganization.Hufiec:
                    return RedirectToAction("Index", "Druzynas", new { id = OrganizationId });
                case TypeOrganization.Druzyna:
                    return RedirectToAction("Index", "Zasteps", new { id = OrganizationId });
                default:
                    return RedirectToAction(nameof(ShowAssignments));
            }
        }

        public IActionResult SendEmailToScout(string mail)
        {
            Email email = new Email();
            email.ScoutEmail = mail;
            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailToScout([Bind("ScoutEmail,Subject,Message")] Email email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var messageTMP = email.Message + "<br /><br />Wiadomość wysłana od " + user.FirstName + ' ' + user.LastName + " (" + user.Email + ')';
                await _mailService.SendEmailAsync(email.ScoutEmail, email.Subject, messageTMP);

                var type = HttpContext.Session.GetString(SessionKeyType);
                if (type == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (type == TypeOrganization.KwateraGlowna.ToString()) return RedirectToAction(nameof(ShowKwateraGlowna));
                else if (type == TypeOrganization.Choragiew.ToString()) return RedirectToAction(nameof(ShowChoragiew));
                else if (type == TypeOrganization.Hufiec.ToString()) return RedirectToAction(nameof(ShowHufiec));
                else if (type == TypeOrganization.Druzyna.ToString()) return RedirectToAction(nameof(ShowDruzyna));
                else if (type == TypeOrganization.Zastep.ToString()) return RedirectToAction(nameof(ShowZastep));
                return RedirectToAction(nameof(ShowAssignments));
            }
            return View(email);
        }

        public IActionResult SendEmailToScouts(int id, TypeOrganization typeOrganization)
        {
            Email email = new Email();
            email.OrganizationID = id;
            email.type = typeOrganization;
            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailToScouts([Bind("OrganizationID,type,ScoutEmail,Subject,Message")] Email email)
        {
            if (ModelState.IsValid)
            {
                List<Scout> scouts = new List<Scout>();
                switch (email.type)
                {
                    case TypeOrganization.KwateraGlowna:
                        scouts = _userManager.Users.Where(u => u.KwateraGlowna != null && u.KwateraGlowna.KwateraGlownaId == email.OrganizationID).ToList();
                        break;
                    case TypeOrganization.Choragiew:
                        var scoutsTmpC = _context.UserChoragiews.Where(u => u.ChoragiewId == email.OrganizationID).ToList();
                        foreach (var scoutTMP in scoutsTmpC)
                        {
                            var scout = await _context.Users.FirstOrDefaultAsync(u => u.Id == scoutTMP.ScoutId);
                            if (scout != null) scouts.Add(scout);
                        }
                        break;
                    case TypeOrganization.Hufiec:
                        var scoutsTmpH = _context.UserHufiecs.Where(u => u.HufiecId == email.OrganizationID).ToList();
                        foreach (var scoutTMP in scoutsTmpH)
                        {
                            var scout = await _context.Users.FirstOrDefaultAsync(u => u.Id == scoutTMP.ScoutId);
                            if (scout != null) scouts.Add(scout);
                        }
                        break;
                    case TypeOrganization.Druzyna:
                        var scoutsTmpD = _context.UserDruzynas.Where(u => u.DruzynaId == email.OrganizationID).ToList();
                        foreach (var scoutTMP in scoutsTmpD)
                        {
                            var scout = await _context.Users.FirstOrDefaultAsync(u => u.Id == scoutTMP.ScoutId);
                            if (scout != null) scouts.Add(scout);
                        }
                        break;
                    case TypeOrganization.Zastep:
                        var scoutsTmpZ = _context.UserZasteps.Where(u => u.ZastepId == email.OrganizationID).ToList();
                        foreach (var scoutTMP in scoutsTmpZ)
                        {
                            var scout = await _context.Users.FirstOrDefaultAsync(u => u.Id == scoutTMP.ScoutId);
                            if (scout != null) scouts.Add(scout);
                        }
                        break;
                    default:
                        break;
                }

                var user = await _userManager.GetUserAsync(User);
                var messageTMP = email.Message + "<br /><br />Wiadomość wysłana od " + user.FirstName + ' ' + user.LastName + " (" + user.Email + ')';

                foreach (var scout in scouts)
                {
                    await _mailService.SendEmailAsync(scout.Email, email.Subject, messageTMP);
                }

                var type = HttpContext.Session.GetString(SessionKeyType);
                if (type == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (type == TypeOrganization.KwateraGlowna.ToString()) return RedirectToAction(nameof(ShowKwateraGlowna));
                else if (type == TypeOrganization.Choragiew.ToString()) return RedirectToAction(nameof(Index));
                else if (type == TypeOrganization.Hufiec.ToString()) return RedirectToAction(nameof(Index));
                else if (type == TypeOrganization.Druzyna.ToString()) return RedirectToAction(nameof(Index));
                else if (type == TypeOrganization.Zastep.ToString()) return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(ShowAssignments));
            }
            return View(email);
        }

        public IActionResult ExportData(int id, TypeOrganization typeOrganization, string name)
        {
            List<Scout> scouts = new List<Scout>();
            switch (typeOrganization)
            {
                case TypeOrganization.KwateraGlowna:
                    scouts = _userManager.Users.Where(u => u.KwateraGlowna != null && u.KwateraGlowna.KwateraGlownaId == id).ToList();
                    break;
                case TypeOrganization.Choragiew:
                    var userInChoragiew = _context.UserChoragiews.Where(x => x.ChoragiewId == id).ToList();
                    scouts = _userManager.Users.ToList().Where(u => userInChoragiew.Exists(x => x.ScoutId == u.Id)).ToList();
                    break;
                case TypeOrganization.Hufiec:
                    var userInHufiec = _context.UserHufiecs.Where(x => x.HufiecId == id).ToList();
                    scouts = _userManager.Users.ToList().Where(u => userInHufiec.Exists(x => x.ScoutId == u.Id)).ToList();
                    break;
                case TypeOrganization.Druzyna:
                    var userInDruzyna = _context.UserDruzynas.Where(x => x.DruzynaId == id).ToList();
                    scouts = _userManager.Users.ToList().Where(u => userInDruzyna.Exists(x => x.ScoutId == u.Id)).ToList();
                    break;
                case TypeOrganization.Zastep:
                    var userInZastep = _context.UserZasteps.Where(x => x.ZastepId == id).ToList();
                    scouts = _userManager.Users.ToList().Where(u => userInZastep.Exists(x => x.ScoutId == u.Id)).ToList();
                    break;
                default:
                    break;
            }

            var workbook = new XLWorkbook();

            IXLWorksheet worksheet = workbook.Worksheets.Add(name);
            worksheet.Cell(1, 1).Value = "Nr";
            worksheet.Cell(1, 2).Value = "Imię";
            worksheet.Cell(1, 3).Value = "Nazwisko";
            worksheet.Cell(1, 4).Value = "Data urodzenia";
            worksheet.Cell(1, 5).Value = "PESEL";

            for (int index = 1; index <= scouts.Count; index++)
            {
                worksheet.Cell(index + 1, 1).Value = index;
                worksheet.Cell(index + 1, 2).Value = scouts[index - 1].FirstName;
                worksheet.Cell(index + 1, 3).Value = scouts[index - 1].LastName;
                worksheet.Cell(index + 1, 4).Value = scouts[index - 1].DateOfBirth;
                worksheet.Cell(index + 1, 5).Value = scouts[index - 1].PESEL;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                string fileName = name + ".xlsx";
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }


        public async Task<IActionResult> ShowChoragiew(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var type = HttpContext.Session.GetString(SessionKeyType);
            var id = HttpContext.Session.GetInt32(SessionKeyOrganization);
            if (type == null || id == null || type != TypeOrganization.Choragiew.ToString())
            {
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var tmp = await _context.Choragiews.FirstOrDefaultAsync(x => x.ChoragiewId == id);
            if (tmp == null)
            {
                return NotFound($"Unable to load chorągiew with this ID.");
            }
            var myFunctionInChoragiew = await _context.FunctionInOrganizations.FirstOrDefaultAsync(f => f.ScoutId == user.Id && ((f.ChorągiewId == tmp.KwateraGlownaId && f.HufiecId == tmp.KwateraGlownaId && f.DruzynaId == tmp.KwateraGlownaId && f.ZastepId == tmp.KwateraGlownaId) || ((f.ChorągiewId == id && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == -1))));
            if (myFunctionInChoragiew == null)
            {
                return RedirectToAction(nameof(ShowAssignments));
            }

            var myScouts = new List<ScoutViewModel>();
            var scoutsTmp = _context.UserChoragiews.Where(u => u.ChoragiewId == id).ToList();
            List<Scout> scouts = new List<Scout>();
            foreach (var scoutTMP in scoutsTmp)
            {
                var scout = await _context.Users.FirstOrDefaultAsync(u => u.Id == scoutTMP.ScoutId);
                if (scout != null) scouts.Add(scout);
            }

            foreach (var scout in scouts)
            {
                var scoutViewModel = new ScoutViewModel();
                scoutViewModel.Id = scout.Id;
                scoutViewModel.FirstName = scout.FirstName;
                scoutViewModel.LastName = scout.LastName;

                var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == id && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == -1);
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


            ViewData["TypeOrganization"] = TypeOrganization.Choragiew;
            ViewData["OrganizationID"] = id;
            ViewData["OrganizationName"] = tmp.Name;

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

        //
        public async Task<IActionResult> ShowHufiec(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var type = HttpContext.Session.GetString(SessionKeyType);
            var id = HttpContext.Session.GetInt32(SessionKeyOrganization);
            if (type == null || id == null || type != TypeOrganization.Choragiew.ToString())
            {
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var tmp = await _context.Choragiews.FirstOrDefaultAsync(x => x.ChoragiewId == id);
            if (tmp == null)
            {
                return NotFound($"Unable to load chorągiew with this ID.");
            }
            var myFunctionInChoragiew = await _context.FunctionInOrganizations.FirstOrDefaultAsync(f => f.ScoutId == user.Id && ((f.ChorągiewId == tmp.KwateraGlownaId && f.HufiecId == tmp.KwateraGlownaId && f.DruzynaId == tmp.KwateraGlownaId && f.ZastepId == tmp.KwateraGlownaId) || ((f.ChorągiewId == id && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == -1))));
            if (myFunctionInChoragiew == null)
            {
                return RedirectToAction(nameof(ShowAssignments));
            }

            var myScouts = new List<ScoutViewModel>();
            var scoutsTmp = _context.UserChoragiews.Where(u => u.ChoragiewId == id).ToList();
            List<Scout> scouts = new List<Scout>();
            foreach (var scoutTMP in scoutsTmp)
            {
                var scout = await _context.Users.FirstOrDefaultAsync(u => u.Id == scoutTMP.ScoutId);
                if (scout != null) scouts.Add(scout);
            }

            foreach (var scout in scouts)
            {
                var scoutViewModel = new ScoutViewModel();
                scoutViewModel.Id = scout.Id;
                scoutViewModel.FirstName = scout.FirstName;
                scoutViewModel.LastName = scout.LastName;

                var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == id && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == -1);
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


            ViewData["TypeOrganization"] = TypeOrganization.Choragiew;
            ViewData["OrganizationID"] = id;
            ViewData["OrganizationName"] = tmp.Name;

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

        //
        public async Task<IActionResult> ShowDruzyna(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var type = HttpContext.Session.GetString(SessionKeyType);
            var id = HttpContext.Session.GetInt32(SessionKeyOrganization);
            if (type == null || id == null || type != TypeOrganization.Choragiew.ToString())
            {
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var tmp = await _context.Choragiews.FirstOrDefaultAsync(x => x.ChoragiewId == id);
            if (tmp == null)
            {
                return NotFound($"Unable to load chorągiew with this ID.");
            }
            var myFunctionInChoragiew = await _context.FunctionInOrganizations.FirstOrDefaultAsync(f => f.ScoutId == user.Id && ((f.ChorągiewId == tmp.KwateraGlownaId && f.HufiecId == tmp.KwateraGlownaId && f.DruzynaId == tmp.KwateraGlownaId && f.ZastepId == tmp.KwateraGlownaId) || ((f.ChorągiewId == id && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == -1))));
            if (myFunctionInChoragiew == null)
            {
                return RedirectToAction(nameof(ShowAssignments));
            }

            var myScouts = new List<ScoutViewModel>();
            var scoutsTmp = _context.UserChoragiews.Where(u => u.ChoragiewId == id).ToList();
            List<Scout> scouts = new List<Scout>();
            foreach (var scoutTMP in scoutsTmp)
            {
                var scout = await _context.Users.FirstOrDefaultAsync(u => u.Id == scoutTMP.ScoutId);
                if (scout != null) scouts.Add(scout);
            }

            foreach (var scout in scouts)
            {
                var scoutViewModel = new ScoutViewModel();
                scoutViewModel.Id = scout.Id;
                scoutViewModel.FirstName = scout.FirstName;
                scoutViewModel.LastName = scout.LastName;

                var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == id && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == -1);
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


            ViewData["TypeOrganization"] = TypeOrganization.Choragiew;
            ViewData["OrganizationID"] = id;
            ViewData["OrganizationName"] = tmp.Name;

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

        //
        public async Task<IActionResult> ShowZastep(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var type = HttpContext.Session.GetString(SessionKeyType);
            var id = HttpContext.Session.GetInt32(SessionKeyOrganization);
            if (type == null || id == null || type != TypeOrganization.Choragiew.ToString())
            {
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var tmp = await _context.Choragiews.FirstOrDefaultAsync(x => x.ChoragiewId == id);
            if (tmp == null)
            {
                return NotFound($"Unable to load chorągiew with this ID.");
            }
            var myFunctionInChoragiew = await _context.FunctionInOrganizations.FirstOrDefaultAsync(f => f.ScoutId == user.Id && ((f.ChorągiewId == tmp.KwateraGlownaId && f.HufiecId == tmp.KwateraGlownaId && f.DruzynaId == tmp.KwateraGlownaId && f.ZastepId == tmp.KwateraGlownaId) || ((f.ChorągiewId == id && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == -1))));
            if (myFunctionInChoragiew == null)
            {
                return RedirectToAction(nameof(ShowAssignments));
            }

            var myScouts = new List<ScoutViewModel>();
            var scoutsTmp = _context.UserChoragiews.Where(u => u.ChoragiewId == id).ToList();
            List<Scout> scouts = new List<Scout>();
            foreach (var scoutTMP in scoutsTmp)
            {
                var scout = await _context.Users.FirstOrDefaultAsync(u => u.Id == scoutTMP.ScoutId);
                if (scout != null) scouts.Add(scout);
            }

            foreach (var scout in scouts)
            {
                var scoutViewModel = new ScoutViewModel();
                scoutViewModel.Id = scout.Id;
                scoutViewModel.FirstName = scout.FirstName;
                scoutViewModel.LastName = scout.LastName;

                var functions = _context.FunctionInOrganizations.Where(f => f.ScoutId == scout.Id && f.ChorągiewId == id && f.HufiecId == -1 && f.DruzynaId == -1 && f.ZastepId == -1);
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


            ViewData["TypeOrganization"] = TypeOrganization.Choragiew;
            ViewData["OrganizationID"] = id;
            ViewData["OrganizationName"] = tmp.Name;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
