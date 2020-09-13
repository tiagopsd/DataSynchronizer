using DataSynchronizer.Domain.Entities;
using DataSynchronizer.Domain.Interfaces;
using DataSynchronizer.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace DataSynchronizer.Infra.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public DbSet<TEntity> Set { get; set; }

        public readonly Context _context;
        public Repository(Context context)
        {
            Set = context.Set<TEntity>();
            _context = context;
        }

        public TEntity GetById(params object[] id)
        {
            try
            {
                return Set.Find(id);
            }
            catch
            {
                throw;
            }
        }

        public void Update(TEntity entity)
        {
            try
            {
                object id = entity.GetType().GetProperty("Id").GetValue(entity, null);
                var original = Set.Find(id);
                _context.Entry(original).State = EntityState.Modified;
                _context.Entry(original).OriginalValues.SetValues(entity);
                _context.Entry(original).CurrentValues.SetValues(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Add(params TEntity[] entities)
        {
            try
            {
                Set.AddRange(entities);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Save()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
