using System;
using NumericalIntegration.Core.Models;
using org.mariuszgromada.math.mxparser;

namespace NumericalIntegration.Core.Services.Integration;

public class NumericalIntegrator
{
    private readonly string _function;
    private readonly double _a;
    private readonly double _b;
    private int _n;

    public NumericalIntegrator(string function, double a, double b, int n)
    {
        _function = function;
        _a = a;
        _b = b;
        _n = n;
    }

    private double EvaluateFunction(double x)
    {
        var expression = new Expression(_function, new Argument("x", x));
        return expression.calculate();
    }

    public double TrapezoidalMethod()
    {
        var h = (_b - _a) / _n;
        var result = (EvaluateFunction(_a) + EvaluateFunction(_b)) / 2;

        for (var i = 1; i < _n; i++)
        {
            var x = _a + i * h;
            result += EvaluateFunction(x);
        }

        return result * h;
    }

    public double SimpsonMethod()
    {
        var h = (_b - _a) / _n;
        var result = EvaluateFunction(_a) + EvaluateFunction(_b);

        for (var i = 1; i < _n; i += 2)
        {
            var x = _a + i * h;
            result += 4 * EvaluateFunction(x);
        }

        for (var i = 2; i < _n - 1; i += 2)
        {
            var x = _a + i * h;
            result += 2 * EvaluateFunction(x);
        }

        return result * h / 3;
    }

    public double GaussianQuadrature()
    {
        // Используем 5-точечную квадратуру Гаусса
        double[] weights =
        {
            0.2369268850561891, 0.4786286704993665, 0.5688888888888889,
            0.4786286704993665, 0.2369268850561891
        };
        double[] points =
        {
            -0.9061798459386640, -0.5384693101056831, 0.0000000000000000,
            0.5384693101056831, 0.9061798459386640
        };

        double sum = 0;
        var mid = (_b + _a) / 2;
        var diff = (_b - _a) / 2;

        for (var i = 0; i < 5; i++)
        {
            var x = mid + diff * points[i];
            sum += weights[i] * EvaluateFunction(x);
        }

        return sum * diff;
    }

    public double CalculateExactValue()
    {
        // Для простоты используем метод Симпсона с большим количеством точек
        var prevN = _n;
        _n = 10000;
        var result = SimpsonMethod();
        _n = prevN;
        return result;
    }

    public IntegrationErrors CalculateErrors()
    {
        var exactValue = CalculateExactValue();
        return new IntegrationErrors
        {
            TrapezoidalError = Math.Abs(TrapezoidalMethod() - exactValue),
            SimpsonError = Math.Abs(SimpsonMethod() - exactValue),
            GaussianError = Math.Abs(GaussianQuadrature() - exactValue)
        };
    }
}