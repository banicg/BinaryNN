using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryNN
{
    class ToySolveXOR : IToy
    {
        static Func<int, bool> RndInit = (i) => Randomizer.NextDouble() > 0.5;

        public int SmallToy { get; set; } = 1;

        public void Run( )
        {

            var szIn = 0;
            var szHid = 0;
            var szOut = 0;

            Dictionary<int, int> truthTable = new Dictionary<int, int>();


            if (SmallToy == 0)
            {
                szIn = 2;
                szHid = 3;
                szOut = 1;
                truthTable.Add(0b11, 0b0);
                truthTable.Add(0b10, 0b1);
                truthTable.Add(0b01, 0b1);
                truthTable.Add(0b00, 0b0);
            }
            else if (SmallToy == 1)
            {
                szIn = 5;
                szHid = 11;
                szOut = 1;

                //NOR
                truthTable.Add(0b00011, 0b0);
                truthTable.Add(0b00010, 0b0);
                truthTable.Add(0b00001, 0b0);
                truthTable.Add(0b00000, 0b1);

                //XOR
                truthTable.Add(0b00111, 0b0);
                truthTable.Add(0b00110, 0b1);
                truthTable.Add(0b00101, 0b1);
                truthTable.Add(0b00100, 0b0);

                //NXOR
                truthTable.Add(0b01011, 0b1);
                truthTable.Add(0b01010, 0b0);
                truthTable.Add(0b01001, 0b0);
                truthTable.Add(0b01000, 0b1);

                //OR
                truthTable.Add(0b01111, 0b1);
                truthTable.Add(0b01110, 0b1);
                truthTable.Add(0b01101, 0b1);
                truthTable.Add(0b01100, 0b0);


                //AND
                truthTable.Add(0b10011, 0b1);
                truthTable.Add(0b10010, 0b0);
                truthTable.Add(0b10001, 0b0);
                truthTable.Add(0b10000, 0b0);
            }




            BitArray[] I = new BitArray[truthTable.Count];
            BitArray[] O = new BitArray[truthTable.Count];
            BitArray[] H = new BitArray[truthTable.Count];
            IEnumerable<BitArray>[] w = new IEnumerable<BitArray>[truthTable.Count];
            int iTT = 0;
            foreach (var tti in truthTable)
            {
                I[iTT] = new BitArray(new int[] { tti.Key }, szIn);
                O[iTT] = new BitArray(new int[] { tti.Value }, szOut);
                H[iTT] = new BitArray(szHid);

                iTT++;
            }

            BitArray WA = null;
            BitArray WB = new BitArray(szHid * szOut, RndInit);
            var output = new BitArray(szOut);
            int loops = 0;
            while (loops < int.MaxValue)
            {
                loops++;

                //Get random weights for hidden layer
                WA = new BitArray(szIn * szHid, RndInit);

                //Generate hidden layer output values for each truth table entry
                for (int i = 0; i < I.Length; i++)
                    BinaryNN.XnorAndActivate(WA, I[i], H[i], BinaryNN.SignMid);

                //Make sure hidden layers produce unique values for all the inputs in the truth table
                if (H.Distinct().Count() < H.Length)
                    continue;

                //Find all the weights which produces a valid output for the hidden layer for each truth table entry (this is effectively the back propagation step)
                for (int i = 0; i < I.Length; i++)
                    w[i] = BinaryNN.FindWeights(H[i], O[i], BinaryNN.SignMid);

                //find a set of weights common to all the thruth table entries
                var wDistinct = w[0].Join(w[1], l => l.AsString(), r => r.AsString(), (l, r) => l);
                for (int i = 2; i < w.Length; i++)
                    wDistinct = wDistinct.Join(w[i], l => l.AsString(), r => r.AsString(), (l, r) => l);


                if (wDistinct.Count() > 0)
                {
                    WB = wDistinct.First();

                    foreach (var tti in truthTable)
                    {
                        var input = new BitArray(new int[] { tti.Key }, szIn);
                        var hidden = new BitArray(szHid);
                        BinaryNN.XnorAndActivate(WA, input, hidden, BinaryNN.SignMid);
                        BinaryNN.XnorAndActivate(WB, hidden, output, BinaryNN.SignMid);
                        Console.WriteLine($"{input} -> {output} (Should be {new BitArray(new int[] { tti.Value }, output.Length)})");
                    }

                    Console.WriteLine($"WA: {WA}");
                    Console.WriteLine($"WB: {WB}");
                    Console.WriteLine($"Loop {loops}");

                    Console.ReadLine();
                }
                else
                {
                    if (loops % 100 == 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"Nothing found for WA: {WA} (Loop {loops})");
                    }

                    //Console.WriteLine($"Nothing found for WA: {WA}");
                }
            }
        }
    }
}
