using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class WaitConcurrentQueue<T> : IAddingCompletableCollection<T>
    {
        private readonly Queue<T> _queue = new();
        private readonly AutoResetEvent _enqueueResetEvent = new(true);
        private readonly AutoResetEvent _completeAddingResetEvent = new(false);
        private readonly AutoResetEvent[] _resetEvents;
        private bool _addingComplete;
        private readonly object _locker = new object();
        public WaitConcurrentQueue()
        {
            _resetEvents = new[] { _enqueueResetEvent, _completeAddingResetEvent };
        }
        public bool IsEmpty => throw new Exception();
        public bool AddingCompleted => _addingComplete;

        public void PushItem(T item)
        {
            lock (_locker)
                _queue.Enqueue(item);

            _enqueueResetEvent.Set();
        }

        public T GetItem()
        {
            AutoResetEvent.WaitAny(_resetEvents);

            if (_addingComplete)
                if (_queue.Count != 0)
                    return _queue.Dequeue();
                else
                    throw new Exception();

            lock (_locker)
            {
                if (_queue.Count == 0)
                    _enqueueResetEvent.Reset();

                var item = _queue.Dequeue();

                return item;
            }
        }

        public void CompletePushing()
        {
            if (_addingComplete)
                throw new Exception();

            _addingComplete = true;
            _completeAddingResetEvent.Set();
        }
    }
}
