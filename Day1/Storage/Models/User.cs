using Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Storage.Models
{
    [Serializable]
    public class User : IEquatable<User>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public List<VisaRecord> Cards { get; }

        public User()
        {
            Cards = new List<VisaRecord>();
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
