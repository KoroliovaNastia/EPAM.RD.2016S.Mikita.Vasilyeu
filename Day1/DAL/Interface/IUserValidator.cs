using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;

namespace DAL.Interface
{
    public interface IUserValidator
    {
        bool Validate(DalUser user);
    }
}
