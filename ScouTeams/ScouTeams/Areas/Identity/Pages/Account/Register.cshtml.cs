using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ScouTeams.Areas.Identity.Data;
using ScouTeams.Services;

namespace ScouTeams.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Scout> _signInManager;
        private readonly UserManager<Scout> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMailService _emailSender;

        public RegisterModel(
            UserManager<Scout> userManager,
            SignInManager<Scout> signInManager,
            ILogger<RegisterModel> logger,
            IMailService emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [DataType(DataType.Text)]
            [Display(Name = "Imię")]
            [Required(ErrorMessage = "Proszę podać imię")]
            public string FirstName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Nazwisko")]
            [Required(ErrorMessage = "Proszę podać nazwisko")]
            public string LastName { get; set; }

            [DataType(DataType.DateTime)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            [Display(Name = "Data urodzenia")]
            [Required(ErrorMessage = "Proszę podać datę urodzenia")]
            public DateTime DateOfBirth { get; set; }

            [Display(Name = "Numer komórkowy")]
            [Required(ErrorMessage = "Proszę podać numer komórkowy")]
            [DataType(DataType.PhoneNumber)]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Proszę podać swój email")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Proszę podać swój PESEL"), RegularExpression(@"^\d{11}$", ErrorMessage = "Proszę podać 11 cyfr")]
            public string PESEL { get; set; }

            [Required(ErrorMessage = "Proszę podać hasło do konta")]
            [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Powtórz hasło")]
            [Compare("Password", ErrorMessage = "Hasło i powtórzone hasło nie są takie same.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Scout { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName, DateOfBirth = Input.DateOfBirth, PhoneNumber = Input.PhoneNumber, Recruitment = true, PESEL = Input.PESEL };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
