using NumericalIntegration.Core;

namespace NumericalIntegration.Tests;

public class NewtonCotesCalculatorTests
{
    // Test functions
    private static double Linear(double x) => x;
    private static double Square(double x) => x * x;
    private static double Constant(double x) => 5;
    private static double Sine(double x) => Math.Sin(x);
    private static double Exponential(double x) => Math.Exp(x);
    private static double Polynomial(double x) => 2 * x * x * x + 3 * x * x + 4 * x + 5;

    // Tests for Trapezoidal Rule (instance method)
    [Theory]
    [InlineData(0, 1, 0.5)] // Integral x dx from 0 to 1 = 0.5
    [InlineData(-1, 1, 0)] // Integral x dx from -1 to 1 = 0
    [InlineData(0, 2, 2)] // Integral x dx from 0 to 2 = 2
    public void TrapezoidalRule_Linear_Function_Returns_Correct_Result(double a, double b, double expected)
    {
        var calculator = new NewtonCotesCalculator(Linear);
        var result = calculator.TrapezoidalRule(a, b);
        Assert.Equal(expected, result, 3);
    }

    [Theory]
    [InlineData(0, 1, 5)] // Integral 5 dx from 0 to 1 = 5
    [InlineData(0, 2, 10)] // Integral 5 dx from 0 to 2 = 10
    [InlineData(-1, 1, 10)] // Integral 5 dx from -1 to 1 = 10
    public void TrapezoidalRule_Constant_Function_Returns_Correct_Result(double a, double b, double expected)
    {
        var calculator = new NewtonCotesCalculator(Constant);
        var result = calculator.TrapezoidalRule(a, b);
        Assert.Equal(expected, result, 3);
    }

    [Theory]
    [InlineData(0, 1, 0.333333, 0.01)] // Integral x^2 dx from 0 to 1 ≈ 1/3
    [InlineData(0, 2, 2.666667, 0.01)] // Integral x^2 dx from 0 to 2 ≈ 8/3
    public void TrapezoidalRule_Square_Function_Returns_Approximate_Result(
        double a, double b, double expected, double tolerance)
    {
        var calculator = new NewtonCotesCalculator(Square);
        var result = calculator.TrapezoidalRule(a, b);
        Assert.Equal(expected, result, 2);
    }

    // Tests for Simpson's Rule (instance method)
    [Theory]
    [InlineData(0, 1, 0.5)] // Integral x dx from 0 to 1 = 0.5
    [InlineData(-1, 1, 0)] // Integral x dx from -1 to 1 = 0
    [InlineData(0, 2, 2)] // Integral x dx from 0 to 2 = 2
    public void SimpsonsRule_Linear_Function_Returns_Correct_Result(double a, double b, double expected)
    {
        var calculator = new NewtonCotesCalculator(Linear);
        var result = calculator.SimpsonsRule(a, b);
        Assert.Equal(expected, result, 3);
    }

    [Theory]
    [InlineData(0, 1, 0.333333)] // Integral x^2 dx from 0 to 1 = 1/3
    [InlineData(0, 2, 2.666667)] // Integral x^2 dx from 0 to 2 = 8/3
    public void SimpsonsRule_Square_Function_Returns_Correct_Result(double a, double b, double expected)
    {
        var calculator = new NewtonCotesCalculator(Square);
        var result = calculator.SimpsonsRule(a, b);
        Assert.Equal(expected, result, 5);
    }

    [Theory]
    [InlineData(0, Math.PI, 2)] // Integral sin(x) dx from 0 to π = 2
    [InlineData(0, Math.PI / 2, 1)] // Integral sin(x) dx from 0 to π/2 = 1
    public void SimpsonsRule_Sine_Function_Returns_Correct_Result(double a, double b, double expected)
    {
        var calculator = new NewtonCotesCalculator(Sine);
        var result = calculator.SimpsonsRule(a, b);
        Assert.Equal(expected, result, 3);
    }

