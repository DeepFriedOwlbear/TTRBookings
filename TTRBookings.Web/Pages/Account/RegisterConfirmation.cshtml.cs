using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Authentication.Data;
using TTRBookings.Infrastructure.Data.Interfaces;

namespace TTRBookings.Web.Pages.Account;

[AllowAnonymous]
public class RegisterConfirmationModel : PageModel
{
    private readonly IRepository<User> _users;

    public RegisterConfirmationModel(IRepository<User> users)
    {
        _users=users;
    }

    public string Name { get; set; }

    public async Task<IActionResult> OnGetAsync(string name)
    {
        if (name == null)
        {
            return RedirectToPage("/Index");
        }

        var users = await _users.Where(x => x.Name == name).ToListAsync();

        if (users.Count == 0)
        {
            return NotFound($"Unable to load user with email '{name}'.");
        }

        Name = name;

        return Page();
    }
}
