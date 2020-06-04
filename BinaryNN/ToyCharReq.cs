using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BinaryNN.ToyCharReq
{
    class Toy : IToy
    {
        static Func<int, bool> RndInit = (i) => Randomizer.NextDouble() > 0.5;


        

        public void Run()
        {
            var szIn = 2;
            var szHid = 3;
            var szOut = 1;

            Dictionary<int, int> truthTable = new Dictionary<int, int>();
            //truthTable.Add(0b11, 0b0);
            //truthTable.Add(0b10, 0b1);
            //truthTable.Add(0b01, 0b1);
            //truthTable.Add(0b00, 0b0);

            truthTable.Add(0b11, 0b0);
            truthTable.Add(0b10, 0b0);
            truthTable.Add(0b01, 0b0);
            truthTable.Add(0b00, 0b1);

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
            int maxLoops = 100;
            while (maxLoops > 0)
            {
                maxLoops--;

                //Get a random weight for hidden layer
                WA = new BitArray(szIn * szHid, RndInit);

                //Generate hidden layer output values for each truth table entry
                for (int i = 0; i < I.Length; i++)
                    BinaryNN.XnorAndActivate(WA, I[i], H[i], BinaryNN.SignMid);

                //Make sure hidden layers produce unique values for all the inputs in the truth table
                if(H.Distinct().Count() < H.Length)
                    continue;

                //Find all the weights which produces a valid output for the hidden layer for each truth table entry (this is effectively the back propagation step)
                for (int i = 0; i < I.Length; i++)
                    w[i] = BinaryNN.FindWeights(H[i], O[i], BinaryNN.SignMid);

                //for (int i = 0; i < w.Length; i++)
                //{
                //    Console.Write($"W{i}   : ");
                //    foreach (var W in w[i]) Console.Write($"[{W}] ");
                //    Console.WriteLine();
                //}


                //find a set of weights common to all the thruth table entries
                var wDistinct = w[0].Join(w[1], l => l.AsString(), r => r.AsString(), (l, r) => l);
                for (int i = 2; i < w.Length; i++)
                    wDistinct = wDistinct.Join(w[i], l => l.AsString(), r => r.AsString(), (l, r) => l);

                //Console.Write("WBs: ");
                //foreach (var wb in wDistinct) Console.Write($"[{wb.AsString()}] ");
                //Console.WriteLine();


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
                }
                else
                    Console.WriteLine($"Nothing found for WA: {WA}");


                Console.ReadLine();
            }
        }
    }
}
