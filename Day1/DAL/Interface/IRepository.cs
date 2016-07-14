using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Interface
{
    public interface IRepository<T> where T : IEntity
    {
        int Add(T user);
        int[] SearchForUsers(Func<T, bool> criteria);
        void Delete(T user);
        IEnumerable<T> GetAllUsers();
        void Save();
        void Load();
    }
}
