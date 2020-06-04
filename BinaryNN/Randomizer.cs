using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryNN
{

    public static class Randomizer
    {
        private static Random R = new Random(1);

        public static List<object> randomValues = new List<object>();

        public static readonly object LockRoot = new object();

        private static void LogNumber(double next)
        {
            //randomValues.Add(next);
        }
        private static void LogNumber(int next)
        {
            //randomValues.Add(next);
        }
        public static void NewRandom()
        {
            R = new Random();
        }
        public static void NewRandom(int seed)
        {
            R = new Random(seed);
        }

        public static float NextDouble()
        {
            var next = (float)R.NextDouble();
            LogNumber(next);

            return next;
        }

        public static float NextFloat(float max = 1)
        {
            var next = (float)R.NextDouble();
            LogNumber(next);

            return next * max;
        }
        public static float NextFloat(float min, float max = 1)
        {
            var next = (float)R.NextDouble();
            LogNumber(next);

            return min + next * (max - min);
        }
        public static bool NextBool()
        {
            return R.NextDouble() > 0.5;
        }
        public static float RandomGaussian(float mu = 0, float sigma = 1)
        {
            var u1 = (float)R.NextDouble();
            var u2 = (float)R.NextDouble();
            LogNumber(u1);
            LogNumber(u2);

            var rand_std_normal = MathF.Sqrt(-2.0f * MathF.Log(u1)) * MathF.Sin(2.0f * MathF.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }

        public static int NextInt(int v1 = int.MinValue, int v2 = int.MaxValue)
        {
            var next = R.Next(v1, v2);
            LogNumber(next);

            return next;
        }
    }

}
