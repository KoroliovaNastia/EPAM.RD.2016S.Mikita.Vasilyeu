using BLL;
using DAL;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Modes;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            UserService repository = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), Master.Instance);
            BllUser user_1 = new BllUser { FirstName = "Mike", LastName = "Jones", Cards = { new BllVisaRecord() {Country = "USA"}, new BllVisaRecord { Country = "China"} }};
            UserService rep_2 = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), new Slave());
            UserService rep_3 = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), new Slave());
            //UserRepository rep_4 = new UserRepository(new Storage.UserStorage(new EvenEnumerator(UserStorage.GetSeed()), new SimpleUserValidator()), new Slave());
            BllUser user_2 = new BllUser { FirstName = "Mike", LastName = "Smith" };
            repository.Add(user_1);
            repository.Add(user_2);
            repository.Save();
            //UserService rep = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), Master.Instance);
            //rep.Load();
            //rep.Add(user_2);
            //foreach (var item in rep.GetAllUsers())
            //{
            //    Console.WriteLine(item);
            //}
        }
    }
}
