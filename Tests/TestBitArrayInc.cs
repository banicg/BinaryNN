using BinaryNN;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{

    [TestClass]
    public class TestBitArrayInc
    {

        [TestInitialize]
        public void BeforeEachTest()
        {
            //new BitArray(new int[] { -1 }).Inc();
            //new BitArray(new int[] { -2 }).Inc();
            //new BitArray(new int[] { 0x000000ff - 1 }).Inc();
            //new BitArray(new int[] { 0x0000ff00 - 1 }).Inc();
            //new BitArray(new int[] { 0x00ff0000 - 1 }).Inc();
        }

        [TestMethod]
        public void TestOverflowException()
        {
            var a = new BitArray(new int[] { -1 });

            Assert.ThrowsException<OverflowException>(() => a.Inc());
        }

        [TestMethod]
        public void TestOverflowException2()
        {
            Assert.ThrowsException<OverflowException>(() => new BitArray(new int[] { -1, -1 }).Inc());
        }

        [TestMethod]
        public void TestOverflowException3()
        {
            Assert.ThrowsException<OverflowException>(() => new BitArray(new int[] { -1, -1, -1 }).Inc());
        }

        [TestMethod]
        public void TestOverflowException4()
        {
            var a = new BitArray(new int[] { -2, -1, -1 });
            a.Inc();
            Assert.ThrowsException<OverflowException>(() => a.Inc()); ;
        }

        [TestMethod]
        public void Test2()
        {
            var a = new BitArray(new int[] { -2 });
            var t = new BitArray(new int[] { -1 });
            a.Inc();
            Assert.AreEqual(a, t);
        }

        [TestMethod]
        public void Test3()
        {
            new BitArray(new int[] { 0x000000ff - 1 }).Inc();
        }

        [TestMethod]
        public void Test4()
        {
            new BitArray(new int[] { 0x0000ff00 - 1 }).Inc();
        }

        [TestMethod]
        public void Test5()
        {
            new BitArray(new int[] { 0x00ff0000 - 1 }).Inc();
        }
    }
}
