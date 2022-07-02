using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTRBookings.Core;

namespace TTRBookings.Infrastructure.Data.Interfaces;

public interface INewRepository<TEntity> : IQueryable<TEntity>
    where TEntity : BaseEntity
{
    Task<bool> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IList<TEntity>> GetAllAsync();
    TTRBookingsContext GetContext();
}
