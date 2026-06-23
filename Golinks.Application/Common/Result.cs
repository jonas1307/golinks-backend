namespace Golinks.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public Error? Error { get; }

    private Result(T value) { IsSuccess = true; Value = value; }
    private Result(Error error) { IsSuccess = false; Error = error; }

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Error error) => new(error);

    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onError)
        => IsSuccess ? onSuccess(Value!) : onError(Error!);
}

public class Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    private Result() { IsSuccess = true; }
    private Result(Error error) { IsSuccess = false; Error = error; }

    public static Result Ok() => new();
    public static Result Fail(Error error) => new(error);

    public static implicit operator Result(Error error) => new(error);

    public TOut Match<TOut>(Func<TOut> onSuccess, Func<Error, TOut> onError)
        => IsSuccess ? onSuccess() : onError(Error!);
}
