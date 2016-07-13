using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Models;
using DAL;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        [TestMethod]
        public void AddValidUser_ReturnsUserId()
        {
            User user = new User { FirstName = "Mike", LastName = "Jones" };
            UserRepository repository = new UserRepository();
            int id = repository.Add(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNullUser_ThrowsException()
        {
            User user = null;
            UserRepository repository = new UserRepository();
            int id = repository.Add(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddInValidUser_ThrowsException()
        {
            User user = new User { FirstName = "Mike" };
            UserRepository repository = new UserRepository();
            int id = repository.Add(user);
        }

        [TestMethod]
        public void DeleteUser_()
        {
            User user = new User { FirstName = "Mike", LastName = "Jones" };
            UserRepository repository = new UserRepository();
            int id = repository.Add(user);
            repository.Delete(user);
            var ids = repository.SearchForUsers(u => u.Id == id);
            Assert.AreEqual(ids, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNullUser_ThrowsException()
        {
            UserRepository repository = new UserRepository();
            repository.Delete(null);
        }

        [TestMethod]
        public void GetAllUsers_ReturnsAllUsers()
        {
            UserRepository repository = new UserRepository();
            User user = new User { FirstName = "Mike", LastName = "Jones" };
            int id = repository.Add(user);
            var users = repository.GetAllUsers();
            int count = users.Count();
            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        public void SearchForUsers_ReturnsSingleUserId()
        {
            UserRepository repository = new UserRepository();
            User user = new User { FirstName = "Mike", LastName = "Jones" };
            int id = repository.Add(user);
            int[] ids = repository.SearchForUsers(u => u.Id == id);
            Assert.AreEqual(ids[0], 0);
        }

        [TestMethod]
        public void SearchForUsers_ReturnsMultipleUserId()
        {
            UserRepository repository = new UserRepository();
            User user_1 = new User { FirstName = "Mike", LastName = "Jones" };
            User user_2 = new User { FirstName = "Mike", LastName = "Smith" };
            repository.Add(user_1);
            repository.Add(user_2);
            int[] ids = repository.SearchForUsers(u => u.FirstName == "Mike");
            Assert.AreEqual(ids.Count(), 2);
        }
    }
}
