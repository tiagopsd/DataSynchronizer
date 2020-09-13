using DataSynchronizer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DataSynchronizer.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity GetById(params object[] id);
        int Save();
        void Update(TEntity entity);
        void Add(params TEntity[] entities);
    }
}
