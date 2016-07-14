using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class UserEntity : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderEntity Gender { get; set; }
        public List<VisaRecordEntity> Cards { get; set; }

        public UserEntity()
        {
            Cards = new List<VisaRecordEntity>();
        }

        public bool Equals(UserEntity other)
        {
            return Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            UserEntity other = obj as UserEntity;
            return Equals(this, other);
        }

        public static bool Equals(UserEntity first, UserEntity second)
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
