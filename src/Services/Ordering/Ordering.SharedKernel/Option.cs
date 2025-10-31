namespace Ordering.SharedKernel;

public sealed record Option<T>
{
    private readonly T _value;

    public bool HasValue { get; }

    public T Value => HasValue
        ? _value
        : throw new InvalidOperationException("No value present");

    private Option(T value)
    {
        _value = value;
        HasValue = true;
    }

    private Option()
    {
        _value = default!;
        HasValue = false;
    }

    public static Option<T> Some(T value) => new(value);
    public static Option<T> None() => new();

    // Functor: map (Select in LINQ)
    public Option<TResult> Map<TResult>(Func<T, TResult> mapper) =>
        HasValue ? Option<TResult>.Some(mapper(_value)) : Option<TResult>.None();

    // Monad: bind (SelectMany in LINQ)
    public Option<TResult> Bind<TResult>(Func<T, Option<TResult>> binder) =>
        HasValue ? binder(_value) : Option<TResult>.None();

    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) =>
        HasValue ? some(_value) : none();
}