using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTRBookings.Authentication.Data;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Account;

[AllowAnonymous]
public class RegisterConfirmationModel : PageModel
{
    private readonly IRepository repository;

    public RegisterConfirmationModel(IRepository repository)
    {
        this.repository = repository;
    }

    public string Name { get; set; }

    public async Task<IActionResult> OnGetAsync(string name)
    {
        if (name == null)
        {
            return RedirectToPage("/Index");
        }

        IList<User> users = repository.List<User>(user => user.Name == name);

        if (users.Count == 0)
        {
            return NotFound($"Unable to load user with email '{name}'.");
        }

        Name = name;

        return Page();
    }
}
