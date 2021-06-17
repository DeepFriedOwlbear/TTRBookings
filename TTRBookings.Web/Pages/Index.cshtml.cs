using Microsoft.AspNetCore.Mvc.RazorPages;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages
{

    public class IndexModel : PageModel
    {
        private readonly IRepository repository;

        public IndexModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet()
        {

        }
    }
}
