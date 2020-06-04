using BinaryNN;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{

    [TestClass]
    public class TestBinaryNN
    {

        static Func<int, bool> RndInit = (i) => Randomizer.NextDouble() > 0.5;

        [TestInitialize]
        public void BeforeEachTest()
        {
        }


        [TestMethod]
        public void TestOverflowException()
        {
            var input = new BitArray(new int[] { 0b00110011 }, 8);
            var output = new BitArray(5);
            var W = new BitArray(new int[] { 0b00110011001100110011001100110011 }, 32);

            Assert.ThrowsException<ArgumentException>(() => {
                BinaryNN.BinaryNN.XnorAndActivate(W, input, output, BinaryNN.BinaryNN.SignHigh);
            });
        }

        [TestMethod]
        public void Test1()
        {
            var input = new BitArray(new int[] { 0b1111_1111 }, 8);
            var output = new BitArray(4);
            var W = new BitArray(new int[] { 0b0000_0000_0000_0000_0000_0000_0000_0000 }, 32 );

            BinaryNN.BinaryNN.XnorAndActivate(W, input, output, BinaryNN.BinaryNN.SignHigh);

            var test = new BitArray(new int[] { 0b0000 }, 4);
            Assert.AreEqual(test, output);
        }

        [TestMethod]
        public void Test2()
        {
            var input = new BitArray(new int[] { 0b0000_0000 }, 8);
            var output = new BitArray(4);
            var W = new BitArray(new int[] { 0b0000_0000_0000_0000_0000_0000_0000_0000 }, 32);
            var test = new BitArray(new int[] { 0b1111 }, 4);

            BinaryNN.BinaryNN.XnorAndActivate(W, input, output, BinaryNN.BinaryNN.SignHigh);

            Assert.AreEqual(test, output);
        }

        [TestMethod]
        public void Test3()
        {
            //Inpt 1010 1010 1010 1010
            //WRow 0000 1010 0000 1010
            //XNOR 0101 1111 0101 1111
            //--------------------------------------------
            //Outp 0    1    0    1

            var input = new BitArray(new int[] { 0b1010 }, 4);
            var output = new BitArray(4);
            var W = new BitArray(new int[] { 0b0000_1010_0000_1010 }, 16);
            var test = new BitArray(new int[] { 0b0101 }, 4);

            BinaryNN.BinaryNN.XnorAndActivate(W, input, output, BinaryNN.BinaryNN.SignHigh);

            Assert.AreEqual(test, output);
        }

        [TestMethod]
        public void Test4()
        {
            //Inpt 10100110 10100110 10100110 10100110 
            //WRow 00000000 00000110 00000000 00000110 
            //XNOR 01011001 01011111 01011001 01011111 
            //--------------------------------------------
            //Outp 0        1        0        1

            var input = new BitArray(new int[] { 0b1010_0110 }, 8);
            var output = new BitArray(4);
            var W = new BitArray(new int[] { 0b0000_0000_0000_0110_0000_0000_0000_0110 }, 32);
            var test = new BitArray(new int[] { 0b0101 }, 4);

            BinaryNN.BinaryNN.XnorAndActivate(W, input, output, BinaryNN.BinaryNN.SignHigh);

            Assert.AreEqual(test, output);
        }

        [TestMethod]
        public void Test5()
        {
            //Inpt 10100110 10100110 10100110 10100110 
            //WRow 00100100 00000110 00000000 00000110 
            //XNOR 01111101 01011111 01011001 01011111 
            //--------------------------------------------
            //Outp 1        1        0        1

            var input = new BitArray(new int[] { 0b1010_0110 }, 8);
            var output = new BitArray(4);
            var W = new BitArray(new int[] { 0b0010_0100_0000_0110_0000_0000_0000_0110 }, 32);
            var test = new BitArray(new int[] { 0b1101 }, 4);

            BinaryNN.BinaryNN.XnorAndActivate(W, input, output, BinaryNN.BinaryNN.SignHigh);

            Assert.AreEqual(test, output);
        }

        [TestMethod]
        public void Test6()
        {
            //Inpt 10100110 10100110 10100110 10100110 10100110 10100110 10100110 10100110 
            //WRow 00100100 00000110 00000000 00000110 00100100 00000110 00000000 00000110 
            //XNOR 01111101 01011111 01011001 01011111 01111101 01011111 01011001 01011111 
            //--------------------------------------------------------------------------------
            //Outp 1        1        0        1        1        1        0        1

            var input = new BitArray(new int[] { 0b1010_0110 }, 8);
            var output = new BitArray(8);
            var W = new BitArray(new int[] { 0b0010_0100_0000_0110_0000_0000_0000_0110, 0b0010_0100_0000_0110_0000_0000_0000_0110 }, 64);
            var test = new BitArray(new int[] { 0b1101_1101 }, 8);

            BinaryNN.BinaryNN.XnorAndActivate(W, input, output, BinaryNN.BinaryNN.SignHigh);

            Assert.AreEqual(test, output);
        }

        [TestMethod]
        public void Test7()
        {
            //Inpt 10100110 10100110 10100110 10100110 10100110 10100110 10100110 
            //WRow 00000110_00000000_00000110 00100100_00000110_00000000_00000110 
            //XNOR 01011111 01011001 01011111 01111101 01011111 01011001 01011111 
            //-----------------------------------------------------------------------
            //Outp 1        0        1        1        1        0        1

            var input = new BitArray(new int[] { 0b1010_0110 }, 8);
            var output = new BitArray(7);
            var W = new BitArray(new int[] { 0b0010_0100_0000_0110_0000_0000_0000_0110, 0b0000_0110_0000_0000_0000_0110 }, 56);
            var test = new BitArray(new int[] { 0b101_1101 }, 7);

            BinaryNN.BinaryNN.XnorAndActivate(W, input, output, BinaryNN.BinaryNN.SignHigh);

            Assert.AreEqual(test, output);
        }
    }
}