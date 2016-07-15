using System;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Models;
using Storage;

namespace Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void UserSimpleValidation_ReturnsTrueForValidUser()
        {
            Storage.UserStorage storage = new Storage.UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            User user = new User { FirstName = "Mike" };
            bool isValid = storage.Validator.Validate(user);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void UserSimpleValidation_ReturnsFalseForInvalidUser()
        {
            Storage.UserStorage storage = new Storage.UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            User user = new User { FirstName = "Mike", LastName = "Jones"};
            bool isValid = storage.Validator.Validate(user);
            Assert.IsTrue(isValid);
        }
    }
}
