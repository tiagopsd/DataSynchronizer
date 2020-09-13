using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DataSynchronizer.Domain.Interfaces
{
    public interface IEntity<T> : IEntity
    {
        T Id { get; set; }
    }

    public interface IEntity
    {
    }
}
