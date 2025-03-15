using System.Reflection;
using Calculator;

namespace Tests.tests;

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
        Assert.That(result1, Is.EqualTo(result2));
    }

    [Test]
    public void Subtract()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_calc.Subtract(10, 5), Is.EqualTo(5));
            Assert.That(_calc.Subtract(5, 10), Is.EqualTo(-5));
        });
    }


    [Test]
    public void Multiply()
    {
        var result1 = _calc.Multiply(4, 5);
        var result2 = _calc.Multiply(4, 5);
        Assert.That(result1, Is.EqualTo(result2));
    }

    [Test]
    public void Divide()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_calc.Divide(10, 2), Is.EqualTo(5));
            Assert.That(_calc.Divide(9, 3), Is.EqualTo(3));
        });
    }

    [Test]
    public void DivideByZero()
    {
        Assert.Throws<DivideByZeroException>(() => _calc.Divide(10, 0));
    }

    [Test]
    public void Factorial_ShouldReturnCachedResult_WhenCalledTwice()
    {
        var result1 = _calc.Factorial(5);
        var result2 = _calc.Factorial(5);
        Assert.That(result1, Is.EqualTo(result2));
    }

    [Test]
    public void Factorial_ShouldReturnCorrectResult()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_calc.Factorial(0), Is.EqualTo(1));
            Assert.That(_calc.Factorial(1), Is.EqualTo(1));
            Assert.That(_calc.Factorial(5), Is.EqualTo(120));
        });
    }

    [Test]
    public void Factorial_NegativeNumber_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _calc.Factorial(-5));
    }
    
    [Test]
    public void Factorial_ShouldUseCache_ForLargeNumbers()
    {
        var result1 = _calc.Factorial(10); 
        var result2 = _calc.Factorial(10); 

        Assert.That(result1, Is.EqualTo(result2));
    }

    [Test]
    public void IsPrime_ShouldReturnCachedResult_WhenCalledTwice()
    {
        var result1 = _calc.IsPrime(7);
        var result2 = _calc.IsPrime(7);
        Assert.That(result1, Is.EqualTo(result2));
    }

    [Test]
    public void IsPrime_ShouldReturnTrueForPrimeNumbers()
    {
        
        Assert.Multiple(() =>
        {
            Assert.That(_calc.IsPrime(2), Is.True);
            Assert.That(_calc.IsPrime(3), Is.True);
            Assert.That(_calc.IsPrime(5), Is.True);
            Assert.That(_calc.IsPrime(7), Is.True);
            Assert.That(_calc.IsPrime(11), Is.True);
        });
        
    }

    [Test]
    public void IsPrime_ShouldReturnFalseForNonPrimeNumbers()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_calc.IsPrime(1), Is.False);
            Assert.That(_calc.IsPrime(4), Is.False);
            Assert.That(_calc.IsPrime(6), Is.False);
            Assert.That(_calc.IsPrime(9), Is.False);
            Assert.That(_calc.IsPrime(15), Is.False);
        });
        
    }
    
    [Test]
    public void IsPrime_ShouldUseCache()
    {
        var result1 = _calc.IsPrime(101);
        var result2 = _calc.IsPrime(101);

        Assert.That(result1, Is.EqualTo(result2));
    }
    
    
    [Test]
    public void Add_ShouldUseCache()
    {
        // Første kald udføres normalt
        var result1 = _calc.Add(100, 200);

        // Andet kald bør komme fra cache
        var result2 = _calc.Add(100, 200);

        Assert.That(result1, Is.EqualTo(result2), "Cache burde have gemt værdien.");
    }
    
    [Test]
    public void Add_ShouldHandleLargeNumbers()
    {
        var result1 = _calc.Add(int.MaxValue, -1);
        var result2 = _calc.Add(int.MaxValue, -1);

        Assert.That(result1, Is.EqualTo(result2), "Add skal kunne håndtere store tal.");
    }
    
    [Test]
    public void Multiply_ShouldUseCache()
    {
        var result1 = _calc.Multiply(12, 12);
        var result2 = _calc.Multiply(12, 12);

        Assert.That(result1, Is.EqualTo(result2), "Multiply bør bruge cache.");
    }
    
    [Test]
    public void Multiply_ShouldReturnZero_WhenMultiplyingByZero()
    {
        var result = _calc.Multiply(12345, 0);
        Assert.That(result, Is.EqualTo(0), "Enhver værdi ganget med 0 skal være 0.");
    }

}