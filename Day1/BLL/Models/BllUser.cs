using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BllUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public BllGender Gender { get; set; }
        public List<BllVisaRecord> Cards { get; set; }

        public BllUser()
        {
            Cards = new List<BllVisaRecord>();
        }

        public bool Equals(BllUser other)
        {
            return Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            BllUser other = obj as BllUser;
            return Equals(this, other);
        }

        public static bool Equals(BllUser first, BllUser second)
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
