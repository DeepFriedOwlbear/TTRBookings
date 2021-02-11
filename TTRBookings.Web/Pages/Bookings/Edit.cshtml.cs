using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Pages.Bookings
{
    public class EditModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private readonly IRepository repository;
        
        [BindProperty]
        public BookingVM BookingVM { get; set; }

        public List<SelectListItem> RoomList { get; } = new List<SelectListItem> { };
        public List<SelectListItem> RoseList { get; } = new List<SelectListItem> { };
        public List<SelectListItem> TierList { get; } = new List<SelectListItem> { };

        public EditModel(IRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet(Guid id)
        {
            var booking = repository.ReadEntryWithIncludes<Booking>(id, _ => _.Room, _ => _.Rose, _ => _.TimeSlot, _ => _.Tier);

            PopulateList<Rose>(booking);
            PopulateList<Tier>(booking);
            PopulateList<Room>(booking);

            //convert booking to bookingvm here;
            BookingVM = BookingVM.Create(booking);
        }

        //TODO: make PopulateList() prettier, can probably be made sleeker.
        private void PopulateList<TEntity>(Booking booking)
        {
            if(typeof(TEntity) == typeof(Rose))
            {
                foreach (Rose rose in repository.List<Rose>())
                {
                    RoseList.Add(new SelectListItem
                    {
                        Value = rose.Id.ToString(),
                        Text = rose.Name
                    });

                    if (rose.Id == booking.Rose.Id) RoseList.Last().Selected = true;
                }
            }
            else if (typeof(TEntity) == typeof(Tier))
            {
                foreach (Tier tier in repository.List<Tier>(_ => _.RoseId == booking.Rose.Id))
                {
                    TierList.Add(new SelectListItem
                    {
                        Value = tier.Id.ToString(),
                        Text = tier.Rate.ToString()
                    });
                    if (tier.Id == booking.Tier.Id) TierList.Last().Selected = true;
                }
            }
            else if (typeof(TEntity) == typeof(Room))
            {
                foreach (Room room in repository.List<Room>())
                {
                    RoomList.Add(new SelectListItem
                    {
                        Value = room.Id.ToString(),
                        Text = room.Name
                    });

                    if (room.Id == booking.Room.Id) RoomList.Last().Selected = true;
                }
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //retrieve original from database.
            Booking booking = repository.ReadEntryWithIncludes<Booking>(BookingVM.Id, _ => _.Room, _ => _.Rose, _ => _.TimeSlot, _ => _.Tier);

            //assign bookingVM values to booking
            //TODO: When changing the Rose AND Tier of a booking at the same time, the Tier refers to the old Rose's tier.
            booking.Room = repository.ReadEntry<Room>(BookingVM.Room.Id);
            booking.Rose = repository.ReadEntry<Rose>(BookingVM.Rose.Id);
            booking.Tier = repository.ReadEntry<Tier>(BookingVM.Tier.Id);
            booking.TimeSlot.Start = BookingVM.TimeSlot.Start;
            booking.TimeSlot.End = BookingVM.TimeSlot.End;

            //store in database
            repository.UpdateEntry(booking);

            //return/redirect user to somewhere
            return RedirectToPage("/Bookings/Edit", new { booking.Id });
        }

        //[BindProperty]
        //public CartVM Cart {get;set;}
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }
        //    var product = _context.Product.SingleOrDefault(p => p.ProductName == Cart.Name);
        //    OrderItem orderItem = new OrderItem
        //    {

        //        Price = Cart.Price,
        //        Qty = Cart.Qty,
        //        TotalPrice = Cart.TotalPrice,
        //        Product = product
        //    };
        //    _context.OrderItem.Add(orderItem);
        //    await _context.SaveChangesAsync();

        //    return RedirectToPage("../Index");
        //}
    }
}
