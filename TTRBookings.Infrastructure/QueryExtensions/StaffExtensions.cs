using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTRBookings.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace TTRBookings.Core.QueryExtensions;

public static class StaffExtensions
{
    public static IQueryable<Staff> WithIncludes(this IQueryable<Staff> query)
    {
        return query.Include(x => x.Tiers);
    }

    public static async Task<Staff> GetByIdWithIncludes(this IQueryable<Staff> query, Guid id)
    {
        return await query.Where(x => x.Id == id)
                          .Include(x => x.Tiers)
                          .FirstOrDefaultAsync();
    }
}
