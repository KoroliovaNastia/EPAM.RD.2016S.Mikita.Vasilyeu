using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Interface
{
    public interface IUserRepository : IRepository<UserEntity>
    {

    }
}
