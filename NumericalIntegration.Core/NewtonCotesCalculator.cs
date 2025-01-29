namespace NumericalIntegration.Core;

public class NewtonCotesCalculator
{
    private readonly Func<double, double> _function;

    public NewtonCotesCalculator(Func<double, double> function)
    {
        _function = function ?? throw new ArgumentNullException(nameof(function));
    }

    private double Function(double x) => _function(x);
    
    public double TrapezoidalRule(double a, double b)
    {
        var h = b - a;
        return (h / 2) * (Function(a) + Function(b));
    }
    
    public double SimpsonsRule(double a, double b)
    {
        var h = (b - a) / 6;  
        var x0 = a;
        var x1 = (a + b) / 2; 
        var x2 = b;
        return h * (Function(x0) + 4 * Function(x1) + Function(x2));
    }
    
    public static double TrapezoidalRule(Func<double, double> function , double a, double b)
    {
        var h = b - a;
        return (h / 2) * (function(a) + function(b));
    }
    
    public static double SimpsonsRule(Func<double, double> function, double a, double b)
    {
        var h = (b - a) / 6;
        var x0 = a;
        var x1 = (a + b) / 2;
        var x2 = b;
        return h * (function(x0) + 4 * function(x1) + function(x2));
    }
}