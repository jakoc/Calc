namespace Calculator;

public abstract class BascisCalculator : ICalculator
{
    public virtual int Add(int a, int b) => a + b;
    public virtual int Subtract(int a, int b) => a - b;
    public virtual int Multiply(int a, int b) => a * b;
    public virtual int Divide(int a, int b) => a / b;
    
    public virtual int Factorial(int n)
    {
        if (n < 0) throw new ArgumentException("Factorial is not defined for negative numbers");
        return (n == 0) ? 1 : n * Factorial(n - 1);
    }

    public virtual bool IsPrime(int candidate)
    {
        if (candidate < 2) return false;
        for (int divisor = 2; divisor <= Math.Sqrt(candidate); ++divisor)
        {
            if (candidate % divisor == 0) return false;
        }
        return true;
    }
}