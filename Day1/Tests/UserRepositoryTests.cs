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
    public class UserRepositoryTests
    {
        [TestMethod]
        public void AddValidUser_ReturnsUserId()
        {
            User user = new User { FirstName = "Mike", LastName = "Jones" };
            UserStorage st = new UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            int id = st.Add(user);
            Assert.AreEqual(id, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNullUser_ThrowsException()
        {
            User user = null;
            UserStorage st = new UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            int id = st.Add(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddInValidUser_ThrowsException()
        {
            User user = new User { FirstName = "Mike" };
            UserStorage st = new UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            int id = st.Add(user);
        }

        [TestMethod]
        public void DeleteUser_()
        {
            User user = new User { FirstName = "Mike", LastName = "Jones" };
            UserStorage st = new UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            int id = st.Add(user);
            st.Delete(user);
            var ids = st.GetByPredicate(u => u.Id == id);
            Assert.AreEqual(ids, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNullUser_ThrowsException()
        {
            UserStorage st = new UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            st.Delete(null);
        }

        [TestMethod]
        public void GetAllUsers_ReturnsAllUsers()
        {
            UserStorage st = new UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            User user = new User { FirstName = "Mike", LastName = "Jones" };
            int id = st.Add(user);
            var users = st.GetAll();
            int count = users.Count();
            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        public void SearchForUsers_ReturnsSingleUserId()
        {
            UserStorage st = new UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            User user = new User { FirstName = "Mike", LastName = "Jones" };
            int id = st.Add(user);
            int[] ids = st.GetByPredicate(u => u.Id == id);
            //Assert.AreEqual(ids[0], 0);
            Assert.AreEqual(ids.Count(), 1);
        }

        [TestMethod]
        public void SearchForUsers_ReturnsMultipleUserId()
        {
            UserStorage st = new UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            User user_1 = new User { FirstName = "Mike", LastName = "Jones" };
            User user_2 = new User { FirstName = "Mike", LastName = "Smith" };
            st.Add(user_1);
            st.Add(user_2);
            int[] ids = st.GetByPredicate(u => u.FirstName == "Mike");
            Assert.AreEqual(ids.Count(), 2);
        }
    }
}
