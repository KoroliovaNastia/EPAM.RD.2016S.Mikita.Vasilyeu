using System;
using BLL;
using BLL.Models;
using BLL.Modes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;

namespace Tests.BLL
{
    [TestClass]
    public class ModeTests
    {
        [TestInitialize]
        public void ResetStatic()
        {
            typeof(Master).GetProperty("IsActivated").SetValue(Master.Instance, false);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SlaveTriesToAdd_ThrowsException()
        {
            var slaveService = new UserService(new UserRepository(), new Slave());
            slaveService.Add(new BllUser {FirstName = "Mike", LastName = "Jones"});
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SlaveTriesToDelete_ThrowsException()
        {
            var masterService = new UserService();
            var slaveService = new UserService(new UserRepository(), new Slave());
            BllUser user = new BllUser { FirstName = "Mike", LastName = "Jones" };
            int id = masterService.Add(user);
            user.Id = id;
            slaveService.Delete(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateMoreThanOneMaster_ThrowsException()
        {
            var masterServices = new UserService[]
            {
                new UserService(),
                new UserService()
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateMoreThanMaxNumberOfSlaves_ThrowsException()
        {
            var slaveServices = new UserService[]
            {
                new UserService(new UserRepository(), new Slave()),
                new UserService(new UserRepository(), new Slave()),
                new UserService(new UserRepository(), new Slave()),
                new UserService(new UserRepository(), new Slave()),
                new UserService(new UserRepository(), new Slave()),
                new UserService(new UserRepository(), new Slave())
            };
        }
    }
}
