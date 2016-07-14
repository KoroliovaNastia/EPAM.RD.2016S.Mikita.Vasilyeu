using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Models;
using DAL;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Storage;
using DAL.Models;

namespace Tests
{
    [TestClass]
    public class XMLSerializationTests
    {
        [TestMethod]
        public void WriteToXml_()
        {
            UserRepository repository = new UserRepository(new Storage.UserStorage(new EvenEnumerator(), new SimpleUserValidator()));
            UserEntity user_1 = new UserEntity { FirstName = "Mike", LastName = "Jones" };
            UserEntity user_2 = new UserEntity { FirstName = "Mike", LastName = "Smith" };
            repository.Add(user_1);
            repository.Add(user_2);
            repository.Save();
        }

        [TestMethod]
        public void ReadFromXml_()
        {
            UserRepository repository = new UserRepository(new Storage.UserStorage(new EvenEnumerator(), new SimpleUserValidator()));
            UserEntity user_1 = new UserEntity { FirstName = "Mike", LastName = "Jones" };
            UserEntity user_2 = new UserEntity { FirstName = "Mike", LastName = "Smith" };
            repository.Add(user_1);
            repository.Add(user_2);
            repository.Save();
            UserRepository repository_1 = new UserRepository(new Storage.UserStorage(new EvenEnumerator(), new SimpleUserValidator()));
            repository_1.Load();
            Assert.AreEqual(repository_1.GetAllUsers().Count(), 2);
        }
    }
}
