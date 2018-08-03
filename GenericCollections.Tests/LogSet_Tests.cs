using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GenericCollections.Tests
{
    [TestFixture]
    public class LogSet_Tests
    {
        [TestCase(new int[]{3, 4, 4, 8, 4, 0, 9, 0}, 4, ExpectedResult = true)]
        [TestCase(new int[] { 3, 4, 4, 8, 4, 0, 9, 0 }, 11, ExpectedResult = false)]
        [TestCase(new int[] { }, 4, ExpectedResult = false)]
        public bool Can_Do_Contains(int[] array, int expected)
        {
            return new LogSet<int>(array).Contains(expected);
        }

        [TestCase(
            new int[] { 3, 4, 4, 8, 4, 0, 9, 0 },
            new int[] { 3, 4, 8, 0, 9},
            4, ExpectedResult = false)]
        [TestCase(
            new int[] { 3, 4, 4, 8, 4, 0, 9, 0 },
            new int[] { 3, 4, 8, 0, 9, 11},
            11,
            ExpectedResult = true)]
        [TestCase(new int[] { },
            new int[] { 4 },
            4, ExpectedResult = true)]
        public bool Can_Add(int[] array, int[] expectedArray, int item)
        {
            LogSet<int> set = new LogSet<int>(array);
            bool result = set.Add(item);

            CollectionAssert.AreEqual(expectedArray, set.ToArray());
            return result;
        }
    }
}
