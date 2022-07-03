using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TTRBookings.Core;
using TTRBookings.Infrastructure.Data.Interfaces;

namespace TTRBookings.Infrastructure.Data;

public class Repository<T> : IRepository<T> 
    where T : BaseEntity
{
    private IQueryable<T> _queryable => DbSet;
    private TTRBookingsContext Context;
    protected DbSet<T> DbSet;

    public Repository(TTRBookingsContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }

    public async Task<bool> AddAsync(T entity)
    {
        try
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        try
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(T entity, bool archive = true)
    {
        try
        {
            if (archive)
            {
                entity.IsArchived = true;
                await UpdateAsync(entity);
                return true;
            }

            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await DbSet.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IList<T>> GetAllAsync(bool archived = false)
        => await _queryable.Where(x => x.IsArchived == archived).ToListAsync();

    public TTRBookingsContext GetContext() => Context;

    #region Inherited Implementations

    IEnumerator IEnumerable.GetEnumerator() => _queryable.GetEnumerator();
    public IEnumerator<T> GetEnumerator() => _queryable.GetEnumerator();
    public IQueryProvider Provider => _queryable.Provider;
    public Type ElementType => _queryable.ElementType;
    public Expression Expression => _queryable.Expression;

    #endregion
}
