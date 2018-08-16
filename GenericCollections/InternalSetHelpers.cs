using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCollections
{
    internal static class InternalSetHelpers
    {
        /// <summary>
        /// Expands the hash code.
        /// It works like cycle shift to left
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>
        /// if hash is positive then 2 * hash else 2 * hash + 1
        /// </returns>
        public static uint ExpandHashCode(int hash) => (uint)(((hash >> 31) & 1) | (hash << 1));

        public static int GetAbsoluteHashCode(int hash) => 0x7F_FF_FF_FF & hash;

        public static int GetNextPrime(int number)
        {
            while (true)
            {
                ++number;

                if (number % 2 != 0 || number % 3 != 0)
                {
                    if (IsPrime(number))
                    {
                        return number;
                    }
                }
            }
        }

        private static bool IsPrime(int number)
        {
            var limit = Math.Sqrt(number);

            for (int i = 4; i < limit; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
