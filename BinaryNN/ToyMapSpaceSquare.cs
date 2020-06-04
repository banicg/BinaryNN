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
    class ToyMapSpaceSquare : IToy
    {
        static Func<int, bool> RndInit = (i) => Randomizer.NextDouble() > 0.4;

        public void Run()
        {
           
            var csvFile = "Results.csv";
            if (File.Exists(csvFile))
                File.Delete(csvFile);

            var table = new ConsoleTable("Input Size", "Output Size", "Search Size", "Solutions Found", "% Of Space", "Search Time");
            File.AppendAllText(csvFile, "Input Size,Output Size,Search Size,Solutions Found,% Of Space,Search Time\r\n");
            int sizeFomMax = 36;

            var searchJobs = new List<SearchTask>();
            for (int szFrom = 1; szFrom <= sizeFomMax; szFrom++)
            {
                for (int szTo = 1; szTo <= szFrom && szFrom * szTo <= sizeFomMax; szTo++)
                {
                    //if (szFrom == 17 && szTo == 2)
                    //if (szFrom * szTo > 32)
                        searchJobs.Add(new SearchTask { SzFrom = szFrom, SzTo = szTo });
                }
            }

            var jobs = new ConcurrentQueue<SearchTask>();
            foreach (var searchJob in searchJobs.OrderBy(o => o.SzSearchSpace))
            {
                jobs.Enqueue(searchJob);
            }

            Parallel.Invoke(Progress, RunJob, RunJob);//, RunJob, RunJob, RunJob, RunJob);

            Console.ReadLine();

            void Progress()
            {

                while (true)
                {
                    Console.Clear();

                    lock (csvFile)
                        table.Write();

                    var compJobs = searchJobs.Where(j => j.Elapsed != null).Count();
                    var waitingJobs = searchJobs.Where(j => j.Elapsed == null && j.Progress == 0).Count();
                    var busyJobs = searchJobs.Where(j => j.Elapsed == null && j.Progress > 0);

                    foreach (var busyJob in busyJobs)
                    {
                        Console.WriteLine($"Searching... (szFrom={busyJob.SzFrom}, szTo={busyJob.SzTo}, searchSpace={busyJob.SzSearchSpace:E}, numSolutions={busyJob.NumSolutions}, progress={busyJob.Progress * 1f / busyJob.SzSearchSpace:P0})");
                    }
                    if (busyJobs.Count() == 0 && waitingJobs == 0)
                        break;

                    Thread.Sleep(5000);
                }

            }

            void RunJob()
            {
                OUTERLOOP:
                while (jobs.TryDequeue(out SearchTask searchJob))
                {
                    try
                    {
                        var mapFrom = new BitArray(searchJob.SzFrom, RndInit);
                        var mapToTarget = new BitArray(searchJob.SzTo, RndInit);
                        var mapToActual = new BitArray(mapToTarget.Length);
                        var W1 = new BitArray(mapFrom.Length * mapToTarget.Length);


                        long numSolutions = 0;
                        var sw = Stopwatch.StartNew();
                        var prevStr = "";
                        for (long iLoop = 0; iLoop < searchJob.SzSearchSpace; iLoop++)
                        {
                            searchJob.Progress = iLoop;

                            BinaryNN.XnorAndActivate(W1, mapFrom, mapToActual, BinaryNN.SignHigh);

                            if (mapToTarget == mapToActual)
                                numSolutions++;

                            try
                            {
                                W1.Inc();

                            }
                            catch (OverflowException ove)
                            {
                                File.AppendAllText($"IntError-OVE.txt", $"SzFrom={searchJob.SzFrom},SzTo={searchJob.SzTo},SzSearchSpace={searchJob.SzSearchSpace},i={iLoop},prevStr={prevStr}\r\n{ove}\r\n");
                                goto OUTERLOOP;
                            }

                            if (iLoop % 10000 == 0)
                                prevStr = $"@{iLoop} => {W1}";

                            searchJob.NumSolutions = numSolutions;
                        }
                        sw.Stop();
                        searchJob.Elapsed = sw.Elapsed;

                        lock (csvFile)
                        {
                            File.AppendAllText(csvFile, $"{mapFrom.Length},{mapToTarget.Length},{searchJob.SzSearchSpace},{numSolutions},{ numSolutions * 1f / searchJob.SzSearchSpace:P1},\"{sw.Elapsed}\"\r\n");
                            table.AddRow(mapFrom.Length, mapToTarget.Length, searchJob.SzSearchSpace, numSolutions, $"{numSolutions * 1f / searchJob.SzSearchSpace:P1}", sw.Elapsed);
                        }
                    }
                    catch (Exception e)
                    {
                        File.AppendAllText($"IntError.txt", $"{e}\r\n");
                    }
                }
            }
        }


    }

    public class SearchTask
    {
        public int SzFrom { get; set; }
        public int SzTo { get; set; }
        public long SzSearchSpace => (long)MathF.Pow(2, SzFrom * SzTo) - 1;
        public long? Progress { get; set; }
        public long? NumSolutions { get; set; }
        public TimeSpan? Elapsed { get; set; }
    }
}
