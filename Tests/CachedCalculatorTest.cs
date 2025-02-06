using Calculator;

namespace Tests;

public class CachedCalculatorTest
{
    private CachedCalculator _calc;

    [SetUp]
    public void Setup()
    {
        _calc = new CachedCalculator();
    }
    
    [Test]
    public void Add_ShouldReturnCachedResult_WhenCalledTwice()
    {
        
        var result1 = _calc.Add(2, 3);
        var result2 = _calc.Add(2, 3);
        Assert.AreEqual(result1, result2); 
    }
    
    [Test]
    public void Subtract()
    {
        Assert.That(_calc.Subtract(10, 5), Is.EqualTo(5));
        Assert.That(_calc.Subtract(5, 10), Is.EqualTo(-5));
    }
    
     [Test]
    public void Multiply()
    {
        var result1 = _calc.Multiply(4, 5);
        var result2 = _calc.Multiply(4, 5);
        Assert.AreEqual(result1, result2);
    }
    
    [Test]
    public void Divide()
    {
        Assert.That(_calc.Divide(10, 2), Is.EqualTo(5));
        Assert.That(_calc.Divide(9, 3), Is.EqualTo(3));
    }

    public void DivideByZero()
    {
        Assert.Throws<DivideByZeroException>(() => _calc.Divide(10, 0));
    }
    
    [Test]
    public void Factorial_ShouldReturnCachedResult_WhenCalledTwice()
    {
        var result1 = _calc.Factorial(5);
        var result2 = _calc.Factorial(5);
        Assert.AreEqual(result1, result2);
    }

    [Test]
    public void Factorial_ShouldReturnCorrectResult()
    {
        Assert.That(_calc.Factorial(0), Is.EqualTo(1));
        Assert.That(_calc.Factorial(1), Is.EqualTo(1));
        Assert.That(_calc.Factorial(5), Is.EqualTo(120));
    }

    [Test]
    public void Factorial_NegativeNumber_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _calc.Factorial(-5));
    }

    [Test]
    public void IsPrime_ShouldReturnCachedResult_WhenCalledTwice()
    {
        var result1 = _calc.IsPrime(7);
        var result2 = _calc.IsPrime(7);
        Assert.AreEqual(result1, result2);
    }
    
    [Test]
    public void IsPrime_ShouldReturnTrueForPrimeNumbers()
    {
        Assert.That(_calc.IsPrime(2), Is.True);
        Assert.That(_calc.IsPrime(3), Is.True);
        Assert.That(_calc.IsPrime(5), Is.True);
        Assert.That(_calc.IsPrime(7), Is.True);
        Assert.That(_calc.IsPrime(11), Is.True);
    }
    
    [Test]
    public void IsPrime_ShouldReturnFalseForNonPrimeNumbers()
    {
        Assert.That(_calc.IsPrime(1), Is.False);
        Assert.That(_calc.IsPrime(4), Is.False);
        Assert.That(_calc.IsPrime(6), Is.False);
        Assert.That(_calc.IsPrime(9), Is.False);
        Assert.That(_calc.IsPrime(15), Is.False);
    }
    
    
    
}