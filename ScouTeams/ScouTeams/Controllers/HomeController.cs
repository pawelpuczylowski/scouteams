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
using ScouTeams.Models;

namespace ScouTeams.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<Scout> _userManager;
        private readonly SignInManager<Scout> _signInManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(SignInManager<Scout> signInManager, ILogger<HomeController> logger, UserManager<Scout> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    /*
     * private readonly UserManager<Scout> _userManager;
        private readonly SignInManager<Scout> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<Scout> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<Scout> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }*/
}
