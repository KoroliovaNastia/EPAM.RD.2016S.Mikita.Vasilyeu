using System;
using BLL;
using BLL.Models;
using BLL.Modes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;

namespace Tests.Storage.Tests
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
            var slaveRepository = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), new Slave());
            slaveRepository.Add(new BllUser {FirstName = "Mike", LastName = "Jones"});
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SlaveTriesToDelete_ThrowsException()
        {
            var masterRepository = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), Master.Instance);
            var slaveRepository = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), new Slave());
            BllUser user = new BllUser { FirstName = "Mike", LastName = "Jones" };
            int id = masterRepository.Add(user);
            user.Id = id;
            slaveRepository.Delete(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateMoreThanOneMaster_ThrowsException()
        {
            var masterRepository_1 = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), Master.Instance);
            var masterRepository_2 = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), Master.Instance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateMoreThanMaxNumberOfSlaves_ThrowsException()
        {
            var slaveRepository_1 = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), new Slave());
            var slaveRepository_2 = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), new Slave());
            var slaveRepository_3 = new UserService(new UserRepository(new EvenEnumerator(), new SimpleUserValidator()), new Slave());
        }
    }
}
