using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Models;
using DAL;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Storage;

namespace Tests
{
    [TestClass]
    public class XMLSerializationTests
    {
        [TestMethod]
        public void WriteToXml_()
        {
            UserRepository repository = new UserRepository(new UserStorage(new EvenEnumerator(), new SimpleUserValidator()));
            User user_1 = new User { FirstName = "Mike", LastName = "Jones" };
            User user_2 = new User { FirstName = "Mike", LastName = "Smith" };
            repository.Add(user_1);
            repository.Add(user_2);
            repository.WriteToXmlFile();
        }

        [TestMethod]
        public void ReadFromXml_()
        {
            UserRepository repository = new UserRepository(new UserStorage(new EvenEnumerator(), new SimpleUserValidator()));
            User user_1 = new User { FirstName = "Mike", LastName = "Jones" };
            User user_2 = new User { FirstName = "Mike", LastName = "Smith" };
            repository.Add(user_1);
            repository.Add(user_2);
            repository.WriteToXmlFile();
            UserRepository repository_1 = new UserRepository(new UserStorage(new EvenEnumerator(), new SimpleUserValidator()));
            repository_1.ReadFromXmlFile();
            Assert.AreEqual(repository_1.GetAllUsers().Count(), 2);
        }
    }
}
