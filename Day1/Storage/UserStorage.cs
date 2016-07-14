using Storage.Interface;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public class UserStorage : AbstractUserStorage
    {
        public ICustomerEnumerator iterator { get; private set; }
        public IUserValidator validator { get; private set; }

        public UserStorage(ICustomerEnumerator iterator = null, IUserValidator validator = null) : base()
        {
            if (iterator != null)
                this.iterator = iterator;
            if (validator != null)
                this.validator = validator;
        }

        public override int Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            if (!validator.Validate(user))
                throw new ArgumentException();
            user.Id = iterator.GetNext();
            Users.Add(user);
            return user.Id;
        }
    }
}
