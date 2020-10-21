using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public interface IValidator<T>
    {
        IValidationResult Validate(params T?[] items);
        IValidationResult Validate(T? item);
    }
}
