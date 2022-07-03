using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTRBookings.Core;

namespace TTRBookings.Infrastructure.Data.Interfaces;

public interface IRepository<T> : IQueryable<T>
    where T : BaseEntity
{
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity, bool archive = true);
    Task<T> GetByIdAsync(Guid id);
    Task<IList<T>> GetAllAsync(bool archived = false);
    TTRBookingsContext GetContext();
}
