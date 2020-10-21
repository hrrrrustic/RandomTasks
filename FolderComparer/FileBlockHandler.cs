using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace FolderComparer
{
    public class FileBlockHandler
    {
        private readonly IProducerConsumerCollection<IHashedFileBlock> _destination;
        public FileBlockHandler(IProducerConsumerCollection<IHashedFileBlock> destination)
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
        private readonly IProducerConsumerCollection<IHashedFlie> _destination;
        public HashedFileBlockHandler(IProducerConsumerCollection<IHashedFlie> destination)
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
        private readonly IProducerConsumerCollection<IHashedDirectory> _destination;
        public HashedFileHandler(IProducerConsumerCollection<IHashedDirectory> destination)
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