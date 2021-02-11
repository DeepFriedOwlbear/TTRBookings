using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TTRBookings.Core.Interfaces
{
    public interface IRepository
    {
        void CreateEntry<TEntity>(TEntity entry) where TEntity : BaseEntity;
        void DeleteEntry<TEntity>(TEntity entry) where TEntity : BaseEntity;
        IList<TEntity> List<TEntity>() where TEntity : BaseEntity;
        IList<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;
        IList<TEntity> ListWithIncludes<TEntity>(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes) where TEntity : BaseEntity;
        TEntity ReadEntry<TEntity>(Guid id) where TEntity : BaseEntity;
        TEntity ReadEntryWithIncludes<TEntity>(Guid id, params Expression<Func<TEntity, object>>[] includes) where TEntity : BaseEntity;
        TEntity UpdateEntry<TEntity>(TEntity entry) where TEntity : BaseEntity;
    }
}