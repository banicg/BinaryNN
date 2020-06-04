using System;
using System.Collections.Generic;
using System.Text;
using ConsoleTables;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BinaryNN
{
    class ToySingleLayer : IToy
    {
        static Func<int, bool> RndInit = (i) => Randomizer.NextDouble() > 0.5;

        public void Run()
        {
            var input = new BitArray(25, RndInit);
            var output = new BitArray(4);
            var W = new BitArray(100, RndInit);

            BinaryNN.XnorAndActivate(W, input, output, BinaryNN.SignHigh);
            Console.WriteLine($"Input:  {input}");
            Console.WriteLine($"W:      {W}");
            Console.WriteLine($"Output: {output}");
        }
    }
}
