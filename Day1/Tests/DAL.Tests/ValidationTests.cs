using System;
using BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL.DTO;
using DAL;

namespace Tests.DAL
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void UserSimpleValidation_ReturnsTrueForValidUser()
        {
            DalUser user = new DalUser { FirstName = "Mike" };
            SimpleUserValidator validator = new SimpleUserValidator();
            bool isValid = validator.Validate(user);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void UserSimpleValidation_ReturnsFalseForInvalidUser()
        {
            DalUser user = new DalUser { FirstName = "Mike", LastName = "Jones"};
            SimpleUserValidator validator = new SimpleUserValidator();
            bool isValid = validator.Validate(user);
            Assert.IsTrue(isValid);
        }
    }
}
