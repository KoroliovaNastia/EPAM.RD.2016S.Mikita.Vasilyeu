using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;

namespace Tests.DAL
{
    [TestClass]
    public class EvenEnumeratorTests
    {
        [TestMethod]
        public void GetDefaultState_ReturnMinusTwo()
        {
            EvenEnumerator iterator = new EvenEnumerator();
            Assert.AreEqual(iterator.Current, -2);
        }

        [TestMethod]
        public void GetFirstEvenNumber_ReturnZero()
        {
            EvenEnumerator iterator = new EvenEnumerator();
            iterator.MoveNext();
            Assert.AreEqual(iterator.Current, 0);
        }

        [TestMethod]
        public void GetLastEvenNumber_ReturnIntMaxValueMinusOne()
        {
            EvenEnumerator iterator = new EvenEnumerator();
            while (iterator.MoveNext()) ;
            Assert.AreEqual(iterator.Current, int.MaxValue - 1);
        }
    }
}
