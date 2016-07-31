using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IRepository<T> where T : IEntity
    {
        int Add(T entity);
        void Delete(T user);
        int[] GetByPredicate(Func<T, bool> predicate);
        List<T> GetAll();
        void Save();
        //void Load();
        List<T> LoadUsers();
    }
}
