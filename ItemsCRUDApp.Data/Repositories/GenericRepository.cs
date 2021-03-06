﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ItemsCRUDApp.Data.Repositories
{
    public abstract class GenericRepository<T>
        : IRepository<T> where T : class
    {

        protected ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public virtual async Task<T> Add(T entity)
        {
            return await Task.Run(()=>_context.Add(entity).Entity);
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await Task.Run(() =>_context.Set<T>());
        }

        public async Task Delete(T entity)
        {
            await Task.Run(()=>_context.Remove(entity));
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() =>_context.Set<T>()
                .AsQueryable()
                .Where(predicate));
        }

        public virtual async Task<T> Get(int id)
        {
            var item = await _context.Set<T>().FindAsync(id);
            return item;
        }

        public virtual async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T> Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await Task.Run(() => entity);
        }
    }
}
