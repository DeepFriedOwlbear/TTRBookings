using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using TTRBookings.Core.QueryExtensions;
using TTRBookings.Infrastructure.Data.Interfaces;
using TTRBookings.Web.DTOs;
using TTRBookings.Web.Models;

namespace TTRBookings.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly ILogger<BookingsController> _logger;
    private readonly IRepository<Booking> _bookings;
    private readonly IRepository<Room> _rooms;
    private readonly IRepository<Staff> _staff;
    private readonly IRepository<House> _houses;
    public Dictionary<string, string> ToastrErrors { get; set; } = new Dictionary<string, string> { };

    public BookingsController(
        ILogger<BookingsController> logger,
        IRepository<Booking> bookings,
        IRepository<Room> rooms,
        IRepository<Staff> staff,
        IRepository<House> houses)
    {
        _logger = logger;
        _bookings = bookings;
        _rooms = rooms;
        _staff = staff;
        _houses=houses;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(BookingDTO bookingDTO)
    {
        await CheckAgainstBusinessRules(bookingDTO);

        if (ToastrErrors.Count > 0)
        {
            return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
        }

        Booking booking = Booking.Create(
            houseId: Guid.Parse(HttpContext.Session.GetString("HouseId")),
            staff: await _staff.GetByIdAsync(bookingDTO.StaffId),
            tier: new Tier(decimal.Parse(bookingDTO.TierRate)),
            room: await _rooms.GetByIdAsync(bookingDTO.RoomId),
            timeslot: new TimeSlot(DateTime.Parse(bookingDTO.TimeStart), DateTime.Parse(bookingDTO.TimeEnd))
        );

        return new JsonResult(new { Success = _bookings.AddAsync(booking) });
    }

    [HttpPost]
    [Route("edit")]
    public async Task<IActionResult> Edit(BookingDTO bookingDTO)
    {
        await CheckAgainstBusinessRules(bookingDTO);

        if (ToastrErrors.Count > 0)
        {
            return new JsonResult(new { Success = false, ToastrJSON = JsonConvert.SerializeObject(ToastrErrors) });
        }

        Booking booking = await _bookings.GetByIdWithIncludes(bookingDTO.BookingId);
        House house = await _houses.GetByIdWithIncludes(booking.HouseId);

        //assign bookingVM values to booking
        house.UpdateBooking(
            booking: booking,
            tier: new Tier(decimal.Parse(bookingDTO.TierRate)),
            staff: await _staff.GetByIdAsync(bookingDTO.StaffId),
            room: await _rooms.GetByIdAsync(bookingDTO.RoomId),
            timeSlot: new TimeSlot(DateTime.Parse(bookingDTO.TimeStart), DateTime.Parse(bookingDTO.TimeEnd))
        );

        return new JsonResult(new { Success = _houses.UpdateAsync(house) });
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(BookingDTO bookingDTO)
    {
        Booking booking = await _bookings.GetByIdWithIncludes(bookingDTO.BookingId);
        House house = await _houses.GetByIdWithIncludes(booking.HouseId);

        house.RemoveBooking(booking);

        return new JsonResult(new { Success = _houses.UpdateAsync(house) });
    }

    private async Task CheckAgainstBusinessRules(BookingDTO bookingDTO)
    {
        //Checks if form fields are filled in
        if (string.IsNullOrWhiteSpace(bookingDTO.TierRate) || string.IsNullOrWhiteSpace(bookingDTO.TimeStart) || string.IsNullOrWhiteSpace(bookingDTO.TimeEnd))
        {
            ModelState.AddModelError("EmptyFormFields", "[FormFields] Some form fields are empty.");
            ToastrErrors.Add("Empty Form Fields", "Some form fields are empty.");
            return;
        }

        //Assign BookingVM values
        BookingVM bookingVM = new BookingVM
        {
            Id = bookingDTO.BookingId,
            Staff = new StaffVM() { Id = bookingDTO.StaffId },
            Tier = new TierVM() { Rate = decimal.Parse(bookingDTO.TierRate) },
            Room = new RoomVM() { Id = bookingDTO.RoomId },
            TimeSlot = new TimeSlotVM() { Start = DateTime.Parse(bookingDTO.TimeStart), End = DateTime.Parse(bookingDTO.TimeEnd) }
        };

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
        var existingQuery = _bookings.Where(x => x.HouseId == Guid.Parse(HttpContext.Session.GetString("HouseId"))
                                           && x.Staff.Id == bookingVM.Staff.Id
                                           && x.Room.Id == bookingVM.Staff.Id
                                           && x.Id == bookingVM.Id
                                           && !x.IsArchived);

        if (bookingVM.Id != Guid.Empty) 
            existingQuery.Where(x => x.Id == bookingVM.Id);

        IList<Booking> existing = await existingQuery.ToListAsync();

        if (existing.Any()) // input data from database, to check against input from frontend
        {
            // Is Start time of NEW booking within timeslot of EXISTING booking?
            if (existing.Where(x => x.TimeSlot.Start <= bookingVM.TimeSlot.Start 
                                 && x.TimeSlot.End >= bookingVM.TimeSlot.End)
                        .Any())
            {
                ModelState.AddModelError("NewTimeslotStartContainedWithinExistingTimeslot", "[StartDate]: Cannot be within timeslot of existing booking.");
                ToastrErrors.Add("Start Time Scheduling Issue", "The Start Time overlaps with an existing booking.");
            }

            // Is End time of NEW booking within timeslot of EXISTING booking?
            if (existing.Where(x => x.TimeSlot.Start <= bookingVM.TimeSlot.End 
                                 && x.TimeSlot.End >= bookingVM.TimeSlot.End)
                        .Any())
            {
                ModelState.AddModelError("NewTimeslotEndContainedWithinExistingTimeslot", "[EndDate]: Cannot be within timeslot of existing booking.");
                ToastrErrors.Add("End Time Scheduling Issue", "The End Time overlaps with an existing booking.");
            }


            //Is Start time & End Time of EXISTING booking contained within timeslot of NEW booking?
            if (existing.Where(x => x.TimeSlot.Start >= bookingVM.TimeSlot.Start
                                 && x.TimeSlot.Start <= bookingVM.TimeSlot.End
                                 && x.TimeSlot.End >= bookingVM.TimeSlot.Start
                                 && x.TimeSlot.End <= bookingVM.TimeSlot.End)
                        .Any())
            {
                ModelState.AddModelError("ExistingTimeslotStartAndEndContainedWithinNewTimeslot", "[ExistingStartDate] and [ExistingEndDate]: Cannot be within timeslot of new booking.");
                ToastrErrors.Add("Existing Booking Overlap", "A booking that uses that timeslot already exists.");
            }

            //Is Start time & End Time of NEW booking within timeslot of EXISTING booking?
            if (existing.Where(x => x.TimeSlot.Start <= bookingVM.TimeSlot.Start
                                 && x.TimeSlot.End >= bookingVM.TimeSlot.Start
                                 && x.TimeSlot.Start <= bookingVM.TimeSlot.End
                                 && x.TimeSlot.End >= bookingVM.TimeSlot.End)
                        .Any())
            {
                ModelState.AddModelError("NewTimeslotStartAndEndContainedWithinExistingTimeslot", "[StartDate] and [EndDate]: Cannot be within timeslot of existing booking.");
                ModelState.Remove("NewTimeslotStartContainedWithinExistingTimeslot");
                ModelState.Remove("NewTimeslotEndContainedWithinExistingTimeslot");

                ToastrErrors.Add("New Booking Overlap", "The new booking overlaps an existing booking.");
                ToastrErrors.Remove("Start Time Scheduling Issue");
                ToastrErrors.Remove("End Time Scheduling Issue");
            }

            //Is Start time of NEW booking within timeslot of EXISTING booking X, and End time of NEW booking within timeslot of EXISTING booking Y?
            var startWithinExistingQuery = existing.Where(x => x.TimeSlot.Start <= bookingVM.TimeSlot.Start
                                                    && x.TimeSlot.End >= bookingVM.TimeSlot.Start);

            if (startWithinExistingQuery.Any())
            {
                Guid startWithinExistingId = startWithinExistingQuery.FirstOrDefault().Id;

                if (existing.Where(x => x.TimeSlot.Start <= bookingVM.TimeSlot.End
                                     && x.TimeSlot.End >= bookingVM.TimeSlot.End
                                     && x.Id != startWithinExistingId)
                            .Any())
                {
                    ModelState.AddModelError("NewBookingOverlapsWithinMultipleExistingTimeslots", "[StartDate and EndDate]: Cannot overlap with existing bookings.");
                    ToastrErrors.Add("New Booking Overlaps Multiple Bookings", "The new booking overlaps at least two existing booking.");
                }

            }
        }
    }
}
