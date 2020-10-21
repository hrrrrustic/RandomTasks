using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class SimplePushing<T> : IAddingCompletableCollection<T>
    {
        private readonly Queue<T> _items = new();
        public bool AddingCompleted { get; private set; }

        public bool IsEmpty => _items.Count == 0;

        public void CompletePushing()
        {
            AddingCompleted = true;
        }

        public T GetItem()
        {
            return _items.Dequeue();
        }

        public void PushItem(T item)
        {
            _items.Enqueue(item);
        }
    }
}
