namespace NumericalIntegration.Core.Models.Results;

public class CalculationResult
{
    public double Result { get; set; }
    public double Error { get; set; }
    public double ExecutionTime { get; set; }
    public string Method { get; set; }
}