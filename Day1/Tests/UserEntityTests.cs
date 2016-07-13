using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Models;

namespace Tests
{
    [TestClass]
    public class UserEntityTests
    {
        [TestMethod]
        public void UserEqualityForEqualUsers_ReturnsTrue()
        {
            User one = new User { FirstName = "Mike", LastName = "Jones" };
            User two = new User { FirstName = "Mike", LastName = "Jones" };
            Assert.IsTrue(one.Equals(two));
        }

        [TestMethod]
        public void UserEqualityForNonEqualUsers_ReturnsFalse()
        {
            User one = new User { FirstName = "Mike", LastName = "Jones" };
            User two = new User { FirstName = "Mike", LastName = "White" };
            Assert.IsFalse(one.Equals(two));
        }

        [TestMethod]
        public void UserHashCode_ReturnsSameValuesForSameUsers()
        {
            User one = new User { FirstName = "Mike", LastName = "Jones" };
            User two = one;
            Assert.AreEqual(one.GetHashCode(), two.GetHashCode());
        }

        [TestMethod]
        public void UserHashCode_ReturnsDifferentValuesForDifferentUsers()
        {
            User one = new User { FirstName = "Mike", LastName = "Jones" };
            User two = new User { FirstName = "Mike", LastName = "Jones" };
            Assert.AreNotEqual(one.GetHashCode(), two.GetHashCode());
        }
    }
}
