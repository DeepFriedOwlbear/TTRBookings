using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TTRBookings.Core.Entities;
using TTRBookings.Core.Interfaces;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Controllers
{
    [Route("api/[controller]")] //< =  https://localhost:12345/api/bookings
    [ApiController]
    public class BookingsController : ControllerBase
    {   
        private readonly ILogger<BookingsController> _logger;
        private readonly IRepository repository;
        public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

        public BookingsController(ILogger<BookingsController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        //------------------------------------------------------------------------------------------------------------
        //--API Calls-------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        [HttpPost]          //starting a NESTED route WITHOUT a '/' as initial character will mean append to current build-up route
        [Route("create")]   //< =  https://localhost:12345/api/bookings/create
        public IActionResult Create(BookingDTO bookingDTO)
        {
            return HandleBooking(bookingDTO, "create");
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(BookingDTO bookingDTO)
        {
            return HandleBooking(bookingDTO, "edit");
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(BookingDTO bookingDTO)
        {
            return HandleBooking(bookingDTO, "delete");
        }

        //------------------------------------------------------------------------------------------------------------
        //--Booking Methods-------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        public JsonResult HandleBooking(BookingDTO bookingDTO, string action)
        {
            //check if form fields are filled in
            if(action != "delete")
            {
                CheckAgainstBusinessRules(bookingDTO);

                //if form fields or business logic threw errors, return a failed success state and toastr errors
                if (ToastrErrors.Count > 0)
                    return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
            }

            switch (action) 
            {
                case "create": 
                    return CreateBooking(bookingDTO);
                case "edit":
                    return EditBooking(bookingDTO);
                case "delete":
                    return DeleteBooking(bookingDTO);
                default:
                    ModelState.AddModelError("ErrorOccured", "An error occured while performing this operation.");
                    ToastrErrors.Add("An error occured", "An error occured while performing this operation.");
                    return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
            }
        }

        public JsonResult CreateBooking(BookingDTO bookingDTO)
        {
            //assign bookingVM values to booking
            Tier tier = new Tier(Decimal.Parse(bookingDTO.TierRate));

            Booking booking = Booking.Create(
            Guid.Parse(HttpContext.Session.GetString("HouseId")),
                repository.ReadEntry<Staff>(bookingDTO.StaffId),
                tier,
                repository.ReadEntry<Room>(bookingDTO.RoomId),
                new TimeSlot(DateTime.Parse(bookingDTO.TimeStart), DateTime.Parse(bookingDTO.TimeEnd))
            );

            return new JsonResult(new { Success = repository.CreateEntry(booking) });
        }

        public JsonResult EditBooking(BookingDTO bookingDTO)
        {
            //retrieve original from database.
            Booking booking = repository.ReadEntryWithIncludes<Booking>(bookingDTO.BookingId, _ => _.Room, _ => _.Staff, _ => _.TimeSlot, _ => _.Tier);
            House house = repository.ReadEntryWithIncludes<House>(booking.HouseId, _ => _.Managers, _ => _.Rooms, _ => _.Staff, _ => _.Bookings, _ => _.Bookings[0].Tier, _ => _.Bookings[0].TimeSlot);

            //assign bookingVM values to booking
            house.UpdateBooking(
                booking,
                new Tier(Decimal.Parse(bookingDTO.TierRate)),
                repository.ReadEntry<Staff>(bookingDTO.StaffId),
                repository.ReadEntry<Room>(bookingDTO.RoomId),
                new TimeSlot(DateTime.Parse(bookingDTO.TimeStart), DateTime.Parse(bookingDTO.TimeEnd))
            );

            return new JsonResult(new { Success = repository.UpdateEntry(house) });
        }

        public JsonResult DeleteBooking(BookingDTO bookingDTO)
        {
            //retrieve originals from database.
            Booking booking = repository.ReadEntryWithIncludes<Booking>(bookingDTO.BookingId, _ => _.Room, _ => _.Staff, _ => _.TimeSlot, _ => _.Tier);
            House house = repository.ReadEntryWithIncludes<House>(booking.HouseId, _ => _.Managers, _ => _.Rooms, _ => _.Staff, _ => _.Bookings, _ => _.Bookings[0].Tier, _ => _.Bookings[0].TimeSlot);

            //remove booking from house
            house.RemoveBooking(booking);

            return new JsonResult(new { Success = repository.UpdateEntry(house) });
        }

        //------------------------------------------------------------------------------------------------------------
        //--Data Transfer Object--------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        public sealed class BookingDTO
        {
            public Guid BookingId { get; set; }
            public Guid StaffId { get; set; }
            public Guid RoomId { get; set; }
            public string TierRate { get; set; }
            public string TimeStart { get; set; }
            public string TimeEnd { get; set; }
        }

        //------------------------------------------------------------------------------------------------------------
        //--Business Logic checks-------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        private void CheckAgainstBusinessRules(BookingDTO bookingDTO)
        {
            //Checks if form fields are filled in
            if (string.IsNullOrWhiteSpace(bookingDTO.TierRate) || string.IsNullOrWhiteSpace(bookingDTO.TimeStart) || string.IsNullOrWhiteSpace(bookingDTO.TimeEnd))
            {
                ModelState.AddModelError("EmptyFormFields", "[FormFields] Some form fields are empty.");
                ToastrErrors.Add("Empty Form Fields", "Some form fields are empty.");
                return;
            }

            //Assign BookingVM values
            BookingVM bookingVM = new BookingVM();
            bookingVM.Id = bookingDTO.BookingId;
            bookingVM.Staff = new StaffVM() { Id = bookingDTO.StaffId };
            bookingVM.Tier = new TierVM() { Rate = Decimal.Parse(bookingDTO.TierRate) };
            bookingVM.Room = new RoomVM() { Id = bookingDTO.RoomId };
            bookingVM.TimeSlot = new TimeSlotVM() { Start = DateTime.Parse(bookingDTO.TimeStart), End = DateTime.Parse(bookingDTO.TimeEnd) };

            //Bookings can't be made in the past.
            if (bookingVM.TimeSlot.Start < DateTime.Now)
            {
                ModelState.AddModelError("StartDateBeforeCurrentDate", "[StartDate]: Cannot be in the past.");
                ToastrErrors.Add("Invalid Start Date", "Start Date can't be in the past.");
            }
            //TimeSlot Start can't be after TimeSlot End
            if (bookingVM.TimeSlot.Start > bookingVM.TimeSlot.End)
            {
                ModelState.AddModelError("EndDateBeforeStartDate", "[EndDate]: Cannot be before [StartDate]");
                ToastrErrors.Add("Invalid End Date", "End Date can't be before Start Date.");
            }
            //Duration between TimeSlot Start and TimeSlot End can't be longer than 24 hours
            if ((bookingVM.TimeSlot.End - bookingVM.TimeSlot.Start).TotalHours >= 24)
            {
                ModelState.AddModelError("BookingDurationLongerThan24Hours", "[EndDate]: Duration between [StartDate] and [EndDate] cannot be longer than 24 hours.");
                ToastrErrors.Add("Invalid Booking Duration", "Booking duration can't be longer than 24 hours.");
            }

            //Load all bookings where the HouseId, RoomId and StaffId matches
            IList<Booking> existing = new List<Booking>();
            if(bookingVM.Id != Guid.Empty)
            {
                existing = repository.ListWithIncludes<Booking>(
                    //the filter
                    booking => !booking.IsDeleted
                    && booking.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))
                    && booking.Room.Id == bookingVM.Room.Id
                    && booking.Staff.Id == bookingVM.Staff.Id
                    && booking.Id != bookingVM.Id
                    ,
                    //the includes
                    _ => _.Room, _ => _.Staff, _ => _.TimeSlot);
            } 
            else
            {
                existing = repository.ListWithIncludes<Booking>(
                    //the filter
                    booking => !booking.IsDeleted
                    && booking.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))
                    && booking.Room.Id == bookingVM.Room.Id
                    && booking.Staff.Id == bookingVM.Staff.Id
                    ,
                    //the includes
                    _ => _.Room, _ => _.Staff, _ => _.TimeSlot);
            }

            //&& booking.Id != BookingVM.Id

            if (existing.Any()) // input data from database, to check against input from frontend
            {
                //Is Start time of NEW booking within timeslot of EXISTING booking?
                if (existing.Where(booking =>
                    bookingVM.TimeSlot.Start >= booking.TimeSlot.Start
                    && bookingVM.TimeSlot.Start <= booking.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("NewTimeslotStartContainedWithinExistingTimeslot", "[StartDate]: Cannot be within timeslot of existing booking.");
                    ToastrErrors.Add("Start Time Scheduling Issue", "The Start Time overlaps with an existing booking.");
                }

                //Is End time of NEW booking within timeslot of EXISTING booking?
                if (existing.Where(booking =>
                    bookingVM.TimeSlot.End >= booking.TimeSlot.Start
                    && bookingVM.TimeSlot.End <= booking.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("NewTimeslotEndContainedWithinExistingTimeslot", "[EndDate]: Cannot be within timeslot of existing booking.");
                    ToastrErrors.Add("End Time Scheduling Issue", "The End Time overlaps with an existing booking.");
                }

                //Is Start time & End Time of EXISTING booking contained within timeslot of NEW booking?
                if (existing.Where(booking =>
                    booking.TimeSlot.Start >= bookingVM.TimeSlot.Start
                    && booking.TimeSlot.Start <= bookingVM.TimeSlot.End
                    && booking.TimeSlot.End >= bookingVM.TimeSlot.Start
                    && booking.TimeSlot.End <= bookingVM.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("ExistingTimeslotStartAndEndContainedWithinNewTimeslot", "[ExistingStartDate] and [ExistingEndDate]: Cannot be within timeslot of new booking.");
                    ToastrErrors.Add("Existing Booking Overlap", "A booking that uses that timeslot already exists.");
                }

                //Is Start time & End Time of NEW booking within timeslot of EXISTING booking?
                if (existing.Where(booking =>
                    bookingVM.TimeSlot.Start >= booking.TimeSlot.Start
                    && bookingVM.TimeSlot.Start <= booking.TimeSlot.End
                    && bookingVM.TimeSlot.End >= booking.TimeSlot.Start
                    && bookingVM.TimeSlot.End <= booking.TimeSlot.End).Any())//check case X of overlap schema.
                {
                    ModelState.AddModelError("NewTimeslotStartAndEndContainedWithinExistingTimeslot", "[StartDate] and [EndDate]: Cannot be within timeslot of existing booking.");
                    ModelState.Remove("NewTimeslotStartContainedWithinExistingTimeslot");
                    ModelState.Remove("NewTimeslotEndContainedWithinExistingTimeslot");

                    ToastrErrors.Add("New Booking Overlap", "The new booking overlaps an existing booking.");
                    ToastrErrors.Remove("Start Time Scheduling Issue");
                    ToastrErrors.Remove("End Time Scheduling Issue");
                }

                //Is Start time of NEW booking within timeslot of EXISTING booking X, and End time of NEW booking within timeslot of EXISTING booking Y?
                var condition1 = existing.Where(booking =>
                    bookingVM.TimeSlot.Start >= booking.TimeSlot.Start
                    && bookingVM.TimeSlot.Start <= booking.TimeSlot.End);
                if (condition1.Any())
                {
                    Guid idCondition1 = condition1.FirstOrDefault().Id;
                    var condition2 = existing.Where(booking =>
                    bookingVM.TimeSlot.End >= booking.TimeSlot.Start
                    && bookingVM.TimeSlot.End <= booking.TimeSlot.End
                    && booking.Id != idCondition1).Any();

                    if (condition2)//check case X of overlap schema.
                    {
                        ModelState.AddModelError("NewBookingOverlapsWithinMultipleExistingTimeslots", "[StartDate and EndDate]: Cannot overlap with existing bookings.");
                        ToastrErrors.Add("New Booking Overlaps Multiple Bookings", "The new booking overlaps at least two existing booking.");
                    }
                }
            }
        }
    }
}
