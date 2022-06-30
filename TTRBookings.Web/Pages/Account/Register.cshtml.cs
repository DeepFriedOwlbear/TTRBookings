using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TTRBookings.Authentication.Data;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly IRepository repository;

    public RegisterModel(IRepository repository)
    {
        this.repository = repository;
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

    public async Task OnGetAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            IList<User> users = repository.List<User>(user => user.Name == Input.Name);

            if (users.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "Username is already in use.");
            }
            else
            {
                User user = Authentication.Data.User.Create(Input.Name, Input.Password);

                //save to DB
                repository.CreateEntry(user);

                return RedirectToPage("RegisterConfirmation", new { name = Input.Name });
            }
        }

        return Page();
    }
}