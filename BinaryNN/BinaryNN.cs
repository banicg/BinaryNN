using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryNN
{
    public static class BinaryNN
    {
        //https://arxiv.org/pdf/1601.06071.pdf

        public static BitArray XnorAndActivate(BitArray W, BitArray Input, BitArray output, Func<BitArray, bool> activation)
        {
            var lenOut = W.Length / Input.Length;
            var lenI = Input.Length;
            var lenO = output.Length;

            if (lenO != lenOut)
                throw new ArgumentException($"Output length must be {lenOut} but is {lenO}");


            for (int j = 0; j < lenOut; j++)
            {
                var tmpA = W.Splice(j * lenI, lenI);
                var tmpB = tmpA.Xnor(Input);

                output[j] = activation(tmpB);
            }

            return output;
        }


        public static bool SignHigh(BitArray b)
        {
            return 2 * (b.CountOnes - 1) > b.Length; //equivalent to (b.CountOnes > b.Length / 2 + 1) but without a division
        }


        public static bool SignMid(BitArray b)
        {
            return 2 * (b.CountOnes) > b.Length; //equivalent to (b.CountOnes > b.Length / 2 ) but without a division
        }


        public static bool SignLow(BitArray b)
        {
            return 2 * (b.CountOnes + 1) > b.Length; //equivalent to (b.CountOnes > b.Length / 2 - 1) but without a division
        }

        public static BitArray FindFirstWeights(BitArray input, BitArray test, Func<BitArray, bool> activation)
        {
            return FindWeights(input, test, activation).FirstOrDefault();
        }

        public static BitArray FindFirstWeights(BitArray input, BitArray test)
        {
            return FindWeights(input, test, SignHigh).FirstOrDefault();
        }


        public static IEnumerable<BitArray> FindWeights(BitArray input, BitArray test)
        {

            return FindWeights(input, test, BinaryNN.SignHigh);
        }


        public static IEnumerable<BitArray> FindWeights(BitArray input, BitArray test, Func<BitArray, bool> activation)
        {
            var W = new BitArray(input.Length * test.Length);
            var output = new BitArray(test.Length);

            var szSearchSpace = (long)MathF.Pow(2, W.Length);
            for (long i = 0; i < szSearchSpace; i++)
            {
                XnorAndActivate(W, input, output, activation);

                if (output == test)
                {
                    yield return W;
                }

                W.Inc();
            }
        }
    }
}
