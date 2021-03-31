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

        public Repository(TTRBookingsContext context, ILogger<Repository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void CreateEntry<TEntity>(TEntity entry)
            where TEntity : BaseEntity
        {
            //TODO: change this to be a local method so we can share a single context within a 'session'
            context.Add(entry);
            context.SaveChanges();
        }

        public TEntity ReadEntry<TEntity>(Guid id)
            where TEntity : BaseEntity
        {
            //load entry from the DB where id matches
            return context.Set<TEntity>()
                .Where(e => !e.IsDeleted)
                .FirstOrDefault(e => e.Id == id);
        }

        public TEntity UpdateEntry<TEntity>(TEntity entry)
            where TEntity : BaseEntity
        {
            //load entry from the DB
            //update entry
            //savechanges
            context.Update(entry);
            context.SaveChanges();
            return entry;
        }

        public bool DeleteEntry<TEntity>(TEntity entry)
            where TEntity : BaseEntity
        {
            //find entry in the DB
            //delete entry
            //savechanges

            try
            {
                entry.IsDeleted = true;
                UpdateEntry(entry);
                return true;
            }
            catch
            {
                return false;
            }
            //context.Remove(entry);
            //context.SaveChanges();
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
            return context.Set<TEntity>()
                .AddIncludes(includes)
                .Where(e => !e.IsDeleted)
                .Where(predicate)
                .ToList();
        }

        public TEntity ReadEntryWithIncludes<TEntity>(Guid id, params Expression<Func<TEntity, object>>[] includes) where TEntity : BaseEntity
        {
            return context.Set<TEntity>()
                .AddIncludes(includes)
                .Where(e => !e.IsDeleted)
                .FirstOrDefault(e => e.Id == id);
        }
    }

    internal static class RepositoryExtensions
    {
        public static IQueryable<TEntity> AddIncludes<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, object>>[] includes) where TEntity : BaseEntity
        {
            foreach (var include in includes)
            {
                var includeString = include.ToString();
                includeString = includeString.Replace(" ", null).Replace("_=>_.", null).Replace(".get_Item(0)", null);
                query = query.Include(includeString);
            }

            return query;
        }
    }
}