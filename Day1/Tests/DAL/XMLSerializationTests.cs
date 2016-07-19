using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL.DTO;
using BLL;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DAL;
using BLL.Models;
using BLL.Modes;

namespace Tests.DAL
{
    [TestClass]
    public class XMLSerializationTests
    {
        [TestInitialize]
        public void ResetStatic()
        {
            typeof(Master).GetProperty("IsActivated").SetValue(Master.Instance, false);
        }

        [TestMethod]
        public void WriteToXml_()
        {
            UserService repository = new UserService();
            BllUser user_1 = new BllUser { FirstName = "Mike", LastName = "Jones" };
            BllUser user_2 = new BllUser { FirstName = "Mike", LastName = "Smith" };
            repository.Add(user_1);
            repository.Add(user_2);
            repository.Save();
        }

        [TestMethod]
        public void ReadFromXml_()
        {
            UserService repository_1 = new UserService();
            BllUser user_1 = new BllUser { FirstName = "Mike", LastName = "Jones" };
            BllUser user_2 = new BllUser { FirstName = "Mike", LastName = "Smith" };
            repository_1.Add(user_1);
            repository_1.Add(user_2);
            repository_1.Save();
            typeof(Master).GetProperty("IsActivated").SetValue(Master.Instance, false);
            UserService repository_2 = new UserService();
            repository_2.Load();
            Assert.AreEqual(repository_2.GetAllUsers().Count(), 2);
        }
    }
}
