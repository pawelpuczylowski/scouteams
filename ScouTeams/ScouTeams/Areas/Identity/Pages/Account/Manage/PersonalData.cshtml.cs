using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ScouTeams.Areas.Identity.Data;
using ScouTeams.Models;
using static Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal.ExternalLoginModel;

namespace ScouTeams.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<Scout> _userManager;
        private readonly SignInManager<Scout> _signInManager;
        private readonly ILogger<PersonalDataModel> _logger;

        public PersonalDataModel(
            UserManager<Scout> userManager,
            SignInManager<Scout> signInManager,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [Display(Name = "Stopień harcerski")]
        public ScoutDegree ScoutDegree { get; set; }

        [Display(Name = "Stopień instruktorski")]
        public InstructorDegree InstructorDegree { get; set; }

        public class InputModel
        {
            [DataType(DataType.Text)]
            [Required(ErrorMessage = "Proszę podać imię")]
            [Display(Name = "Imię")]
            public string FirstName { get; set; }

            [DataType(DataType.Text)]
            [Required(ErrorMessage = "Proszę podać nazwisko")]
            [Display(Name = "Nazwisko")]
            public string LastName { get; set; }


            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            [DataType(DataType.Date)]
            [Required(ErrorMessage = "Proszę podać datę urodzenia")]
            [Display(Name = "Data urodzenia")]
            public DateTime DateOfBirth { get; set; }

            [Required]
            [Display(Name = "Werbunek")]
            public bool Recruitment { get; set; }

            [Required, RegularExpression(@"^\d{11}$", ErrorMessage ="Proszę podać 11 cyfr")]
            public string PESEL { get; set; }
        }

        private void Load(Scout user)
        {
            ScoutDegree = user.ScoutDegree;
            InstructorDegree = user.InstructorDegree;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Recruitment = user.Recruitment,
                PESEL = user.PESEL
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            var scout = await _userManager.FindByIdAsync(user.Id);
            if (Input.FirstName != scout.FirstName)
            {
                scout.FirstName = Input.FirstName;
            }
            if (Input.LastName != scout.LastName)
            {
                scout.LastName = Input.LastName;
            }
            if (Input.DateOfBirth != scout.DateOfBirth)
            {
                scout.DateOfBirth = Input.DateOfBirth;
            }
            if (Input.Recruitment != scout.Recruitment)
            {
                scout.Recruitment = Input.Recruitment;
            }
            if (Input.PESEL != scout.PESEL)
            {
                scout.PESEL = Input.PESEL;
            }

            var result = await _userManager.UpdateAsync(scout);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Twój profil został zaktualizowany.";
                return RedirectToPage();
            }
            else
            {
                StatusMessage = "Wystąpił błąd podczas zapisywania danych.";
                return RedirectToPage();
            }
        }

    }
}