    // Tests for static Trapezoidal Rule method
    [Theory]
    [InlineData(0, 1, 0.5)]
    [InlineData(-1, 1, 0)]
    public void Static_TrapezoidalRule_Linear_Function_Returns_Correct_Result(
        double a, double b, double expected)
    {
        var result = NewtonCotesCalculator.TrapezoidalRule(Linear, a, b);
        Assert.Equal(expected, result, 3);
    }

    [Theory]
    [InlineData(0, 1, 1.718282)] // Integral e^x dx from 0 to 1 ≈ e - 1
    public void Static_TrapezoidalRule_Exponential_Function_Returns_Approximate_Result(
        double a, double b, double expected)
    {
        var result = NewtonCotesCalculator.TrapezoidalRule(Exponential, a, b);
        Assert.Equal(expected, result, 3);
    }

    // Tests for static Simpson's Rule method
    [Theory]
    [InlineData(0, 1, 0.333333)]
    [InlineData(0, 2, 2.666667)]
    public void Static_SimpsonsRule_Square_Function_Returns_Correct_Result(double a, double b, double expected)
    {
        var result = NewtonCotesCalculator.SimpsonsRule(Square, a, b);
        Assert.Equal(expected, result, 5);
    }

    [Theory]
    [InlineData(0, 1, 7.333333)] // Integral (2x^3 + 3x^2 + 4x + 5) dx from 0 to 1
    public void Static_SimpsonsRule_Polynomial_Function_Returns_Correct_Result(
        double a, double b, double expected)
    {
        var result = NewtonCotesCalculator.SimpsonsRule(Polynomial, a, b);
        Assert.Equal(expected, result, 5);
    }

    // Tests for invalid input
    [Fact]
    public void TrapezoidalRule_With_Null_Function_Throws_Exception()
    {
        Assert.Throws<ArgumentNullException>(() => new NewtonCotesCalculator(null));
    }

    [Theory]
    [InlineData(1, 0)] // lower bound greater than upper bound
    public void TrapezoidalRule_With_Invalid_Bounds_Returns_Negative_Area(double a, double b)
    {
        var calculator = new NewtonCotesCalculator(Linear);
        var result = calculator.TrapezoidalRule(a, b);
        Assert.True(result < 0);
    }

    [Theory]
    [InlineData(1, 0)] // lower bound greater than upper bound
    public void SimpsonsRule_With_Invalid_Bounds_Returns_Negative_Area(double a, double b)
    {
        var calculator = new NewtonCotesCalculator(Linear);
        var result = calculator.SimpsonsRule(a, b);
        Assert.True(result < 0);
    }

    // Tests for comparing method accuracy
    [Theory]
    [InlineData(0, 1)]
    [InlineData(0, 2)]
    public void SimpsonsRule_More_Accurate_Than_TrapezoidalRule_For_Square_Function(double a, double b)
    {
        var calculator = new NewtonCotesCalculator(Square);
        var exactValue = (Math.Pow(b, 3) - Math.Pow(a, 3)) / 3;
        
        var trapezoidError = Math.Abs(calculator.TrapezoidalRule(a, b) - exactValue);
        var simpsonError = Math.Abs(calculator.SimpsonsRule(a, b) - exactValue);
        
        Assert.True(simpsonError < trapezoidError);
    }

    // Tests for symmetry
    [Theory]
    [InlineData(-1, 1)]
    [InlineData(-2, 2)]
    public void TrapezoidalRule_Symmetric_For_Even_Function(double a, double b)
    {
        var calculator = new NewtonCotesCalculator(Square);
        var result1 = calculator.TrapezoidalRule(a, b);
        var result2 = calculator.TrapezoidalRule(-b, -a);
        Assert.Equal(result1, result2, 8);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(-2, 2)]
    public void SimpsonsRule_Symmetric_For_Even_Function(double a, double b)
    {
        var calculator = new NewtonCotesCalculator(Square);
        var result1 = calculator.SimpsonsRule(a, b);
        var result2 = calculator.SimpsonsRule(-b, -a);
        Assert.Equal(result1, result2, 8);
    }
}