using DAL.Interface;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Serializable]
    public class SimpleUserValidator : IUserValidator
    {
        public bool Validate(DalUser user)
        {
            if (ReferenceEquals(user, null))
                throw new ArgumentNullException();
            if (user.FirstName == null || user.LastName == null)
                return false;
            return true;
        }
    }
}
