using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace TTRBookings.Core.QueryExtensions;

public static class HouseExtensions
{
    public static IQueryable<House> WithIncludes(this IQueryable<House> query)
    {
        return query.Include(x => x.Staff)
                    .Include(x => x.Managers)
                    .Include(x => x.Rooms)
                    .Include(x => x.Bookings)
                        .ThenInclude(x => x.Tier)
                    .Include(x => x.Bookings)
                        .ThenInclude(x => x.TimeSlot);
    }

    public static async Task<House> GetByIdWithIncludes(this IQueryable<House> query, Guid id)
    {
       return await query.Where(x => x.Id == id)
                         .Include(x => x.Staff)
                         .Include(x => x.Managers)
                         .Include(x => x.Rooms)
                         .Include(x => x.Bookings)
                            .ThenInclude(x => x.Tier)
                         .Include(x => x.Bookings)
                            .ThenInclude(x => x.TimeSlot)
                         .FirstOrDefaultAsync();
    }
}
