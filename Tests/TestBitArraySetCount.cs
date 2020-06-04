using BinaryNN;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TestBitArraySetCount
    {
        [TestMethod]
        public void Test1()
        {
            Assert.AreEqual(11, new BitArray(new int[] { 
                0b01000000001101100011010001001100 }).CountOnes);
        }
        [TestMethod]
        public void Test2()
        {
            Assert.AreEqual(22, new BitArray(new int[] { 
                0b01000000001101100011010001001100, 
                0b01000000001101100011010001001100 }).CountOnes);
        }
        [TestMethod]
        public void Test3()
        {
            Assert.AreEqual(0, new BitArray(new int[] { 0 }).CountOnes);
        }
        [TestMethod]
        public void Test4()
        {
            Assert.AreEqual(1, new BitArray(new int[] { 
                0b00000000000000000000000000000000, 
                0b00000000000000000000000000000000, 
                0b00000000000000000100000000000000 }).CountOnes);
        }
        [TestMethod]
        public void Test5()
        {
            Assert.AreEqual(1, new BitArray(new int[] {
                0b00000000000000000000000000000000,
                0b00000000000000100000000000000000, 
                0b00000000000000000000000000000000 }).CountOnes);
        }
        [TestMethod]
        public void Test6()
        {
            Assert.AreEqual(1, new BitArray(new int[] {
                0b00000000000001000000000000000000, 
                0b00000000000000000000000000000000, 
                0b00000000000000000000000000000000 }).CountOnes);
        }
        [TestMethod]
        public void Test7()
        {
            Assert.AreEqual(3, new BitArray(new int[] { 
                0b01000000000000000000000000000000, 
                0b01000000000000000000000000000000, 
                0b01000000000000000000000000000000 }).CountOnes);
        }
        [TestMethod]
        public void Test8()
        {
            Assert.AreEqual(32, new BitArray(new int[] { 0, 0, -1 }).CountOnes);
        }
        [TestMethod]
        public void Test9()
        {
            Assert.AreEqual(32, new BitArray(new int[] { 0, -1, 0 }).CountOnes);
        }
        [TestMethod]
        public void Test10()
        {
            Assert.AreEqual(32, new BitArray(new int[] { -1, 0, 0 }).CountOnes);
        }
        [TestMethod]
        public void Test11()
        {
            Assert.AreEqual(96, new BitArray(new int[] { -1, -1, -1 }).CountOnes);
        }
        [TestMethod]
        public void Test12()
        {
            Assert.AreEqual(1, new BitArray(new int[] { 
                0b00000000000000000000000000000000, 
                0b00000000000000000000000000000000, 
                -2147483648 }).CountOnes);
        }
        [TestMethod]
        public void Test13()
        {
            Assert.AreEqual(1, new BitArray(new int[] { 
                0b00000000000000000000000000000000, 
                -2147483648, 
                0b00000000000000000000000000000000 }).CountOnes);
        }
        [TestMethod]
        public void Test14()
        {
            Assert.AreEqual(1, new BitArray(new int[] { 
                -2147483648, 
                0b00000000000000000000000000000000,
                0b00000000000000000000000000000000 }).CountOnes);
        }
        [TestMethod]
        public void Test15()
        {
            Assert.AreEqual(64, new BitArray(new int[] { -1, 0, 0 }).CountZeros);
        }
        [TestMethod]
        public void Test16()
        {
            Assert.AreEqual(64, new BitArray(new int[] { 0, -1, 0 }).CountZeros);
        }
        [TestMethod]
        public void Test17()
        {
            Assert.AreEqual(64, new BitArray(new int[] { 0, 0, -1 }).CountZeros);
        }
        [TestMethod]
        public void Test18()
        {
            Assert.AreEqual(0, new BitArray(new int[] { -1, -1, -1 }).CountZeros);
        }
        [TestMethod]
        public void Test19()
        {
            Assert.AreEqual(5, new BitArray(new int[] { -1 }, 5).CountOnes);
        }


    }
}