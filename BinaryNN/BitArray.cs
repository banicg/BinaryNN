using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace BinaryNN
{


    public class BitArray : IEnumerable<bool>
    {
        private int[] data;
        private int data_Length;

        public int[] RawData => data;

        private int _length;
        public int Length { get => _length; private set => _length = value; }

        private uint mask;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bits">MSB to left of string</param>
        public BitArray(string bits)
            : this(bits.Length)
        {
            char[] bitArray = bits.Reverse().ToArray();
            for (int i = 0; i < bitArray.Length; i++)
            {
                this[i] = bitArray[i] == '1';
            }
        }

        public string AsString()
        {
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < _length; i++)
            {
                s.Append(this[i] ? '1' : '0');
            }
            return s.ToString();
        }

        public BitArray(int[] data)
            : this(data, data.Length * 32)
        {
        }
        public BitArray(BitArray ba)
           : this(ba.data, ba._length)
        {
        }

        public BitArray(int[] data, int length)
            : this(length)
        {
            if (length > data_Length * 32)
                throw new ArgumentException("Length more than the data array length x 32.");

            for (int i = 0; i < data_Length; i++)
            {
                this.data[i] = data[i];
            }
        }

        public BitArray(int length, Func<int, bool> R)
            : this(length)
        {

            for (int i = 0; i < _length; i++)
            {
                this[i] = R(i);
            }
        }

        public BitArray(int length)
        {
            if (length < 0)
                throw new ArgumentException("Length must be positive");

            _length = length;

            mask = 0xffffffff >> (32 - length % 32);

            data_Length = length / 32 + (length % 32 == 0 ? 0 : 1);
            data = new int[data_Length];
        }

        #region Derived BitArray

        public void CopyTo(int srcIndex, BitArray dest, int destIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                dest[destIndex + i] = this[srcIndex + i];
            }
        }

        public BitArray Splice(int start, int length)
        {
            if (Length < start + length)
                throw new ArgumentException($"Length must be larger or equal to start + length.");

            if (start < 0)
                throw new ArgumentException("start parameter must be positive");

            var c = new BitArray(length);

            for (int i = start; i < start + length; i++)
                c[i - start] = this[i];

            return c;
        }


        //private BitArray(BitArray ba, int start, int length)
        //{

        //    if (Length < ba.start + start)
        //        throw new ArgumentException($"Length must be larger or equal to start + length.");

        //    this.data = ba.data;
        //    this.start = ba.start + start;
        //    this.length = length;

        //}

        //int start, length;
        //public BitArray Splice2(int start, int length)
        //{
        //    if (Length < start + length)
        //        throw new ArgumentException($"Length must be larger or equal to start + length.");

        //    if (start < 0)
        //        throw new ArgumentException("start parameter must be positive");

        //    var c = new BitArray(length);

        //    for (int i = start; i < start + length; i++)
        //        c[i - start] = this[i];

        //    return c;
        //}

        public BitArray Clone()
        {
            return new BitArray(this);
        }

        #endregion


        #region Modifiers

        public void Set(int[] data, int length)
        {
            if (length > data_Length * 32)
                throw new ArgumentException("Length more than the data array length x 32.");

            for (int i = 0; i < data_Length; i++)
            {
                this.data[i] = data[i];
            }
        }

        public bool this[int x, int y]
        {
            get
            {
                if (x * y > _length - 1)
                    throw new IndexOutOfRangeException();

                int w = (int)Math.Sqrt(_length);

                return this[w * y + x];
            }
            set
            {
                if (x * y > _length - 1)
                    throw new IndexOutOfRangeException();

                int w = (int)Math.Sqrt(_length);

                this[w * y + x] = value;
            }
        }

        public bool this[int i]
        {
            get
            {
                if (i > _length - 1)
                    throw new IndexOutOfRangeException();

                return (data[i / 32] & (1 << (i % 32))) != 0;
            }
            set
            {
                if (i > _length - 1)
                    throw new IndexOutOfRangeException();

                if (value)
                    data[i / 32] |= 1 << (i % 32);
                else
                    data[i / 32] &= ~(1 << (i % 32));
            }
        }

        public void Inc()
        {
            //var oldVal = (int[])data.Clone();
            for (int i = 0; i < data_Length; i++)
            {
                if ((uint)data[i] == 0xffffffff)
                {
                    if (i + 1 < data_Length)
                    {
                        data[i] = 0;
                        continue;
                    }
                    else
                    {
                        //var s = "";
                        //for (int e = 0; e < oldVal.Length; e++)
                        //{
                        //    if (e > 0)
                        //        s += ',';

                        //    s += $"{Convert.ToString(oldVal[e], 2)}";
                        //}
                        //File.WriteAllText($"IntError-({s}).txt", s);
                        throw new OverflowException();
                    }
                }
                else
                {
                    data[i]++;
                    break;
                }
            }

        }

        #endregion


        #region Enumeration and Bit counting

        public int CountOnes
        {
            get
            {
                //Modified from Brian Kernighan’s Algorithm @ https://www.geeksforgeeks.org/count-set-bits-in-an-integer/
                int total = 0;
                for (int i = 0; i < data_Length; i++)
                {
                    uint n = (uint)data[i];

                    //Mask the last int to the remaining length
                    //int usedBits = Length % 32;
                    //uint mask = 0xffffffff >> (32 - usedBits);
                    //n &= mask;
                    if (i == data_Length - 1)
                        n &= mask;

                    int count = 0;
                    while (n > 0)
                    {
                        n &= n - 1;
                        count++;
                    }
                    total += count;
                }

                return total;
            }
        }

        public int CountOnesOld
        {
            get
            {
                int tmpVal = 0;
                for (int i = 0; i < Length; i++)
                {
                    tmpVal += this[i] ? 1 : 0;
                }

                return tmpVal;
            }
        }

        public int CountZeros
        {
            get
            {
                return Length - CountOnes;
            }
        }

        public IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < _length; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<bool> Values()
        {
            for (int i = 0; i < _length; i++)
                yield return this[i];
        }

        public List<bool> ToList()
        {
            return Values().ToList();
        }

        public bool[] ToArray()
        {
            return Values().ToArray();
        }

        #endregion


        #region Logical Methods

        public BitArray Xnor(BitArray b)
        {
            if (b._length != _length)
                throw new ArgumentException($"Length must be equal to {Length}");

            var c = new BitArray(_length);


            for (int i = 0; i < data_Length; i++)
            {
                if (i == data_Length - 1)
                    c.data[i] = (int)~(((uint)data[i] & mask) ^ ((uint)b.data[i] & mask));
                else
                    c.data[i] = ~(data[i] ^ b.data[i]);
            }

            return c;
        }


        public BitArray Or(BitArray b)
        {
            if (b._length != _length)
                throw new ArgumentException($"Length must be equal to {_length}");

            var c = new BitArray(_length);

            for (int i = 0; i < data_Length; i++)
            {
                if (i == data_Length - 1)
                    c.data[i] = (int)(((uint)data[i] & mask) | ((uint)b.data[i] & mask));
                else
                    c.data[i] = data[i] | b.data[i];
            }

            return c;
        }

        public BitArray And(BitArray b)
        {
            if (b._length != _length)
                throw new ArgumentException($"Length must be equal to {_length}");

            var c = new BitArray(_length);

            for (int i = 0; i < data_Length; i++)
            {
                if (i == data_Length - 1)
                    c.data[i] = (int)(((uint)data[i] & mask) & ((uint)b.data[i] & mask));
                else
                    c.data[i] = data[i] & b.data[i];
            }

            return c;
        }

        public BitArray Nand(BitArray b)
        {
            if (b.Length != _length)
                throw new ArgumentException($"Length must be equal to {_length}");

            var c = new BitArray(_length);

            for (int i = 0; i < data_Length; i++)
            {
                if (i == data_Length - 1)
                    c.data[i] = (int)~(((uint)data[i] & mask) & ((uint)b.data[i] & mask));
                else
                    c.data[i] = ~(data[i] & b.data[i]);
            }

            return c;
        }

        public BitArray Xor(BitArray b)
        {
            if (b._length != _length)
                throw new ArgumentException($"Length must be equal to {_length}");

            var c = new BitArray(_length);

            for (int i = 0; i < data_Length; i++)
            {
                if (i == data_Length - 1)
                    c.data[i] = (int)(((uint)data[i] & mask) ^ ((uint)b.data[i] & mask));
                else
                    c.data[i] = data[i] ^ b.data[i];
            }

            return c;
        }

        public BitArray Nor(BitArray b)
        {
            if (b._length != _length)
                throw new ArgumentException($"Length must be equal to {_length}");

            var c = new BitArray(_length);

            for (int i = 0; i < data_Length; i++)
            {
                if (i == data_Length - 1)
                    c.data[i] = (int)~(((uint)data[i] & mask) | ((uint)b.data[i] & mask));
                else
                    c.data[i] = ~(data[i] | b.data[i]);
            }

            return c;
        }

        public BitArray Not()
        {
            var c = new BitArray(_length);

            for (int i = 0; i < data_Length; i++)
            {
                if (i == data_Length - 1)
                    c.data[i] = (int)~((uint)data[i]);
                else
                    c.data[i] = ~data[i];
            }

            return c;
        }

        #endregion


        #region Genetics

        public void Mutate(float probability = 0.001f)
        {
            for (int i = 0; i < _length; i++)
            {
                if (Randomizer.NextFloat() < probability)
                    this[i] = !this[i];
            }
        }
        public BitArray Crossover(BitArray partner)
        {
            var child = new BitArray(partner._length);

            if (child.data_Length >= 8)
                for (int i = 0; i < child.data_Length; i++)
                {
                    child.data[i] = Randomizer.NextBool() ? this.data[i] : partner.data[i];
                }
            else //if (child.data_Length >= 4)
                for (int i = 0; i < child.data_Length; i++)
                {
                    int X = Randomizer.NextInt(0, 6);
                    uint maskB;
                    uint maskA;
                    switch (X)
                    {
                        case 0:
                            maskA = 0xff00ff00;
                            maskB = 0x00ff00ff;
                            break;
                        case 1:
                            maskA = 0x00ff00ff;
                            maskB = 0xff00ff00;
                            break;
                        case 2:
                            maskA = 0xffff0000;
                            maskB = 0x0000ffff;
                            break;
                        case 3:
                            maskA = 0x00ffff00;
                            maskB = 0xff0000ff;
                            break;
                        case 4:
                            maskA = 0x0000ffff;
                            maskB = 0xffff0000;
                            break;
                        default:
                            maskA = 0xff0000ff;
                            maskB = 0x00ffff00;
                            break;
                    }

                    uint newVal = ((uint)data[i] & maskA) | ((uint)partner.data[i] & maskB);

                    child.data[i] = (int)newVal;


                    //Console.WriteLine();
                    //Console.WriteLine();

                    //Console.WriteLine($"A         : {ToString((uint)data[i])}");
                    //Console.WriteLine($"maskA     : {ToString(maskA)}");
                    //Console.WriteLine($"mask & A  : {ToString((uint)data[i] & maskA)}");
                    //Console.WriteLine();

                    //Console.WriteLine($"B         : {ToString((uint)partner.data[i])}");
                    //Console.WriteLine($"maskB     : {ToString(maskB)}");
                    //Console.WriteLine($"mask & B  : {ToString((uint)partner.data[i] & maskB)}");
                    //Console.WriteLine();

                    //Console.WriteLine($"mA | mB   : {ToString(newVal)}");
                    //Console.WriteLine($"mA | mB 2 : {ToString2((int)newVal)}");
                }
            //else
            //    for (int i = 0; i < child.Length; i++)
            //    {
            //        child[i] = Randomizer.NextBool() ? this[i] : partner[i];
            //    }

            return child;

            //string ToString(uint i)
            //{
            //    return $"{Convert.ToString(i, 2).PadLeft(32, '0')}";
            //}

            //string ToString2(int i)
            //{
            //    return ToString((uint)i);
            //}
        }

        #endregion


        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < _length; i++)
            {
                if (i % 4 == 0)
                    s.Append(' ');

                s.Append(this[i] ? '1' : '0');
            }
            return new string(s.ToString().Reverse().ToArray());
        }


        //public override string ToString()
        //{
        //    StringBuilder s = new StringBuilder();

        //    for (int i = 0; i < data_Length; i++)
        //    {
        //        s.Append($"{i}:{Convert.ToString(data[i], 2)}");
        //    }
        //    return s.ToString();
        //}

        public static bool operator ==(BitArray obj1, BitArray obj2)
        {
            if (ReferenceEquals(obj1, obj2)) return true;
            if (ReferenceEquals(obj1, null)) return false; //* - .Equals() performs the null check on obj2
            return obj1.Equals(obj2);
        }
        public static bool operator !=(BitArray obj1, BitArray obj2)
        {
            return !(obj1 == obj2);
        }


        public override bool Equals(object other)
        {
            if (other is null)
                return false;

            var otherBA = other as BitArray;

            if (_length != otherBA._length)
                return false;

            for (int i = 0; i < data_Length; i++)
                if (((uint)data[i] & mask) != ((uint)otherBA.data[i] & otherBA.mask))
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < data_Length; i++)
                hash ^= data[i];

            return hash;
        }
    }

}
