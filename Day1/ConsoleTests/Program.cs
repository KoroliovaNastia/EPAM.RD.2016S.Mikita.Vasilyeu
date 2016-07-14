using DAL;
using Storage;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            UserRepository repository = new UserRepository(new UserStorage());
            User user_1 = new User { FirstName = "Mike", LastName = "Jones" };
            User user_2 = new User { FirstName = "Mike", LastName = "Smith" };
            repository.Add(user_1);
            repository.Add(user_2);
            repository.WriteToXmlFile();
            UserRepository rep_2 = new UserRepository(new UserStorage());
            rep_2.ReadFromXmlFile();
            foreach (var item in rep_2.GetAllUsers())
            {
                Console.WriteLine(item.FirstName + " " + item.LastName);
            }
        }
    }
}
