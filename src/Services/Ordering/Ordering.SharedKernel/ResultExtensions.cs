namespace Ordering.SharedKernel;

public static class ResultExtensions
{
    public static Result<TResult> Map<T, TResult>(
        this Result<T> result, Func<T, TResult> mapper) =>
        result.IsSuccess ? Result<TResult>.Success(mapper(result.Value!))
                         : Result<TResult>.Fail(result.Error!, result.ErrorCategory!);

    public static Result<TResult> Bind<T, TResult>(
        this Result<T> result, Func<T, Result<TResult>> binder) =>
        result.IsSuccess ? binder(result.Value!)
                         : Result<TResult>.Fail(result.Error!, result.ErrorCategory!);
}

