﻿using System.Linq.Expressions;

namespace Pragma.Application.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, string includeProperties = null);
        T Update(T entity);
        void Remove(T entity);
        T Create(T entity);
    }
}
