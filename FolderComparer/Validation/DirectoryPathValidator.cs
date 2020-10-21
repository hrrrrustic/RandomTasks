using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class DirectoryPathValidator : IValidator<String>
    {
        public IEnumerator<IFile> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IValidationResult Validate(params string?[] items)
        {
            foreach (var item in items)
                Validate(item);

            throw new Exception();
        }

        public IValidationResult Validate(string? item)
        {
            if (item is null)
                throw new Exception();

            if (!Directory.Exists(item))
                throw new Exception();

            throw new Exception();
        }
    }
}
