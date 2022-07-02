using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace TTRBookings.Core.QueryExtensions;

public static class BookingExtensions
{
    public static IQueryable<Booking> WithIncludes(this IQueryable<Booking> query)
    {
        return query.Include(x => x.Staff)
                    .Include(x => x.Room)
                    .Include(x => x.TimeSlot)
                    .Include(x => x.Tier);
    }

    public static async Task<Booking> GetByIdWithIncludes(this IQueryable<Booking> query, Guid id)
    {
        return await query.Where(x => x.Id == id)
                          .Include(x => x.Staff)
                          .Include(x => x.Room)
                          .Include(x => x.TimeSlot)
                          .Include(x => x.Tier)
                          .FirstOrDefaultAsync();
    }
}
