using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TTRBookings.Core.Interfaces;

public interface IRepository
{
    bool AddAndSaveChanges<TEntity>(TEntity entry) where TEntity : BaseEntity;
    bool ArchiveAndSaveChanges<TEntity>(TEntity entry) where TEntity : BaseEntity;
    IList<TEntity> List<TEntity>() where TEntity : BaseEntity;
    IList<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;
    IList<TEntity> ListWithIncludes<TEntity>(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes) where TEntity : BaseEntity;
    TEntity GetById<TEntity>(Guid id) where TEntity : BaseEntity;
    TEntity ReadEntryWithIncludes<TEntity>(Guid id, params Expression<Func<TEntity, object>>[] includes) where TEntity : BaseEntity;
    TEntity UpdateAndSaveChanges<TEntity>(TEntity entry) where TEntity : BaseEntity;
}