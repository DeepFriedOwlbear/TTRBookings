using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTRBookings.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string Text { get; set; }

        public IndexModel(ILogger<IndexModel> logger, NeedToBeInjected needToBeInjected)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Text = "Helloworld!";
        }
    }
}
