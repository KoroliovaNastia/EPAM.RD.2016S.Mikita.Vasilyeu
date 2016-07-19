using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Modes;
using BLL.Config;
using System.Configuration;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterServices section =
                RegisterServices.GetConfig();

            if (section != null)
            {
                Console.WriteLine(section.ServicesItems[0].ServiceType);
                Console.WriteLine(section.ServicesItems[0].Path);
            }

            //AppDomain ad = AppDomain.CreateDomain("New domain");
            //UserService repositoryMaster =
            //    (UserService) ad.CreateInstanceAndUnwrap(typeof(UserService).Assembly.FullName, "UserService");

            ////UserService repositoryMaster = new UserService();
            //UserService repositorySlave = new UserService(new Slave());
            //BllUser user_1 = new BllUser
            //{
            //    FirstName = "Mike",
            //    LastName = "Jones",
            //    Cards =
            //    {
            //        new BllVisaRecord { Country = "USA" },
            //        new BllVisaRecord { Country = "China" }
            //    }
            //};
            //BllUser user_2 = new BllUser { FirstName = "Mike", LastName = "Smith" };
            //repositoryMaster.Add(user_1);
            //repositoryMaster.Add(user_2);
            //repositoryMaster.Save();
            //repositorySlave.Load();
            //foreach (var item in repositorySlave.GetAllUsers())
            //{
            //    Console.WriteLine(item);
            //}
        }
    }
}
