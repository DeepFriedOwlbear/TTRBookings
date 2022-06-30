using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TTRBookings.Authentication;
using TTRBookings.Authentication.Data;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly IRepository repository;

    public LoginModel(IRepository repository)
    {
        this.repository = repository;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public async Task OnGetAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            IList<User> users = repository.List<User>(user => user.Name == Input.Name);

            if (users.Count == 0 || !Encryption.VerifyPassword(Input.Password, users.FirstOrDefault().Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password");
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, users.FirstOrDefault().Id.ToString()),
                new Claim(ClaimTypes.Name, users.FirstOrDefault().Name),
                //new Claim("UserDefined", "whatever") // <-- Can be used to create custom Claims
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties { IsPersistent = true });

            return RedirectToPage("/Index");
        }

        return Page();

        //return RedirectToPage("Index");
    }
}

