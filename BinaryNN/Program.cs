using ConsoleTables;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace BinaryNN
{


    static class Program
    {

        static void Main(string[] args)
        {
            new ToyMapSpaceSquare().Run();
            //new ToyFindSolution().Run();            
            //new ToySolveXOR().Run();
        }
    }

}
