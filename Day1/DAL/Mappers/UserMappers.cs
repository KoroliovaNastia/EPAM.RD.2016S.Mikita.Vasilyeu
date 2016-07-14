using DAL.Models;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappers
{
    public static class UserMappers
    {
        public static UserEntity ToUserEntity(this User user)
        {
            return new UserEntity
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = (Models.GenderEntity) user.Gender,
                Cards = user.Cards.Select(card => new VisaRecordEntity
                {
                    Country = card.Country,
                    ExpiryDate = card.ExpiryDate,
                    StartDate = card.StartDate
                }).ToList()
            };
        }

        public static User ToUser(this UserEntity user)
        {
            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = (Storage.Models.Gender)user.Gender,
                Cards = user.Cards.Select(card => new VisaRecord
                {
                    Country = card.Country,
                    ExpiryDate = card.ExpiryDate,
                    StartDate = card.StartDate
                }).ToList()
            };
        }
    }
}
