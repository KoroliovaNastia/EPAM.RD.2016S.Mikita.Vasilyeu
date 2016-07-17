using BLL.Models;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappers
{
    public static class UserMappers
    {
        public static BllUser ToBllUser(this DalUser user)
        {
            return new BllUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = (Models.BllGender) user.Gender,
                Cards = user.Cards.Select(card => new BllVisaRecord
                {
                    Country = card.Country,
                    ExpiryDate = card.ExpiryDate,
                    StartDate = card.StartDate
                }).ToList()
            };
        }

        public static DalUser ToDalUser(this BllUser user)
        {
            return new DalUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = (DalGender)user.Gender,
                Cards = user.Cards.Select(card => new DalVisaRecord
                {
                    Country = card.Country,
                    ExpiryDate = card.ExpiryDate,
                    StartDate = card.StartDate
                }).ToList()
            };
        }
    }
}
