﻿using System;
using System.Collections.Generic;
using System.Linq;
using SimpleApp.Core.Models;
using SimpleApp.Infrastructure.Data;

namespace SimpleApp.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseModel, new()
    {
        protected Repository(AppDbContext dataContext)
        {
            Context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        protected readonly AppDbContext Context;

        public virtual T GetById(Guid id)
        {
            return Context.Set<T>()
                .FirstOrDefault(e => e.Id == id && e.IsActive);
        }

        public virtual IEnumerable<T> GetAllActive()
        {
            return Context.Set<T>()
                .Where(e => e.IsActive)
                .ToList();
        }

        public virtual T Add(T entity)
        {
            Context.Set<T>()
                .Add(entity);

            return entity;
        }

        public virtual void Delete(T entity)
        {
            entity.IsActive = false;
        }

        public virtual void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}