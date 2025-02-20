namespace Calculator;

public class CachedCalculator : BascisCalculator
{
    private readonly Dictionary<string, object> _cache = new();

    private T GetOrAddToCache<T>(Func<T> calculation, params object[] keys)
    {
        var key = string.Join("_", keys);
        if (_cache.TryGetValue(key, out var cachedValue))
            return (T)cachedValue;

        var result = calculation() ?? throw new InvalidOperationException($"Calculation returned null for {key}");
        _cache[key] = result;
        return result;
    }

    public override int Add(int a, int b)
    {
        return GetOrAddToCache(() => base.Add(a, b), nameof(Add), a, b);
    }

    public override int Subtract(int a, int b)
    {
        return GetOrAddToCache(() => base.Subtract(a, b), nameof(Subtract), a, b);
    }

    public override int Multiply(int a, int b)
    {
        return GetOrAddToCache(() => base.Multiply(a, b), nameof(Multiply), a, b);
    }

    public override int Divide(int a, int b)
    {
        return GetOrAddToCache(() => base.Divide(a, b), nameof(Divide), a, b);
    }

    public override int Factorial(int n)
    {
        return GetOrAddToCache(() => base.Factorial(n), nameof(Factorial), n);
    }

    public override bool IsPrime(int candidate)
    {
        return GetOrAddToCache(() => base.IsPrime(candidate), nameof(IsPrime), candidate);
    }
}