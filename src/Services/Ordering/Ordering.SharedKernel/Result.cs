namespace Ordering.SharedKernel;

public sealed record Result<T>
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T? Value { get; }

    public string? Error { get; }

    public ErrorCategory? ErrorCategory { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = null;
        ErrorCategory = null;
    }

    private Result(string error, ErrorCategory? errorCategory)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
        ErrorCategory = errorCategory;
    }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Fail(string error, ErrorCategory? errorCategory) 
        => new(error, errorCategory);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, TResult> onFailure)
        => IsSuccess ? onSuccess(Value!) : onFailure(Error!);
}
