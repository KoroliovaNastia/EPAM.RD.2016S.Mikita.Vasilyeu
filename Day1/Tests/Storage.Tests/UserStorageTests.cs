using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL.DTO;
using BLL;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DAL;
using BLL.Models;

namespace Tests
{
    [TestClass]
    public class UserStorageTests
    {
        [TestMethod]
        public void AddValidUser_ReturnsUserId()
        {
            DalUser user = new DalUser { FirstName = "Mike", LastName = "Jones" };
            UserRepository st = new UserRepository(new EvenEnumerator(), new SimpleUserValidator());
            int id = st.Add(user);
            Assert.AreEqual(id, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNullUser_ThrowsException()
        {
            DalUser user = null;
            UserRepository st = new UserRepository(new EvenEnumerator(), new SimpleUserValidator());
            int id = st.Add(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddInValidUser_ThrowsException()
        {
            DalUser user = new DalUser { FirstName = "Mike" };
            UserRepository st = new UserRepository(new EvenEnumerator(), new SimpleUserValidator());
            int id = st.Add(user);
        }

        [TestMethod]
        public void DeleteUser_()
        {
            DalUser user = new DalUser { FirstName = "Mike", LastName = "Jones" };
            UserRepository st = new UserRepository(new EvenEnumerator(), new SimpleUserValidator());
            int id = st.Add(user);
            st.Delete(user);
            var ids = st.GetByPredicate(u => u.Id == id);
            Assert.AreEqual(ids, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNullUser_ThrowsException()
        {
            UserRepository st = new UserRepository(new EvenEnumerator(), new SimpleUserValidator());
            st.Delete(null);
        }

        [TestMethod]
        public void GetAllUsers_ReturnsAllUsers()
        {
            UserRepository st = new UserRepository(new EvenEnumerator(), new SimpleUserValidator());
            DalUser user = new DalUser { FirstName = "Mike", LastName = "Jones" };
            int id = st.Add(user);
            var users = st.GetAll();
            int count = users.Count();
            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        public void SearchForUsers_ReturnsSingleUserId()
        {
            UserRepository st = new UserRepository(new EvenEnumerator(), new SimpleUserValidator());
            DalUser user = new DalUser { FirstName = "Mike", LastName = "Jones" };
            int id = st.Add(user);
            int[] ids = st.GetByPredicate(u => u.Id == id);
            //Assert.AreEqual(ids[0], 0);
            Assert.AreEqual(ids.Count(), 1);
        }

        [TestMethod]
        public void SearchForUsers_ReturnsMultipleUserId()
        {
            UserRepository st = new UserRepository(new EvenEnumerator(), new SimpleUserValidator());
            DalUser user_1 = new DalUser { FirstName = "Mike", LastName = "Jones" };
            DalUser user_2 = new DalUser { FirstName = "Mike", LastName = "Smith" };
            st.Add(user_1);
            st.Add(user_2);
            int[] ids = st.GetByPredicate(u => u.FirstName == "Mike");
            Assert.AreEqual(ids.Count(), 2);
        }
    }
}
