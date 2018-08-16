using NUnit.Framework;

namespace GenericCollections.Tests
{
    [TestFixture]
    public partial class Set_Tests
    {
        [TestCase(3, ExpectedResult = false)]
        [TestCase(9, ExpectedResult = true)]
        [TestCase(8, ExpectedResult = true)]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(-2, ExpectedResult = true)]
        [TestCase(int.MinValue, ExpectedResult = true)]
        [TestCase(int.MaxValue, ExpectedResult = true)]
        [TestCase(-11, ExpectedResult = false)]
        public bool Can_Add_Integer(int number)
        {
            Set<int> intSet = new Set<int> { 3, 4, 1, 7, -11 };

            return intSet.Add(number);
        }

        [Test]
        public void Can_Enumerate_Integer()
        {
            Set<int> intSet = new Set<int>(new int[] { 3, 3, 4, 4, 9, 9, 8, 1, 7, 8, 9, int.MaxValue, int.MaxValue, int.MinValue, int.MinValue});

            CollectionAssert.AreEqual(new int[]{3, 4, 9, 8, 1, 7, int.MaxValue, int.MinValue}, intSet);
        }
    }
}
