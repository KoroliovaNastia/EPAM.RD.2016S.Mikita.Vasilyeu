using Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Models
{
    public class User : IEquatable<User>
    {
        public static ICustomerEnumerator iterator { get; set; }
        public static IUserValidator validator { get; set; }

        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public IEnumerable<VisaRecord> Cards { get; }

        static User()
        {
            iterator = new EvenEnumerator();
            validator = new SimpleUserValidator();
        }

        public User(ICustomerEnumerator iterator = null, IUserValidator validator = null)
        {
            if (iterator != null)
                User.iterator = iterator;
            if (validator != null)
                User.validator = validator;
            Cards = new List<VisaRecord>();
            Id = User.iterator.GetNext();
        }

        public bool Equals(User other)
        {
            return Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            User other = obj as User;
            return Equals(this, other);
        }

        public static bool Equals(User first, User second)
        {
            return first?.FirstName == second?.FirstName
                && first?.LastName == second?.LastName;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Id} {FirstName} {LastName}";
        }
    }

}
