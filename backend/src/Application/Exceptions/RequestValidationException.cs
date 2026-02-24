namespace Application.Exceptions;

public sealed class RequestValidationException : Exception
{
    public RequestValidationException(IDictionary<string, string[]> errors, string? message = null)
        : base(message ?? "Validation failed.")
    {
        Errors = new Dictionary<string, string[]>(errors, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}
