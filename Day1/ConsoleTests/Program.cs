using DAL;
using Storage;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Modes;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            UserRepository repository = new UserRepository(new UserStorage(new EvenEnumerator(), new SimpleUserValidator()), Master.Instance);
            UserEntity user_1 = new UserEntity { FirstName = "Mike", LastName = "Jones", Cards = { new VisaRecordEntity() {Country = "USA"}, new VisaRecordEntity { Country = "China"} }};
            UserRepository rep_2 = new UserRepository(new UserStorage(new EvenEnumerator(), new SimpleUserValidator()), new Slave());
            UserRepository rep_3 = new UserRepository(new UserStorage(new EvenEnumerator(), new SimpleUserValidator()), new Slave());
            //UserRepository rep_4 = new UserRepository(new Storage.UserStorage(new EvenEnumerator(UserStorage.GetSeed()), new SimpleUserValidator()), new Slave());
            UserEntity user_2 = new UserEntity { FirstName = "Mike", LastName = "Smith" };
            repository.Add(user_1);
            repository.Add(user_2);
            repository.Save();
            UserRepository rep = new UserRepository(new UserStorage(new EvenEnumerator(), new SimpleUserValidator()), Master.Instance);
            rep.Load();
            rep.Add(user_2);
            foreach (var item in rep.GetAllUsers())
            {
                Console.WriteLine(item);
            }
        }
    }
}
