using DAL;
using Storage;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            UserRepository repository = new UserRepository(new Storage.UserStorage(new EvenEnumerator(), new SimpleUserValidator()));
            UserEntity user_1 = new UserEntity { FirstName = "Mike", LastName = "Jones", Cards = { new VisaRecordEntity() {Country = "USA"}, new VisaRecordEntity { Country = "China"} }};
            UserEntity user_2 = new UserEntity { FirstName = "Mike", LastName = "Smith" };
            repository.Add(user_1);
            repository.Add(user_2);
            repository.Save();
            //UserRepository rep_2 = new UserRepository(new Storage(new EvenEnumerator(), new SimpleUserValidator()));
            UserRepository rep_2 = new UserRepository(new Storage.UserStorage(new EvenEnumerator {Current = repository.GetAllUsers().OrderByDescending(u=>u.Id).FirstOrDefault().Id }, new SimpleUserValidator()));
            rep_2.Load();
            rep_2.Add(user_2);
            foreach (var item in rep_2.GetAllUsers())
            {
                Console.WriteLine(item);
            }
        }
    }
}
