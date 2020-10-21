using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class FileBlockHandler
    {
        private readonly IAddingCompletableCollection<IHashedFileBlock> _destination;
        public FileBlockHandler(IAddingCompletableCollection<IHashedFileBlock> destination)
        {
            _destination = destination;
        }
        public void CompletePushing()
        {
            throw new NotImplementedException();
        }

        public IFileBlock GetItem()
        {
            throw new NotImplementedException();
        }

        public void PushItem(IFileBlock item)
        {
            throw new NotImplementedException();
        }

        public void StartHandling()
        {
            throw new NotImplementedException();
        }
    }
    public class HashedFileBlockHandler
    {
        private readonly IAddingCompletableCollection<IHashedFlie> _destination;
        public HashedFileBlockHandler(IAddingCompletableCollection<IHashedFlie> destination)
        {
            _destination = destination;
        }
        public void CompletePushing()
        {
            throw new NotImplementedException();
        }

        public IFileBlock GetItem()
        {
            throw new NotImplementedException();
        }

        public void PushItem(IFileBlock item)
        {
            throw new NotImplementedException();
        }

        public void StartHandling()
        {
            throw new NotImplementedException();
        }
    }
    public class HashedFileHandler
    {
        private readonly IAddingCompletableCollection<IHashedDirectory> _destination;
        public HashedFileHandler(IAddingCompletableCollection<IHashedDirectory> destination)
        {
            _destination = destination;
        }
        public void CompletePushing()
        {
            throw new NotImplementedException();
        }

        public IFileBlock GetItem()
        {
            throw new NotImplementedException();
        }

        public void PushItem(IFileBlock item)
        {
            throw new NotImplementedException();
        }

        public void StartHandling()
        {
            throw new NotImplementedException();
        }
    }
}