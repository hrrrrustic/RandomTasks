using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public interface IAddingCompletableCollection<T>
    {
        bool IsEmpty { get; }
        bool AddingCompleted { get; }
        void PushItem(T item);
        void CompletePushing();
        T GetItem();
    }
}
