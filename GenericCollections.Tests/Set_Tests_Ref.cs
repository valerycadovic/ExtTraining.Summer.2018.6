using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace GenericCollections.Tests
{
    [TestFixture]
    public partial class Set_Tests
    {
        [TestCase(3, 1, ExpectedResult = false)]
        [TestCase(6, -2, ExpectedResult = true)]
        [TestCase(4, 4, ExpectedResult = true)]
        [TestCase(0, 0, ExpectedResult = true)]
        public bool Can_Add_Ref(int x, int y)
        {
            HashSet<RefTypeWithBadHashCode> refSet = new HashSet<RefTypeWithBadHashCode>
            {
                new RefTypeWithBadHashCode(3, 1),
                new RefTypeWithBadHashCode(4, 1),
                new RefTypeWithBadHashCode(2, 2),
                new RefTypeWithBadHashCode(5, -1),
                null
            };

            return refSet.Add(new RefTypeWithBadHashCode(x, y));
        }
    }

    internal class RefTypeWithBadHashCode : IEquatable<RefTypeWithBadHashCode>
    {
        private readonly int x;

        private readonly int y;

        public RefTypeWithBadHashCode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(RefTypeWithBadHashCode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RefTypeWithBadHashCode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return x + y;
            }
        }
    }
}
