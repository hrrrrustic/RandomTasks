namespace FolderComparer
{
    public interface IValidator<T>
    {
        IValidationResult Validate(params T?[] items);
        IValidationResult Validate(T? item);
    }
}
