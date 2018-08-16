namespace GenericCollections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Set<T> : ISet<T>
    {
        #region Private fields
        private const int DefaultCapacity = 5;

        private readonly IEqualityComparer<T> equalityComparer;

        private Node[] hashBuckets;

        private int filledBucketsCount;

        private int version;

        private Node iterationHead;

        private Node iterationTail;
        #endregion

        #region Constructors

        public Set() : this(EqualityComparer<T>.Default)
        {
        }

        public Set(IEqualityComparer<T> equalityComparer)
        {
            this.equalityComparer =
                equalityComparer ?? throw new ArgumentNullException($"{nameof(equalityComparer)} is null");

            this.hashBuckets = new Node[DefaultCapacity];
            this.iterationHead = this.iterationTail = null;
        }

        public Set(IEnumerable<T> sequence) : this(sequence, EqualityComparer<T>.Default)
        {
        }

        public Set(IEnumerable<T> sequence, IEqualityComparer<T> equalityComparer)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException($"{nameof(sequence)} is null");
            }

            this.equalityComparer =
                equalityComparer ?? throw new ArgumentNullException($"{nameof(equalityComparer)} is null");

            this.hashBuckets = new Node[sequence.Count()];

            foreach (var item in sequence)
            {
                this.Add(item);
            }
        }
        #endregion

        #region Properties
        public int Count { get; private set; }

        public bool IsReadOnly => false;
        #endregion

        #region IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            var currentVersion = this.version;

            if (iterationHead == null)
            {
                yield break;
            }

            var current = iterationHead;

            while (true)
            {
                if (this.version != currentVersion)
                {
                    throw new InvalidOperationException("illegal operation during iteration");
                }

                yield return current.Value;

                if (current == iterationTail)
                {
                    break;
                }

                current = current.NextOrder;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ICollection<T>
        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }
        #endregion

        #region ISet<T>
        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CRUD

        public bool Contains(T item)
        {
            this.UpdateVersion();
            var index = this.GenerateBucketIndex(item);
            var current = this.hashBuckets[index];

            while (current != null)
            {
                if (this.equalityComparer.Equals(item, current.Value))
                {
                    return true;
                }

                current = current.NextHash;
            }

            return false;
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public bool Add(T item)
        {
            return this.Add(new Node(item));
        }
        #endregion

        private bool Add(Node node)
        {
            this.UpdateVersion();
            int index = this.GenerateBucketIndex(node.Value);

            if (this.hashBuckets[index] == null)
            {
                this.CreateBucketAtIndex(index, node);
            }
            else
            {
                Node current = this.hashBuckets[index];

                while (true)
                {
                    if (equalityComparer.Equals(node.Value, current.Value))
                    {
                        return false;
                    }

                    if (current.NextHash == null)
                    {
                        break;
                    }

                    current = current.NextHash;
                }

                current.NextHash = node;
            }
            
            this.AddToIterationList(node);
            this.Count++;
            return true;
        }

        #region Private methods
        private static int GetHashCodeConsideringNulls(T item)
        {
            return item == null ? 0 : InternalSetHelpers.GetAbsoluteHashCode(item.GetHashCode());
        }

        private void AddToIterationList(Node item)
        {
            if (iterationHead == null)
            {
                iterationHead = iterationTail = item;
                return;
            }

            this.iterationTail.NextOrder = item;
            item.PreviousOrder = this.iterationTail;
            this.iterationTail = item;
        }

        private int GenerateBucketIndex(T item)
        {
            return GetHashCodeConsideringNulls(item) % this.hashBuckets.Length;
        }

        private void CreateBucketAtIndex(int index, Node item)
        {
            this.hashBuckets[index] = item;
            this.filledBucketsCount++;

            if (this.IsBucketArrayFilled()) 
            {
                this.ResizeBuckets();
            }
        }

        private void ResizeBuckets()
        {
            int newSize = InternalSetHelpers.GetNextPrime(this.filledBucketsCount);

            Node[] temp = this.hashBuckets;
            this.hashBuckets = new Node[newSize];
            this.filledBucketsCount = 0;

            foreach (var item in temp)
            {
                Node current = item;

                while (current != null)
                {
                    this.Add(current);
                    current = current.NextHash;
                }
            }
        }

        private bool IsBucketArrayFilled() => this.hashBuckets.Length == this.filledBucketsCount;

        private void UpdateVersion()
        {
            unchecked
            {
                this.version++;
            }
        }

        #endregion

        private class Node
        {
            public Node(T value)
            {
                this.Value = value;
            }

            public T Value { get; }

            public Node NextOrder { get; set; }

            public Node PreviousOrder { get; set; }

            public Node NextHash { get; set; }

            public override int GetHashCode()
            {
                return this.Value.GetHashCode();
            }
        }
    }
}
