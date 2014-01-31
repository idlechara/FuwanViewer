using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuwanViewer.Model.Infrastructure
{
    /// <summary>
    /// Exposes methods for retrieving entities.
    /// </summary>
    /// <typeparam name="TId">Type of entity's Id.</typeparam>
    /// <typeparam name="T">Type of entities.</typeparam>
    public interface IRepository<TId, T>
    {
        IEnumerable<T> GetAll();
        T Get(TId id);
    }
}
