using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL.DTO;

namespace Tests
{
    [TestClass]
    public class UserEntityTests
    {
        [TestMethod]
        public void UserEqualityForEqualUsers_ReturnsTrue()
        {
            DalUser one = new DalUser { FirstName = "Mike", LastName = "Jones" };
            DalUser two = new DalUser { FirstName = "Mike", LastName = "Jones" };
            Assert.IsTrue(one.Equals(two));
        }

        [TestMethod]
        public void UserEqualityForNonEqualUsers_ReturnsFalse()
        {
            DalUser one = new DalUser { FirstName = "Mike", LastName = "Jones" };
            DalUser two = new DalUser { FirstName = "Mike", LastName = "White" };
            Assert.IsFalse(one.Equals(two));
        }

        [TestMethod]
        public void UserHashCode_ReturnsSameValuesForSameUsers()
        {
            DalUser one = new DalUser { Id = 0, FirstName = "Mike", LastName = "Jones" };
            DalUser two = one;
            Assert.AreEqual(one.GetHashCode(), two.GetHashCode());
        }

        [TestMethod]
        public void UserHashCode_ReturnsDifferentValuesForDifferentUsers()
        {
            DalUser one = new DalUser { Id = 0, FirstName = "Mike", LastName = "Jones" };
            DalUser two = new DalUser { Id = 2, FirstName = "Mike", LastName = "Jones" };
            Assert.AreNotEqual(one.GetHashCode(), two.GetHashCode());
        }
    }
}
