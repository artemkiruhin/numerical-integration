namespace NumericalIntegration.Core.Models.Results;

public class MethodResult
{
    public double Result { get; set; }
    public double Error { get; set; }
    public double ExecutionTime { get; set; }
    public int Segments { get; set; }
}