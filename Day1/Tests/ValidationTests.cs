using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Models;

namespace Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void UserSimpleValidation_ReturnsTrueForValidUser()
        {
            User user = new User { FirstName = "Mike" };
            bool isValid = User.validator.Validate(user);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void UserSimpleValidation_ReturnsFalseForInvalidUser()
        {
            User user = new User { FirstName = "Mike", LastName = "Jones" };
            bool isValid = User.validator.Validate(user);
            Assert.IsTrue(isValid);
        }
    }
}
