using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Mappers;
using DAL.DTO;

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
            return Equals(this.ToDalUser(), other?.ToDalUser());
        }

        public override bool Equals(object obj)
        {
            BllUser other = obj as BllUser;
            return Equals(this.ToDalUser(), other?.ToDalUser());
        }

        public static bool Equals(BllUser first, BllUser second)
        {
            return (DalUser.Equals(first?.ToDalUser(), second.ToDalUser()));
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
