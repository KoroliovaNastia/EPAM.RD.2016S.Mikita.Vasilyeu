using System;
using BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL.DTO;
using DAL;

namespace Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void UserSimpleValidation_ReturnsTrueForValidUser()
        {
            UserRepository storage = new UserRepository(new EvenEnumerator(), new SimpleUserValidator());
            DalUser user = new DalUser { FirstName = "Mike" };
            bool isValid = storage.Validator.Validate(user);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void UserSimpleValidation_ReturnsFalseForInvalidUser()
        {
            UserRepository storage = new UserRepository(new EvenEnumerator(), new SimpleUserValidator());
            DalUser user = new DalUser { FirstName = "Mike", LastName = "Jones"};
            bool isValid = storage.Validator.Validate(user);
            Assert.IsTrue(isValid);
        }
    }
}
