using BinaryNN;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{

    [TestClass]
    public class TestBitArrayLogic
    {
        BitArray a;

        BitArray b;


        [TestInitialize]
        public void BeforeEachTest()
        {
            a = new BitArray(new int[] { 0b0101 }, 4);

            b = new BitArray(new int[] { 0b0110 }, 4);
        }

        [TestMethod]
        public void TestNor()
        {
            //a:0b0101
            //b:0b0110
            //c:0b1000
            var c = a.Nor(b);
            BitArray t = new BitArray(new int[] { 0b1000 }, 4);
            Assert.IsTrue(t == c);
        }

        [TestMethod]
        public void TestNot()
        {
            //a:0b0101
            //c:0b1010
            var c = a.Not();
            BitArray t = new BitArray(new int[] { 0b1010 }, 4);
            Assert.IsTrue(t == c);
        }

        [TestMethod]
        public void TestOr()
        {
            //a:0b0101
            //b:0b0110
            //c:0b0111
            var c = a.Or(b);
            BitArray t = new BitArray(new int[] { 0b0111 }, 4);
            Assert.IsTrue(t == c);
        }
        [TestMethod]
        public void TestXnor()
        {
            //a:0b0101
            //b:0b0110
            //c:0b1100
            var c = a.Xnor(b);
            BitArray t = new BitArray(new int[] { 0b1100 }, 4);
            Assert.IsTrue(t == c);
        }
        [TestMethod]
        public void TestXor()
        {
            //a:0b0101
            //b:0b0110
            //c:0b0011
            var c = a.Xor(b);
            BitArray t = new BitArray(new int[] { 0b0011 }, 4);
            Assert.IsTrue(t == c);
        }

        [TestMethod]
        public void TestAnd()
        {
            //a:0b0101
            //b:0b0110
            //c:0b0100
            var c = a.And(b);
            BitArray t = new BitArray(new int[] { 0b0100 }, 4);
            Assert.IsTrue(t == c);
        }

        [TestMethod]
        public void TestNand()
        {
            //a:0b0101
            //b:0b0110
            //c:0b1011
            var c = a.Nand(b);
            BitArray t = new BitArray(new int[] { 0b1011 }, 4);
            Assert.IsTrue(t == c);
        }
    }
}
