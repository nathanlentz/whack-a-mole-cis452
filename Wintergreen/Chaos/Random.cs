using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintergreen.Chaos
{
    public static class Random
    {
        private static System.Random _rand;

        static Random()
        {
            int seed = (int)System.DateTime.Now.Ticks;
            _rand = new System.Random(seed);
        }

        /// <summary>
        /// Returns a pseudorandom <see cref="int"/> in the range of <c>0</c> to maxValue.
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int Integer(int maxValue)
        {
            return Integer(0, maxValue);
        }

        /// <summary>
        /// Returns a pseudorandom <see cref="int"/> in the range of minValue to maxValue.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int Integer(int minValue, int maxValue)
        {
            return _rand.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a pseudorandom <see cref="float"/> in the range of <c>0</c> to maxValue.
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static float Float(float maxValue)
        {
            return Float(0, maxValue);
        }

        /// <summary>
        /// Returns a pseudorandom <see cref="float"/> in the range of minValue to maxValue.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static float Float(float minValue, float maxValue)
        {
            float multiple = maxValue - minValue;
            float difference = minValue - 0;
            return Convert.ToSingle(_rand.NextDouble() * multiple + difference);
        }

        /// <summary>
        /// Returns a pseudorandom <see cref="double"/> in the range of <c>0</c> to maxValue.
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static double Double(double maxValue)
        {
            return Double(0, maxValue);
        }

        /// <summary>
        /// Returns a pseudorandom <see cref="double"/> in the range of minValue to maxValue.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static double Double(double minValue, double maxValue)
        {
            double multiple = maxValue - minValue;
            double difference = minValue - 0;
            return _rand.NextDouble() * multiple + difference;
        }
    }
}
