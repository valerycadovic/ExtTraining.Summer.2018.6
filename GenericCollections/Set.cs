using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomCollections.Generic;

namespace GenericCollections
{
    public sealed class LogSet<T> : ISet<T>
    {
        private readonly BinarySearchTree<T> searchTree;

        private readonly CustomCollections.Generic.Queue<T> values;

        private int capacity;

        private int version;
    
        #region Constructors

        public LogSet()
        {
            this.searchTree = new BinarySearchTree<T>();
            this.values = new CustomCollections.Generic.Queue<T>();
        }

        public LogSet(IEnumerable<T> sequence) : this()
        {
            if (sequence == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var item in sequence)
            {
                this.Add(item);
            }
        }
        #endregion

        #region
        public IEnumerator<T> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            foreach (var item in other)
            {
                Add(item);
            }
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

        public bool Add(T item)
        {
            if (this.searchTree.Add(item))
            {
                values.Enqueue(item);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            this.searchTree.Clear();
        }

        public bool Contains(T item)
        {
            if (this.searchTree.Contains(item))
            {
                return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            values.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public int Count => this.searchTree.Count;

        public bool IsReadOnly => false;

        #endregion
    }
}
