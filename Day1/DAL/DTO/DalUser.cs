using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAL.DTO
{
    [Serializable]
    public class DalUser : IEquatable<DalUser>, IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DalGender Gender { get; set; }
        public List<DalVisaRecord> Cards { get; set; }

        public DalUser()
        {
            Cards = new List<DalVisaRecord>();
        }

        public bool Equals(DalUser other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            DalUser other = obj as DalUser;
            if (ReferenceEquals(other, null))
                return false;
            return Equals(this, other);
        }

        public static bool Equals(DalUser first, DalUser second)
        {
            if (ReferenceEquals(first, null)|| ReferenceEquals(second, null))
                return false;
            return string.Equals(first.FirstName, second.FirstName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(first.LastName, second.LastName, StringComparison.OrdinalIgnoreCase);
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
