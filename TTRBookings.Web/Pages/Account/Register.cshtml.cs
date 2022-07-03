using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Authentication.Data;
using TTRBookings.Infrastructure.Data.Interfaces;

namespace TTRBookings.Web.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly IRepository<User> _users;

    public RegisterModel(IRepository<User> users)
    {
        _users=users;
    }

    [BindProperty]
    public InputModel Input { get; set; }
    public string ReturnUrl { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public class InputModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public void OnGet(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var users = await _users.Where(x => x.Name == Input.Name).ToListAsync();

            if (users.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "Username is already in use.");
            }
            else
            {
                User user = Authentication.Data.User.Create(Input.Name, Input.Password);

                await _users.AddAsync(user);

                return RedirectToPage("RegisterConfirmation", new { name = Input.Name });
            }
        }

        return Page();
    }
}