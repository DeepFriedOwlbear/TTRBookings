using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Web.Pages.Bookings
{
    public class DetailsModel : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;
        //private readonly IRepository repository;

        public IList<Booking> Bookings { get; set; }

        public void OnGet(Guid id)
        {
            //what was the goal for this page?
            //What do we need to ask our dearest dependency container?

            
            //Bookings = repository.ListWithIncludes<Booking>(_ => _.Id == id, _ => _.Room);

           


            //Load booking where id matches from database
            //pass information to Details.cshtml


        }

        private void NoteAboutLambda()
        {
            //why dont we have to say 'var ... = repo...'?
            //LambdaMethodXYZ<ClassOfType>(x => x.SomePropertyOrMethod);
            //x refers to an single item within the collection.
            //so x = an x of the class ClassOfType

            //var list = new List<Booking>();
            //var newlist = new List<Booking>();
            //foreach (Booking booking in list)
            //{
            //    if (booking.Id == Guid.NewGuid())
            //    {
            //        newList.Add(booking);
            //    }
            //}

            //var list2 = new List<Booking>();
            //var newlist2 = list2.Where(b => b.Id == Guid.NewGuid()).ToList();
        }
    }
}
