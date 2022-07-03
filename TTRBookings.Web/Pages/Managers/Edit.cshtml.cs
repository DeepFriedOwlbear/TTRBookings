using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Infrastructure.Data.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Managers;

public class EditModel : PageModel
{
    private readonly IRepository<Manager> _managers;

    [BindProperty]
    public ManagerVM ManagerVM { get; set; }

    public EditModel(IRepository<Manager> managers)
    {
        _managers = managers;
    }

    public async Task OnGetAsync(Guid id)
    {
        var manager = await _managers.GetByIdAsync(id);

        //convert booking to bookingvm here;
        ManagerVM = ManagerVM.Create(manager);
    }
}
