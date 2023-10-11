namespace BookWriting.Domain;

public class Option<T>
{
    private readonly T _value;
    private readonly bool _hasValue;

    private Option() => _hasValue = false;

    private Option(T value)
    {
        _value = value;
        _hasValue = true;
    }

    public static Option<T> None => new Option<T>();

    public static Option<T> Some(T value) => new Option<T>(value);

    public bool HasValue => _hasValue;

    public T Value
        => !_hasValue ? throw new InvalidOperationException("Option has no value.") : _value;

    public Option<TResult> Map<TResult>(Func<T, TResult> func)
        => !_hasValue ? Option<TResult>.None : Option<TResult>.Some(func(_value));
}