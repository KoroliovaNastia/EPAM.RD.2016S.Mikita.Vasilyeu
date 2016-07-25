using BLL;
using BLL.Models;
using BLL.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DomainConfig
{
    public class ThreadInitializer
    {
        private static IEnumerable<BllUser> _userTestCollection = new List<BllUser>
        {
            new BllUser
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                //PersonalId = "MP123",
                BirthDate = DateTime.Now
            },
            new BllUser
            {
                FirstName = "Petr",
                LastName = "Petrov",
                //PersonalId = "MP456",
                BirthDate = DateTime.Now
            },
            new BllUser
            {
                FirstName = "Bob",
                LastName = "Smith",
                //PersonalId = "MP789",
                BirthDate = DateTime.Now
            },
            new BllUser
            {
                FirstName = "Jack",
                LastName = "Jackson",
                //PersonalId = "MP999",
                BirthDate = DateTime.Now
            }
        };

        private static readonly Func<BllUser, bool> _searchFoAllUserPredicate = u => u.FirstName != String.Empty;
        private static string lines { get; } = String.Join("", Enumerable.Repeat("-", 30));

        public static IEnumerable<Thread> InitializeThreads(UserService master, IEnumerable<UserService> slaves)
        {
            IList<Thread> threads = new List<Thread>();
            var masterThread = new Thread(() =>
            {
                BllUser previousUser = null;
                while (true)
                {
                    foreach (var user in _userTestCollection)
                    {
                        master.Add(user);
                        Thread.Sleep(6000);
                        if (previousUser != null)
                        {
                            master.Delete(previousUser);
                        }
                        previousUser = user;
                        Thread.Sleep(6000);
                        Console.WriteLine(lines + "\n" + "Master Search: ");
                        var userIds = master.SearchForUsers(_searchFoAllUserPredicate);
                        Console.Write("User's IDs: ");
                        foreach (var userId in userIds)
                        {
                            Console.Write(userId + " ");
                        }
                        Console.WriteLine("\n" + lines + "\n");
                    }
                }
            });
            masterThread.IsBackground = true;
            masterThread.Start();
            threads.Add(masterThread);

            foreach (var slave in slaves)
            {
                var slaveThread = new Thread(() =>
                {
                    while (true)
                    {
                        var userIds = slave.SearchForUsers(_searchFoAllUserPredicate);
                        Console.WriteLine(lines);
                        //Console.Write(slave.Name + " User's IDs: ");
                        foreach (var user in userIds)
                        {
                            Console.Write(user + " ");
                        }
                        Console.WriteLine("\n" + lines);
                        Thread.Sleep(2000);
                    }

                });
                slaveThread.Start();
                slaveThread.IsBackground = true;
                threads.Add(slaveThread);
            }
            return threads;
        }
    }
}
