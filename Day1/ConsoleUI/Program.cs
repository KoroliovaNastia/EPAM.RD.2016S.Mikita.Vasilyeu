using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Heap;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User { FirstName = "Mike", LastName = "Jones" };
            User user1 = new User { FirstName = "Mike", LastName = "Mayers" };
            User user2 = new User { FirstName = "Kel", LastName = "Mitchell" };
            Console.WriteLine(user);
            Console.WriteLine(user2);
            Console.WriteLine(User.Equals(user,user1));
            Console.WriteLine(user.Equals(user1));
            Console.WriteLine();

            UserRepository repository = new UserRepository();
            Console.WriteLine(repository.Add(user));
            Console.WriteLine(repository.Add(user1));
            Console.WriteLine(repository.Add(user2));
            Console.WriteLine();
            foreach (var item in repository.GetAllUsers())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            int[] ids = repository.SearchForUser(u => u.FirstName == "Mike");
            foreach (var item in ids)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            repository.Delete(user);
            foreach (var item in repository.GetAllUsers())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();



        }
    }
}
