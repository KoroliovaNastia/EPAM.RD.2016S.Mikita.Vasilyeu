using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.Models;

namespace Storage.Interface
{
    public interface IUserValidator
    {
        bool Validate(User user);
    }
}
