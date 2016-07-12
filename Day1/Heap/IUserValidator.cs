using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heap
{
    public interface IUserValidator
    {
        bool Validate(User user);
    }
}
