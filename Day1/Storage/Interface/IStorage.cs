using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Interface
{
    public interface IStorage<T>
    {
        int Add(T entity);
        void Delete(T entity);
        int[] GetByPredicate(Func<T, bool> predicate);
        IEnumerable<T> GetAll();
        void Save();
        void Load();
    }
}
