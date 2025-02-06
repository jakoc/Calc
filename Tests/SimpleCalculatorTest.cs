using Calculator;

namespace Tests;

public class SimpleCalculatorTest
{
    [Test]
    public void Add()
    {
        // Arrange
        var calc = new SimpleCalculator();
        var a = 2;
        var b = 3;
        
        // Act
        var result = calc.Add(a, b);
        
        // Assert
        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void Subtract()
    {
        // Arrange
        var calc = new SimpleCalculator();
        var a = 2;
        var b = 3;
        
        // Act
        var result = calc.Subtract(a, b);
        
        // Assert
        Assert.That(result, Is.EqualTo(-1));
    }

    [Test]
    public void Multiply()
    {
        // Arrange
        var calc = new SimpleCalculator();
        var a = 2;
        var b = 3;
        
        // Act
        var result = calc.Multiply(a, b);
        
        // Assert
        Assert.That(result, Is.EqualTo(6));
    }
    
    [Test]
    public void Divide()
    {
        // Arrange
        var calc = new SimpleCalculator();
        var a = 6;
        var b = 3;
        
        // Act
        var result = calc.Divide(a, b);
        
        // Assert
        Assert.That(result, Is.EqualTo(2));
    }
    
    [Test]
    public void Divide_ByZero_ShouldThrowException()
    {
        var calc = new SimpleCalculator();
        Assert.Throws<DivideByZeroException>(() => calc.Divide(10, 0));
    }

    [Test]
    public void Factorial()
    {
        // Arrange
        var calc = new SimpleCalculator();
        var a = 3;
        
        // Act
        var result = calc.Factorial(a);
        
        // Assert
        Assert.That(result, Is.EqualTo(6));
    }
    
    [Test]
    public void Factorial_OfZero_ShouldReturnOne()
    {
        var calc = new SimpleCalculator();
        Assert.That(calc.Factorial(0), Is.EqualTo(1));
    }
    
    [Test]
    public void Factorial_Negative_ShouldThrowException()
    {
        var calc = new SimpleCalculator();
        Assert.Throws<ArgumentException>(() => calc.Factorial(-5));
    }

    [Test]
    public void isPrime()
    {
        // Arrange
        var calc = new SimpleCalculator();
        var primeNumber = 7;
        var notPrimeNumber = 8;

        // Act
        var primeResult = calc.IsPrime(primeNumber);
        var notPrimeResult = calc.IsPrime(notPrimeNumber);

        // Assert
        Assert.That(primeResult, Is.True);
        Assert.That(notPrimeResult, Is.False);
    }
    
    [Test]
    public void IsPrime_ShouldReturnFalseForNegativeNumbers()
    {
        var calc = new SimpleCalculator();
        Assert.That(calc.IsPrime(-1), Is.False);
        Assert.That(calc.IsPrime(-5), Is.False);
        Assert.That(calc.IsPrime(-10), Is.False);
    }

    [Test]
    public void IsPrime_ShouldHandleEdgeCases()
    {
        var calc = new SimpleCalculator();
        Assert.That(calc.IsPrime(0), Is.False);
        Assert.That(calc.IsPrime(1), Is.False);
        Assert.That(calc.IsPrime(2), Is.True);
    }
}