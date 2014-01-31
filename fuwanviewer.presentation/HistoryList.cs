using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Presentation
{
    /// <summary>
    /// Class representing a short history log, up to maximum size.
    /// </summary>
    /// <remarks>
    /// This class supports only Add() operation, but it also implements IEnumerable and INotifyCollectionChanged.
    /// 
    /// Any insertions after reaching the maximum size, will remove the last item in history. 
    /// If an item already exists, it's moved to front of the list and no duplicate is added.
    /// 
    /// Enumeration returns items in "Most recently used" order.
    /// </remarks>
    /// <typeparam name="T">Type of history entries.</typeparam>
    public class HistoryList<T> : IEnumerable<T>, INotifyCollectionChanged, ICollection<T>
    {
        private LinkedList<T> _linkedList;
        
        public int MaximumSize { get; set; }

        public HistoryList(int maximumSize)
        {
            MaximumSize = maximumSize;
            _linkedList = new LinkedList<T>();
        }

        #region ICollection<T> members

        public int Count { get { return _linkedList.Count; } }
        public bool IsReadOnly { get { return false; } }

        public void Add(T value)
        {
            // if you try to add null, return
            if (typeof(T).IsValueType == false && value == null)
                return;

            // Check if item already exists
            LinkedListNode<T> curNode = _linkedList.First;
            LinkedListNode<T> dupNode = null;
            int index = 0;
            while (curNode != null)
            {
                if (curNode.Value.Equals(value))
                {
                    dupNode = curNode;
                    break;
                }
                else
                {
                    curNode = curNode.Next;
                    index++;
                }
            }

            NotifyCollectionChangedEventArgs eventArgs;
            // if not => add item
            if (dupNode == null)
            {
                _linkedList.AddFirst(value);

                if (_linkedList.Count > MaximumSize)
                {
                    var oldItem = _linkedList.Last.Value;
                    _linkedList.RemoveLast();
                    eventArgs = new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Replace, /*action*/
                        value, /*new item*/
                        oldItem); /*old item*/
                }
                else
                {
                    eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value);
                }
            }
            // else => move item upfront
            else
            {
                if (index == 0) /* move 'front item' to front is no action*/
                    return;

                _linkedList.Remove(dupNode);
                _linkedList.AddFirst(dupNode);
                eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, value /*affected value*/, 0 /*new index*/, index /*old index*/);
            }

            OnCollectionChanged(eventArgs);
        }

        public bool Remove(T item) 
        { 
            var result = _linkedList.Remove(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return result;
        }

        public void Clear() 
        { 
            _linkedList.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item) { return _linkedList.Contains(item); }
        public void CopyTo(T[] array, int arrayIndex) { _linkedList.CopyTo(array, arrayIndex); }

        #endregion // ICollection<T> members

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            var curNode = _linkedList.First;
            for (int i = 0; i < _linkedList.Count; i++)
            {
                yield return curNode.Value;
                curNode = curNode.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion // IEnumerable<T>

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }

        #endregion // INotifyCollectionChanged
    }
}
