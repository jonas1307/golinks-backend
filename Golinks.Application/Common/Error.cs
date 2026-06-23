namespace Golinks.Application.Common;

public enum ErrorType { NotFound, Conflict, Validation, Failure, Gone }

public record Error(ErrorType Type, string Description)
{
    public static Error NotFound(string description) => new(ErrorType.NotFound, description);
    public static Error Conflict(string description) => new(ErrorType.Conflict, description);
    public static Error Validation(string description) => new(ErrorType.Validation, description);
    public static Error Failure(string description) => new(ErrorType.Failure, description);
    public static Error Gone(string description) => new(ErrorType.Gone, description);
}
