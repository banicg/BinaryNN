using BinaryNN;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{

    [TestClass]
    public class TestBitArrayIndexer
    {

        [TestInitialize]
        public void BeforeEachTest()
        {
        }
        [TestMethod]
        public void TestOverflowException()
        {
            var a = new BitArray(new int[] { 0 });

            Assert.ThrowsException<IndexOutOfRangeException>(() => a[33] = true);
        }

        [TestMethod]
        public void TestOverflowException2()
        {
            var a = new BitArray(new int[] { 0, 0 });

            Assert.ThrowsException<IndexOutOfRangeException>(() => a[65] = true);
        }

        [TestMethod]
        public void TestOverflowException3()
        {
            var a = new BitArray(new int[] { 0 }, 5);

            Assert.ThrowsException<IndexOutOfRangeException>(() => a[5] = true);
        }


        [TestMethod]
        public void Test2()
        {
            var a = new BitArray(new int[] { 0 });
            a[1] = true;
            Assert.IsTrue(a[1]);
        }

        [TestMethod]
        public void Test3()
        {
            var a = new BitArray(new int[] { 0 }, 17);
            a[16] = true;
            Assert.IsTrue(a[16]);
        }

        [TestMethod]
        public void Test4()
        {
            var a = new BitArray(new int[] { 0, 0 }, 40);
            a[35] = true;
            Assert.IsTrue(a[35]);
        }
    }
}
