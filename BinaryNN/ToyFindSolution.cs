using System;

namespace BinaryNN
{
    class ToyFindSolution : IToy
    {

        public void Run()
        {
            try
            {

                var input = new BitArray(new int[] { 0b1010_0110 },8);
                var output = new BitArray(4);
                var W = new BitArray(32);//0000 0000 0110 0000 0000 0110
                var test = new BitArray(new int[] { 0b0101 }, 4);

                var szSearchSpace = (long)MathF.Pow(2, W.Length) - 1;
                for (long i = 0; i < szSearchSpace; i++)
                {
                    BinaryNN.XnorAndActivate(W, input, output, BinaryNN.SignHigh);

                    if (output == test)
                    {
                        Console.WriteLine($"Input: " + input);
                        Console.WriteLine($"Output: " + test);
                        Console.WriteLine($"Weights: " + W);
                        Console.WriteLine("");
                        Console.WriteLine("Press 'Enter' to find the next solution....");
                        Console.ReadLine();
                    }

                    W.Inc();
                }

                Console.WriteLine(W);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
        }

        public void Run1()
        {
            try
            {

                var input = new BitArray(new int[] { 0b1010 }, 4);
                var output = new BitArray(4);
                var W = new BitArray(16);
                var test = new BitArray(new int[] { 0b0101 }, 4);

                var szSearchSpace = (long)MathF.Pow(2, W.Length) - 1;
                for (long i = 0; i < szSearchSpace; i++)
                {
                    BinaryNN.XnorAndActivate(W, input, output, BinaryNN.SignHigh);

                    if (output == test)
                    {
                        Console.WriteLine(W);
                        Console.ReadLine();
                    }

                    W.Inc();
                }

                Console.WriteLine(W);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
        }
    }
}
