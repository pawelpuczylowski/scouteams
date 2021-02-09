using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ScouTeams.Areas.Identity.Data;
using ScouTeams.Data;

namespace ScouTeams.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly UserManager<Scout> _userManager;
        private readonly SignInManager<Scout> _signInManager;
        private readonly ScouTDBContext _context;

        public ConfirmEmailChangeModel(UserManager<Scout> userManager, SignInManager<Scout> signInManager, ScouTDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Nie znaleziono tego użytkownika");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (user != null && email != null) 
            {
                user.Email = email;
                await _context.SaveChangesAsync();
            }
            else
            {
                StatusMessage = "Błąd podczas zmiany adresu e-mail.";
                return Page();
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = "Błąd podczas zmiany nazwy użytkownika.";
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Dziękujemy za potwierdzenie zmiany adresu e-mail.";
            return Page();
        }
    }
}
