using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TTRBookings.Core;
using TTRBookings.Core.Interfaces;

namespace TTRBookings.Infrastructure.Data
{
    public class Repository : IRepository
    {
        private readonly TTRBookingsContext context;
        private readonly ILogger<Repository> logger;

        //TODO: see list below, then clean up all DATABASE code from other classes and such
        //CRUD
        //-----
        //CREATE
        //READ
        //UPDATE
        //DELETE

        public Repository(TTRBookingsContext context, ILogger<Repository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void CreateEntry<TEntity>(TEntity entry)
            where TEntity : BaseEntity
        {
            //TODO: change this to be a local method so we can share a single context within a 'session'
            //using var context = new TTRBookingsContext();

            context.Add(entry);
            context.SaveChanges();
        }

        public TEntity ReadEntry<TEntity>(Guid id)
            where TEntity : BaseEntity
        {
            //load entry from the DB where id matches

            //using var context = new TTRBookingsContext();
            return context.Set<TEntity>().FirstOrDefault(e => e.Id == id);
        }

        public TEntity UpdateEntry<TEntity>(TEntity entry)
            where TEntity : BaseEntity
        {
            //load entry from the DB
            //update entry
            //savechanges
            //using var context = new TTRBookingsContext();
            context.Update(entry);
            context.SaveChanges();
            return entry;
        }

        public void DeleteEntry<TEntity>(TEntity entry)
            where TEntity : BaseEntity
        {
            //find entry in the DB
            //delete entry
            //savechanges
            //using var context = new TTRBookingsContext();
            context.Remove(entry);
            context.SaveChanges();
        }

        public IList<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : BaseEntity
            => ListWithIncludes(predicate);

        public IList<TEntity> List<TEntity>()
            where TEntity : BaseEntity
            => ListWithIncludes<TEntity>(_=>true);

        public IList<TEntity> ListWithIncludes<TEntity>(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : BaseEntity
        {
            //using var context = new TTRBookingsContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            foreach (var include in includes)
            {
                var includeString = include.ToString();
                includeString = includeString.Replace(" ", null).Replace("_=>_.", null).Replace(".get_Item(0)", null);
                query = query.Include(includeString);
            }

            return query.Where(predicate).ToList();
        }
    }
}