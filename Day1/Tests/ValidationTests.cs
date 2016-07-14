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
            UserStorage storage = new UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            User user = new User { FirstName = "Mike" };
            bool isValid = storage.validator.Validate(user);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void UserSimpleValidation_ReturnsFalseForInvalidUser()
        {
            UserStorage storage = new UserStorage(new EvenEnumerator(), new SimpleUserValidator());
            User user = new User { FirstName = "Mike", LastName = "Jones"};
            bool isValid = storage.validator.Validate(user);
            Assert.IsTrue(isValid);
        }
    }
}
